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
    public partial class FrmMonitorEdit : Form
    {

        public Device mDev;
        private Dictionary<string, string> mBedMapping;
        public FrmMonitorEdit(Device dev,Dictionary<string,string> dicBedMapping)
        {
            InitializeComponent();
            mDev = dev;
            mBedMapping = dicBedMapping;
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMonitorEdit_Load(object sender, EventArgs e)
        {

            if (mDev != null)
            {
                this.txbBarcode.Text = mDev.BARCODE;
                this.txbBedSN.Text = mDev.BEDSN;
                this.txbDeviceId.Text = mDev.DEVICEID;
                this.txbIP.Text = mDev.DEVICEIP;
                this.cmbDeviceType.Text = mDev.DEVICETYPE;
                this.txbSort.Text = mDev.NUMSORT.ToString();
            }
            else
            {
                this.cmbDeviceType.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfig_Click(object sender, EventArgs e)
        {
            int numSort = 0;
            if (string.IsNullOrEmpty(txbSort.Text)||! int.TryParse(txbSort.Text, out numSort))
            {
                MessageBox.Show("排序编号不能为空且为整数！");
                return;
            }
            GetDevice();
            if (string.IsNullOrEmpty(mDev.ID.ToString()) || mDev.ID==0)
            {
                AddDevice();
                mDev.ID=GetMaxDeviceID();
            }
            else
            {
                UpdateDevice();
            }

            if (!mBedMapping.ContainsKey(mDev.DEVICEIP))
            {
                AddBedMapping();
            }
            else 
            {
                UpdateBedMapping();
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        #region 设备维护
        /// <summary>
        /// 获取设备信息
        /// </summary>
        private void GetDevice()
        {
            if (mDev == null) mDev = new Device();
            mDev.DEVICETYPE = cmbDeviceType.SelectedItem.ToString();
            mDev.DEVICEIP = txbIP.Text;
            mDev.BEDSN = txbBedSN.Text;
            mDev.BEDNUM = string.Empty;
            mDev.BARCODE = txbBarcode.Text;
            mDev.PATIENTNAME =string.Empty;
            mDev.DEVICEID = txbDeviceId.Text;
            mDev.STATUS = chkStatus.Checked?"1":"0";
            mDev.NUMSORT = int.Parse(txbSort.Text);
        }

        /// <summary>
        /// 增加一个设备
        /// </summary>
        private void AddDevice()
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            string strSql =string.Format( @"insert into Device(DEVICEID,DEVICETYPE,BARCODE,DEVICEIP,INHOSNUM,ARCHIVESID,BEDNUM,PATIENTNAME,STATUS,NUMSORT)
                              VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}') ",
                              mDev.DEVICEID,mDev.DEVICETYPE,mDev.BARCODE,mDev.DEVICEIP,"","",mDev.BEDNUM,mDev.PATIENTNAME,mDev.STATUS,mDev.NUMSORT);
              DBConnect.SYS.ExecSQL(strSql);
        }


        /// <summary>
        /// 更新设备
        /// </summary>
        private void UpdateDevice()
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            string strSql = string.Format(@"update Device set DEVICEID='{0}',DEVICETYPE='{1}',BARCODE='{2}',DEVICEIP='{3}',INHOSNUM='{4}',ARCHIVESID='{5}',STATUS='{6}',NUMSORT='{7}' where ID={8}",
                    mDev.DEVICEID, mDev.DEVICETYPE, mDev.BARCODE, mDev.DEVICEIP, "", "", mDev.STATUS,mDev.NUMSORT, mDev.ID);
            DBConnect.SYS.ExecSQL(strSql);
        }

        /// <summary>
        /// 获取最大的设备信息
        /// </summary>
        private int GetMaxDeviceID()
        {
            int ID = -1;
            string strSql = "select * from Device order by ID Desc";
            DBReader dr = new DBReader(DBConnect.SYS);
            dr.Select(strSql);
            if (dr.Read())
            {
                ID = dr.GetInt("ID");
            }
            return ID;
        }

        #endregion

        #region  设备IP维护
        private void AddBedMapping()
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            string strSql = string.Format(@"insert into BedDevMapping(IP,BedSN)VALUES('{0}','{1}')", mDev.DEVICEIP, mDev.BEDSN);
            DBConnect.SYS.ExecSQL(strSql);
        }
        private void UpdateBedMapping()
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            string strSql = string.Format(@"update  BedDevMapping set BedSN='{0}' where IP='{1}'", mDev.BEDSN, mDev.DEVICEIP);
            DBConnect.SYS.ExecSQL(strSql);
        }
        #endregion
    }
}
