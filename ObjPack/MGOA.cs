using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//--------------------------------------------------------------------------------------
using GlobalClass;
using System.Diagnostics;
using System.Collections;
using ObjPack;
//--------------------------------------------------------------------------------------
namespace MGOA_Pack
{
    public class MGOA
    {
        //------------------------------------------------------------------------------
        //静态变量定义
        /*public static List<MGOA> ModuleList = new List<MGOA>();
        //------------------------------------------------------------------------------
        public static void Initialize()
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From Module Order By ID");
            if (dr.Count <= 0) return;

            Debug.WriteLine("******** Module ********");
            //ModuleList = new Module[dr.Count];
            int i = 0;
            while (dr.Read())
            {
                Debug.WriteLine("---------------------");
                ModuleList.Add(new MGOA(null, dr));
            }
            //参数绑定单位设定
            //ParaGroup.BindUnit();
        }
        //------------------------------------------------------------------------------
        public static MGOA Find(String Full_SN_For_Find)
        {
            String[] snList = Full_SN_For_Find.Split('.');
            return FindChild(ModuleList, snList);
        }*/
        //------------------------------------------------------------------------------
        public static MGOA Find(List<MGOA> ModuleList, String Full_SN_For_Find)
        {
            String[] snList = Full_SN_For_Find.Split('.');
            return FindChild(ModuleList, snList);
        }
        //------------------------------------------------------------------------------
        private static MGOA FindChild(List<MGOA> ItemList, String[] snList)
        {
            if (ItemList == null) return null;
            MGOA Mgoa = null;
            for (int i = 0; i < ItemList.Count; i++)
            {
                Mgoa = ItemList[i]._Find(snList);
                if (Mgoa != null) return Mgoa;
            }
            return null;
        }
        //------------------------------------------------------------------------------
        public UInt32 ID = 0;
        public String SN;
        public UInt32 FullID = 0;
        public String FullSN;
        public int Level = 0;
        public List<MGOA> Childs = new List<MGOA>();
        public int ViewID = -1;
        public List<AlarmPara> mAlarmPara { get; set; }
        //------------------------------------------------------------------------------
        public MGOA(MGOA Parent, DBReader dReader, int ViewID,List<AlarmPara> list=null)
        {
            this.ViewID = ViewID;
            this.mAlarmPara = list;
            NewMGOA(Parent, dReader);
        }
        //------------------------------------------------------------------------------
        public MGOA(MGOA Parent, DBReader dReader, List<AlarmPara> list=null)
        {
            this.mAlarmPara = list;
            NewMGOA(Parent, dReader);
        }
        //------------------------------------------------------------------------------
        private void NewMGOA(MGOA Parent, DBReader dReader)
        {
            ID = (UInt32)dReader.GetInt("ID");
            SN = dReader.GetStr("SN");
            Init(Parent, dReader);

            //生成子对象，直到无Child定义
            if (dReader.hasField("Child"))
            {
                String Str = dReader.GetStr("Child");
                String[] sList = Str.Split(',');
                for (int i = 0; i < sList.Length; i++)
                    CreateChild(sList[i]);
            }
        }
        //------------------------------------------------------------------------------
        public MGOA(MGOA Parent, DBReader dReader, UInt32 ID_Att, String SN_Att)
        {
            this.ID = ID_Att;
            this.SN = SN_Att;
            Init(Parent, dReader);
        }
        //------------------------------------------------------------------------------
        private void Init(MGOA Parent, DBReader dReader)
        {
            FullSN = (Parent == null) ? SN : Parent.FullSN + "." + SN;
            Level = (Parent == null) ? 0 : Parent.Level + 1;
            switch (Level)
            {
                case 0: FullID = ID * 0xFFFF; break;
                case 1: FullID = Parent.FullID + ID * 32 * 32; break;
                case 2: FullID = Parent.FullID + ID * 32; break;
                default: FullID = Parent.FullID + ID; break;
            }

            //InitLanguage(); //生成语言文本

            //Debug.WriteLine(FullID + ":" + FullSN);
        }
        //------------------------------------------------------------------------------
        public void InitLanguage()
        {   //生成语言文本
            if (Level > 2) return;
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From Lang Where Section='MGOA' And Key='" + FullSN + "'");
            if (dr.Read()) return;
            DBConnect.SYS.ExecSQL("Insert Into Lang (Section, Key, EN) Values('MGOA','" + FullSN + "','" + SN + "')");
        }
        //------------------------------------------------------------------------------
        protected virtual void CreateChild(String Name)
        {//读入Object
            DBReader dr = new DBReader(DBConnect.SYS);

            if (GLB.Same(Name, "ParaItem"))
                dr.Select("Select * From " + Name + " Where Path='" + FullSN + "' And ViewID=" + ViewID.ToString() + " Order By ID");
            else
                dr.Select("Select * From " + Name + " Where Path='" + FullSN + "' Order By ID");
            if (dr.Count <= 0) return;

            //Childs = new MGOA[dr.Count];
            int i = 0;
            while (dr.Read())
            {
                Childs.Add(CreateChildItem(Name, dr));
            }
        }
        //------------------------------------------------------------------------------
        private MGOA CreateChildItem(String Name, DBReader dr)
        {
            if (GLB.Same(Name, "Module")) return new MGOA(this, dr, ViewID,mAlarmPara);
            if (GLB.Same(Name, "ParaGroup")) return new ParaGroup(this, dr, ViewID,mAlarmPara);
            if (GLB.Same(Name, "ParaItem")) return new ParaItem(this, dr, ViewID, mAlarmPara);
            if (GLB.Same(Name, "WaveGroup")) return new WaveGroup(this, dr, mAlarmPara);
            if (GLB.Same(Name, "WaveItem")) return new WaveItem(this, dr, mAlarmPara);
            if (GLB.Same(Name, "UnitGroup")) return new UnitGroup(this, dr,mAlarmPara);
            if (GLB.Same(Name, "UnitItem")) return new UnitItem(this, dr, mAlarmPara);
            return new MGOA(this, dr, mAlarmPara);
        }
        //------------------------------------------------------------------------------
        protected MGOA _Find(String[] snList)
        {
            //if (snList[0].Equals("ART")&&SN.Equals("ART")) {
            //    Console.WriteLine(snList);
            //}
            if (Level >= snList.Length) return null;
            if (!GLB.Same(snList[Level], SN)) return null;
            if (Level == snList.Length - 1) return this;    //末层一致

            return FindChild(Childs, snList);
        }
        //------------------------------------------------------------------------------
        public MGOA FindChild(String ChildSN)
        {
            if (Childs == null) return null;
            for (int i = 0; i < Childs.Count; i++)
            {
                if (GLB.Same(Childs[i].SN, ChildSN))
                    return Childs[i];
            }
            return null;
        }
        //------------------------------------------------------------------------------
        public String GetName()
        {
            return Lang.Get("MGOA", FullSN);
        }

        public string GetENName()
        {
            return Lang.Get("MGOA", "EN");
        }
        //------------------------------------------------------------------------------
    }
}
//--------------------------------------------------------------------------------------
