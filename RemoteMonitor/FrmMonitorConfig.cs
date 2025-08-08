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
    public partial class FrmMonitorConfig : Form
    {

        private List<Device> mlistDevice;
        private Dictionary<string, string> dicBedMapping = new Dictionary<string, string>(); 
        public FrmMonitorConfig(List<Device> list=null)
        {
            InitializeComponent();
            mlistDevice = list;
            this.Load += FrmMonitorConfig_Load;
        }

       private  void FrmMonitorConfig_Load(object sender, EventArgs e)
        {
            try
            {
                if (mlistDevice == null)
                {
                    GetBedMapping();
                    mlistDevice = new List<Device>();
                    LoadDeviceInfo();
                }
                if(mlistDevice!=null)
                {
                    int i = 0;
                    int w= plContent.Width /4;
                    foreach (Device dev in mlistDevice)
                    {
                        CheckBox chk = new CheckBox();
                        chk.Checked=dev.STATUS=="1"?true:false;
                        chk.Text = dev.BEDSN + ":" + dev.PATIENTNAME + ":" + dev.BEDNUM;
                        chk.Width = w;
                        chk.Left = (i % 4) * w;
                        chk.Top = (i / 4) * 30;
                        chk.Tag = dev;
                        chk.Show();
                        plContent.Controls.Add(chk);
                        i++;
                    }
                }
               
            }
            catch (Exception ex)
            {
                
             
            }
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
       /// <summary>
       /// 加载监护信息
       /// </summary>
       private void LoadDeviceInfo(string strWhere = "")
       {
           string strSql = "select * from Device " + strWhere + " order by ID Desc";
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
               dev.BEDSN = dicBedMapping.ContainsKey(dev.DEVICEIP) ? dicBedMapping[dev.DEVICEIP] : string.Empty;
               mlistDevice.Add(dev);
           }
       }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void btnConfig_Click(object sender, EventArgs e)
       {
           try
           {
               foreach (Control ctl in plContent.Controls)
               {
                   if (ctl is CheckBox)
                   {
                       CheckBox chk = ctl as CheckBox;
                       Device dev = chk.Tag as Device;
                       if (dev != null)
                       {
                           dev.STATUS = chk.Checked ? "1" : "0";
                       }
                       UpdateDeviceStatus(dev);
                   }
               }
               this.DialogResult = System.Windows.Forms.DialogResult.OK;
           }
           catch (Exception ex)
           {
               
           }

       }

       private void UpdateDeviceStatus(Device dev)
       {
           DBReader dr = new DBReader(DBConnect.SYS);
           string strSql = string.Format(@"update Device set STATUS='{0}' where ID={1}", dev.STATUS, dev.ID);
           DBConnect.SYS.ExecSQL(strSql);
       }

        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void btnCancel_Click(object sender, EventArgs e)
       {
           this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
       }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void chkCheck_CheckedChanged(object sender, EventArgs e)
       {
           CheckBox chk = sender as CheckBox;
           if (chk != null)
           {
               ChkChange(chk.Checked);
           }
           
       }

        /// <summary>
        /// 全选或者权否
        /// </summary>
        /// <param name="isChecked">是否选中</param>
       private void ChkChange(bool  isChecked)
       {
           try
           {
               foreach (Control chk in plContent.Controls)
               {
                   if (chk is CheckBox)
                   {
                       (chk as CheckBox).Checked = isChecked;
                   }
               }
           }
           catch (Exception)
           {
               
               
           }
       }
    }
}
