using GlobalClass;
using ObjPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RemoteMonitor
{
    public partial class FrmMonitor : Form
    {

        private List<Device> listDevice = new List<Device>();
        private Dictionary<string, string> dicBedMapping = new Dictionary<string, string>(); 
        public FrmMonitor()
        {
            InitializeComponent();
            this.Load += FrmMonitor_Load;
        }

        /// <summary>
        /// 加载窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void FrmMonitor_Load(object sender, EventArgs e)
        {
            try
            {
                dgMontior.AutoGenerateColumns = false;
                GetBedMapping();
                LoadDeviceInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 加载监护信息
        /// </summary>
        private void LoadDeviceInfo(string strWhere="")
        {
            string strSql = "select * from Device " + strWhere +" order by NUMSORT ASC";
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select(strSql);
            while (dr.Read())
            {
                Device dev = new Device();
                dev.ID = dr.GetInt("ID");
                dev.ARCHIVESID = dr.GetStr("ARCHIVESID");
                dev.BARCODE = dr.GetStr("BARCODE");
                dev.BEDNUM = dr.GetStr("BEDNUM");
                dev.DEVICEID = dr.GetStr("DEVICEID");

                dev.DEVICEIP = dr.GetStr("DEVICEIP");
                dev.DEVICETYPE = dr.GetStr("DEVICETYPE");
                dev.INHOSNUM = dr.GetStr("INHOSNUM");
                dev.PATIENTNAME = dr.GetStr("PATIENTNAME");
                dev.STATUS = dr.GetStr("STATUS");
                dev.NUMSORT = dr.GetInt("NUMSORT");
                if (dicBedMapping.ContainsKey(dev.DEVICEIP))
                {
                    dev.BEDSN = dicBedMapping[dev.DEVICEIP];
                }
                listDevice.Add(dev);
            }
            dgMontior.DataSource = listDevice;
            dgMontior.Refresh();
        }

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                FrmMonitorEdit frmMonitorEdit = new FrmMonitorEdit(null,dicBedMapping);
                frmMonitorEdit.Owner = this;
                frmMonitorEdit.StartPosition = FormStartPosition.CenterScreen;
                if (frmMonitorEdit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    listDevice.Insert(0, frmMonitorEdit.mDev);
                    if (!dicBedMapping.ContainsKey(frmMonitorEdit.mDev.DEVICEIP))
                    {
                        dicBedMapping.Add(frmMonitorEdit.mDev.DEVICEIP, frmMonitorEdit.mDev.BEDSN);
                    }
                }
 
            }
            catch (Exception)
            {
                
            }
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if(dgMontior.SelectedRows.Count<=0)
                {
                    MessageBox.Show("请选择一条记录");
                    return;
                }

                DataGridViewRow row = dgMontior.SelectedRows[0];
                Device dev = row.DataBoundItem as Device;
                if (dev != null)
                {
                    FrmMonitorEdit frmMonitorEdit = new FrmMonitorEdit(dev,dicBedMapping);
                    frmMonitorEdit.Owner = this;
                    frmMonitorEdit.StartPosition = FormStartPosition.CenterScreen;
                    if (frmMonitorEdit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        LoadDeviceInfo();
                    }
                }
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgMontior.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请选择一条记录");
                return;
            }

            DataGridViewRow row = dgMontior.SelectedRows[0];
            Device dev = row.DataBoundItem as Device;
            if (dev != null)
            {
                if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    dgMontior.DataSource = null;
                    DelDevice(dev);
                    listDevice.Remove(dev);

                    if (dicBedMapping.ContainsKey(dev.DEVICEIP) && !string.IsNullOrEmpty(dev.DEVICEIP))
                    {
                        DelBedMapping(dev.DEVICEIP);
                    }
                    dgMontior.DataSource = listDevice;
                    dgMontior.Refresh();
                }
            }
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="dev"></param>
        private void DelDevice(Device dev)
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            string strSql = string.Format(@"delete from Device where ID='{0}'", dev.ID);
            DBConnect.SYS.ExecSQL(strSql);
        }
        
        /// <summary>
        /// 删除设备
        /// </summary>
        private void DelBedMapping(string IP)
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            string strSql = string.Format(@"delete from beddevmapping where IP='{0}'", IP);
            DBConnect.SYS.ExecSQL(strSql);
        }

        /// <summary>
        /// 获取床号
        /// </summary>
        private void GetBedMapping()
        {
            string strSql = "select * from  beddevmapping ";
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select(strSql);
            while (dr.Read())
            {
                string strIP = dr.GetStr("IP");
                string strSN = dr.GetStr("BedSN");
                if (!dicBedMapping.ContainsKey(strIP))
                {
                    dicBedMapping.Add(strIP, strSN);
                }
            }
        }

      
    }
}
