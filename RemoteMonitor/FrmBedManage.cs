using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
//------------------------------------------------------------------------------
using GlobalClass;
//------------------------------------------------------------------------------
namespace RemoteMonitor
{
    public partial class FrmBedManage : Form
    {
        private int ViewID = 0;
        public Boolean IsChanged = false;
        //------------------------------------------------------------------------------
        public FrmBedManage()
        {
            InitializeComponent();
        }
        //------------------------------------------------------------------------------
        private void FrmBedManage_Activated(object sender, EventArgs e)
        {
              IsChanged = false;
              InitParaList();
              InitWaveGroup();
        }
        //------------------------------------------------------------------------------
        private void InitParaList()
        {
            treeView2.Nodes.Clear();
            DBReader dr = new DBReader(DBConnect.SYS);
            //dr.Select("Select * From ViewList Where Type='Para' And ViewID=" + ViewID.ToString() + " Order By ID");
            dr.Select("Select * From ViewList Where Type like 'Para%' And ViewID=-1 And Visible>=0 Order By ID");
            while (dr.Read())
            {
                TreeNode Node = new TreeNode(Lang.Get("MGOA", dr.GetStr("GroupName")));
                Node.Checked = dr.GetBoolean("Visible");
                Node.Name = dr.GetStr("GroupName");
                treeView2.Nodes.Add(Node);
            }
        }
        //------------------------------------------------------------------------------
        private void InitWaveGroup()
        {
            treeView1.Nodes.Clear();
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From ViewList Where Type='WaveGroup' And ViewID=" + ViewID.ToString() + " Order By ID");
            while (dr.Read())
            {
                TreeNode Node = new TreeNode(Lang.Get("MGOA", dr.GetStr("GroupName")));
                Node.Checked = dr.GetBoolean("Visible");
                Node.Name = dr.GetStr("GroupName");
                treeView1.Nodes.Add(Node);

                InitWaveItem(dr.GetStr("GroupName"), Node);
            }
            treeView1.ExpandAll();
        }
        //------------------------------------------------------------------------------
        private void InitWaveItem(String GroupName, TreeNode ParentNode)
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select("Select * From ViewList Where GroupName='" + GroupName + "' And Type='WaveItem' And ViewID=" + ViewID.ToString() + " Order By ID");
            if (dr.Count <= 1) return;
            while (dr.Read())
            {
                 TreeNode Node = new TreeNode(Lang.Get("MGOA", dr.GetStr("GroupName") + "." + dr.GetStr("ItemName")));
                 Node.Checked = dr.GetBoolean("Visible");
                 Node.Name = dr.GetStr("ItemName");
                 ParentNode.Nodes.Add(Node);
            }
        }
        //------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            Save(-1);
            Close();
        }
        //------------------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            Save(ViewID);
            this.Close();
        }
        //------------------------------------------------------------------------------
        private void Save(int SaveID)
        {
            String ViewIDStr = (SaveID < 0) ? " And ViewID>=0" : " And ViewID=" + ViewID.ToString();

            DBConnect Conn = DBConnect.SYS;
            Conn.BeginTransaction();
            //保存参数设置
            for (int i = 0; i < treeView2.Nodes.Count; i++)
            {
                TreeNode Node = treeView2.Nodes[i];
                /*
                String str = "Update ViewList Set Visible=" + (Node.Checked ? "1" : "0")
                    + " Where Type like 'Para%' And GroupName='" + Node.Name + "'" + ViewIDStr;

                Conn.ExecSQL("Update ViewList Set Visible=" + (Node.Checked ? "1" : "0")
                    + " Where Type like 'Para%' And GroupName='" + Node.Name + "'" + ViewIDStr);
                */
                Conn.ExecSQL("Update ViewList Set Visible=" + (Node.Checked ? "1" : "0")
                    + " Where Type like 'Para%' And GroupName='" + Node.Name + "'");
            }
            //保存波形设置
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                TreeNode Node = treeView1.Nodes[i];
                Conn.ExecSQL("Update ViewList Set Visible=" + (Node.Checked ? "1" : "0")
                    + " Where Type='WaveGroup' And GroupName='" + Node.Name + "'" + ViewIDStr);
                for (int ci = 0; ci < Node.Nodes.Count; ci++)
                {
                    TreeNode n = Node.Nodes[ci];
                    Conn.ExecSQL("Update ViewList Set Visible=" + (n.Checked ? "1" : "0")
                        + " Where Type='WaveItem' And GroupName='" + Node.Name
                        + "' And ItemName='" + n.Name + "'" + ViewIDStr);
                }
            }
            //提交
            Conn.Commit();
            IsChanged = true;
        }
        //------------------------------------------------------------------------------
    }
}
