using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//--------------------------------------------------------------------------------------
using GlobalClass;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ObjPack;
//--------------------------------------------------------------------------------------
namespace MGOA_Pack
{
    public class ParaGroup : MGOA
    {
        public static List<ParaGroup> Items = new List<ParaGroup>();
        /*public static void BindUnit()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                ((ParaGroup)Items[i])._BindUnit();
            }
        }*/
        //------------------------------------------------------------------------------
        public String UnitName;
        public UnitGroup Unit;
        public Color dColor;
        public Byte[] ValBuf;
        public ParaItem[] PItems;
        public Boolean NeedSave = false;
        public int Min = int.MaxValue;
        public int Max = int.MinValue;
        public int FullID;
        public List<AlarmPara> mAlarmPara;
        //------------------------------------------------------------------------------
        public ParaGroup(MGOA Parent, DBReader dReader,int ViewID,List<AlarmPara> list)
            : base(Parent, dReader, ViewID, list)
        {
            Items.Add(this);
            UnitName = dReader.GetStr("Unit");
            dColor = dReader.GetColor("Color");
            FullID = dReader.GetInt("FullID");
            //
            PItems = new ParaItem[Childs.Count];
            for (int i = 0; i < Childs.Count; i++)
            {
                PItems[i] = (ParaItem)Childs[i];
                Min = Math.Min(Min, PItems[i].Min);
                Max = Math.Max(Max, PItems[i].Max);
            }
            //
            _BindUnit();   //参数绑定单位设定
            //InitViewList();
        }
        //------------------------------------------------------------------------------
        protected override void CreateChild(string Name)
        {
            base.CreateChild(Name);
            ValBuf = new Byte[Childs.Count*2];
        }
        //------------------------------------------------------------------------------
        public void _BindUnit()
        {
            Unit = UnitGroup.Find(UnitName);
            //Unit = (UnitGroup)MGOA.Find("Unit." + UnitName);
            for (int i = 0; i < Childs.Count; i++)
            {
                if (Unit == null)
                    PItems[i].BindUnit(null);
                else
                    PItems[i].BindUnit((UnitItem)Unit.Childs[0]);
            }
        }
        //------------------------------------------------------------------------------
        public String GetUnitName()
        {
            if (Unit == null) return "";
            return Unit.GetUnitName();
        }
        //------------------------------------------------------------------------------
        public String GetValueString(int Value)
        {
            if (Unit == null || Unit.Childs==null) return Value.ToString();
            return ((UnitItem)Unit.Childs[0]).GetString(Value);
        }
        //------------------------------------------------------------------------------
        public void InitViewList()
        {   //生成参数显示列表，初始化时使用
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From ViewList Where GroupName='" + FullSN + "'");
            if (dr.Read()) return;
            DBConnect.SYS.ExecSQL("Insert Into ViewList (GroupName, Color) Values('" + FullSN + "','" + dColor.ToArgb().ToString() + "')");
        }
        //------------------------------------------------------------------------------
        public void SetValue(Int16[] Data)
        {
            //float[] numbers = new float[] { 1.23f, 2.23f, 3.34f };
            //byte[] result = new byte[numbers.Length * Marshal.SizeOf(numbers[0])];
            IntPtr dPtr = Marshal.UnsafeAddrOfPinnedArrayElement(Data, 0);
            Marshal.Copy(dPtr, ValBuf, 0, ValBuf.Length);
            //
            for (int i = 0; i < Childs.Count; i++)
            {
                //ValData[i] = Data[i];
                PItems[i].SetValue(Data[i]);
            }
            NeedSave = true;
            //Log.d(FullSN + " SetValue " + Data[0].ToString());
        }
        //------------------------------------------------------------------------------
        public static ParaGroup Find(String FullSN)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (GLB.Same(FullSN, Items[i].FullSN)) 
                    return Items[i];
            }
            return null;
        }
        //------------------------------------------------------------------------------
    }
}
//--------------------------------------------------------------------------------------
