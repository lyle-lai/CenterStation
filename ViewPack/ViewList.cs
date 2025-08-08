using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Drawing;
//-----------------------------------------------------------------------
using GlobalClass;
using MGOA_Pack;
using System.Diagnostics;
//-----------------------------------------------------------------------
namespace ViewPack
{
    //-----------------------------------------------------------------------
    public class ViewList : ViewBase
    {
        public List<ViewBase> Childs = new List<ViewBase>();
        //------------------------------------------------------------------------------
        public enum Orientation { Horizontal, Vertical };
        private static Orientation GetOrientation(Xml_Node xNode)
        {
            return GLB.Same(xNode.getStr("Orientation"), "Vertical") ?
                Orientation.Vertical : Orientation.Horizontal;
        }
        public Orientation Ori = Orientation.Horizontal;
        //------------------------------------------------------------------------------
        public ViewList()
        {
            //InitBaseSize();
        }
        //------------------------------------------------------------------------------
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            LayoutView();
        }
        //------------------------------------------------------------------------------
        public void LoadParaList(int ViewID, int vplid, List<MGOA> MList)
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            //dr.Select("Select * From V_ViewList Where Visible=1 And Type='Para_" + vplid.ToString() + "' And ViewID=" + ViewID.ToString());
            dr.Select("Select * From V_ViewList Where Visible=1 And Type='Para_" + vplid.ToString() + "' And ViewID=-1 and visible=1 order by OrderNum");
            while (dr.Read())
            {
                String gn = dr.GetStr("GroupName");
                //ParaGroup PGrp = (ParaGroup)MGOA.Find(dr.GetStr("GroupName"));
                ParaGroup PGrp = (ParaGroup)MGOA.Find(MList, dr.GetStr("GroupName")); 
                ViewParaGroup v = new ViewParaGroup(PGrp, dr.GetStr("Xml"));
                v.Weight = dr.GetF("Weight", 1);
                v.Parent = this;
                v.Visible = true;
                v.Width = 100;
                //Log.d(dr.GetStr("Xml"));
                Childs.Add(v);
            }
            //LayoutView();
        }
        //------------------------------------------------------------------------------
        public List<ViewWaveGroup> LoadWaveList(int ViewID, List<MGOA> MList)
        {
            List<ViewWaveGroup> waveList = new List<ViewWaveGroup>();
            DBReader dr = new DBReader(DBConnect.SYS);
            //dr.Select("Select * From ViewList Where Visible=1 And Type='WaveGroup' And ViewID=" + ViewID.ToString());
            dr.Select("Select * From ViewList Where Visible=1 And Type='WaveGroup' And ViewID=0");
            while (dr.Read())
            {
                //WaveGroup Grp = (WaveGroup)MGOA.Find(dr.GetStr("GroupName"));
                WaveGroup Grp = (WaveGroup)MGOA.Find(MList, dr.GetStr("GroupName"));
                //if (Grp == null) {
                //    foreach (var item in MList)
                //    {
                //        //var a = dr.GetStr("GroupName");
                //        Debug.WriteLine($"item:{item.SN}");
                //    }
                //}
                ViewWaveGroup v = new ViewWaveGroup(Grp, ViewID);
                v.Parent = this;
                v.Visible = true;
                Childs.Add(v);
                waveList.Add(v);
            }
            //LayoutView();
            return waveList;
        }
        //------------------------------------------------------------------------------
        public virtual void LayoutView()
        {
            if (Childs.Count <= 0) return;
            int vTop = 0;

            Double RemWeight = 0;
            for (int i = 0; i < Childs.Count; i++)
                RemWeight += Childs[i].Weight;

            int RemHeight = this.ClientSize.Height;
            for (int i = 0; i < Childs.Count; i++)
            {
                Childs[i].Visible = true;
                Childs[i].Left = 0;
                Childs[i].Top = vTop;
                Childs[i].Width = this.ClientSize.Width;
                //高度
                int H = (int)(Childs[i].Weight * RemHeight / RemWeight);
                Childs[i].Height = H;
                vTop += H;
                RemHeight -= H;
                RemWeight -= Childs[i].Weight;
                //
                if (Childs[i] is ViewList)
                    ((ViewList)Childs[i]).LayoutView();
            }
        }
        //------------------------------------------------------------------------------
        public override void Tick()
        {
            for (int i = 0; i < Childs.Count; i++)
                Childs[i].Tick();
        }
        //------------------------------------------------------------------------------
    }
    //-----------------------------------------------------------------------
}