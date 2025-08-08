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
    public partial class FrmAlarmPara : Form
    {
        private List<AlarmPara> mAlarmPara;
        private int mDeviceID = 0;
        private string mDeviceSN = string.Empty;
        private bool isHasVal = false;
        private Dictionary<string, AlarmPara> dicAlarmPara = new Dictionary<string, AlarmPara>(); 
        public FrmAlarmPara(List<AlarmPara> para,int strDeviceID=0,string strDeviceSN="")
        {
            InitializeComponent();
            this.mAlarmPara = para;
            this.mDeviceID = strDeviceID;
            this.mDeviceSN = strDeviceSN;
            this.Load += FrmAlarmPara_Load;
            if (para != null && para.Count >= 0 && strDeviceID!=0)
            {
                isHasVal =para[0].DeviceID==0?false:true;
            }
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void FrmAlarmPara_Load(object sender, EventArgs e)
        {
            try
            {
                #region  动态加载控件

                DBReader dr = new DBReader(DBConnect.SYS);
                dr.Select("Select * From ParaItem Where ViewID=-1 and mid is not null Order By ID");
                int i = 0;

                List<string> listLevel = new List<string>();
                listLevel.Add("高");
                listLevel.Add("中");
                listLevel.Add("低");

               
                while (dr.Read())
                {
                    string strName = dr.GetStr("SN");
                    Button btn = new Button();
                    btn.Size = new System.Drawing.Size(60,25);
                    btn.Top = i * 30 + 50;
                    btn.Left =50;
                    btn.Text = strName;
                    btn.Name = strName;
                    btn.Tag = strName;
                    btn.Show();

                    ComboBox cmbAlarm = new ComboBox();
                    cmbAlarm.Size = new System.Drawing.Size(60, 25);
                    cmbAlarm.Top = i * 30 + 50;
                    cmbAlarm.Left = 150;
                    cmbAlarm.DataSource = new List<string>() {"开","关"};
                    cmbAlarm.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbAlarm.Name = strName + "_isEnabled";
                    cmbAlarm.Show();

                    TextBox txbHigh = new TextBox();
                    txbHigh.Size = new System.Drawing.Size(80, 25);
                    txbHigh.Top = i * 30 + 50;
                    txbHigh.Left = 250;
                    txbHigh.Name = strName + "_High";
                    txbHigh.Show();


                    TextBox txbLow= new TextBox();
                    txbLow.Size = new System.Drawing.Size(80, 25);
                    txbLow.Top = i * 30 + 50;
                    txbLow.Left = 350;
                    txbLow.Name = strName + "_Low";
                    txbLow.Show();

                    ComboBox cmbRecord = new ComboBox();
                    cmbRecord.Size = new System.Drawing.Size(60, 25);
                    cmbRecord.Top = i * 30 + 50;
                    cmbRecord.Left = 450;
                    cmbRecord.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbRecord.Name = strName + "_isRecord";
                    cmbRecord.DataSource = new List<string>() { "开", "关" };
                    cmbRecord.Show();

                    ComboBox cmbLevel = new ComboBox();
                    cmbLevel.Size = new System.Drawing.Size(60, 25);
                    cmbLevel.Top = i * 30 + 50;
                    cmbLevel.Left = 550;
                    cmbLevel.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbLevel.DataSource = new List<string>() { "高", "中","低"};
                    cmbLevel.Name = strName + "_Level";
                    cmbLevel.Show();

                    this.Controls.Add(btn);
                    this.Controls.Add(cmbAlarm);
                    this.Controls.Add(txbHigh);
                    this.Controls.Add(txbLow);
                    this.Controls.Add(cmbRecord);
                    this.Controls.Add(cmbLevel);
                    i++;
                }
                #endregion

                #region  加载告警参数

                if (mAlarmPara != null)
                {
                    LoadPara(mAlarmPara);
                }
                else
                {
                    BtnDefault_Click(null,null);
                }

                #endregion

            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="list"></param>
        private void LoadPara(List<AlarmPara> list)
        {
            foreach (AlarmPara para in list)
            {
                if (!dicAlarmPara.ContainsKey(para.ParaName))
                {
                    dicAlarmPara.Add(para.ParaName, para);
                }
                foreach (Control ctl in this.Controls)
                {
                    if (ctl.Name.Contains(para.ParaName))
                    {
                        if (ctl.Name.Contains("_isEnabled"))
                        {
                            ComboBox cmb = ctl as ComboBox;
                            cmb.SelectedIndex = para.isEnabled;
                        }
                        else if (ctl.Name.Contains("_High"))
                        {
                            TextBox txb = ctl as TextBox;
                            txb.Text = para.High.ToString();
                        }
                        else if (ctl.Name.Contains("_Low"))
                        {
                            TextBox txb = ctl as TextBox;
                            txb.Text = para.Low.ToString();
                        }
                        else if (ctl.Name.Contains("_isRecord"))
                        {
                            ComboBox cmb = ctl as ComboBox;
                            cmb.SelectedIndex = para.isRecord;
                        }
                        else if (ctl.Name.Contains("_Level"))
                        {
                            ComboBox cmb = ctl as ComboBox;
                            cmb.SelectedIndex = para.Level;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 恢复默认值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDefault_Click(object sender, EventArgs e)
        {
            try
            {
                DBReader dr = new DBReader(DBConnect.SYS);
                dr.Select("select * from AlarmPara where deviceID=0");
                List<AlarmPara> list = new List<AlarmPara>(); 
                while (dr.Read())
                {
                    AlarmPara para = new AlarmPara();
                    para.DeviceID = dr.GetInt("DeviceID");
                    para.DeviceSN = dr.GetStr("DeviceSN");
                    para.High = dr.GetF("High",0);
                    para.Low = dr.GetF("Low",0);
                    para.ID = dr.GetInt("ID");
                    para.isEnabled = dr.GetInt("isEnabled");
                    para.isRecord = dr.GetInt("isRecord");
                    para.Level = dr.GetInt("Level");
                    para.ParaName = dr.GetStr("ParaName");
                    list.Add(para);
                }
                if (list.Count > 0)
                {
                    LoadPara(list);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfig_Click(object sender, EventArgs e)
        {
            try
            {
                
                foreach (Control ctl in this.Controls)
                { 
  
                   if (ctl is TextBox)
                   {
                       TextBox txb = ctl as TextBox;
                       double numSort = 0;
                       if (string.IsNullOrEmpty(txb.Text) || !double.TryParse(txb.Text, out numSort))
                       {
                           MessageBox.Show("文本框不能为空且为数字！");
                           return;
                       }
                       string[] strName = txb.Name.Split('_');
                       if (strName.Length > 1)
                       {
                           if (!dicAlarmPara.ContainsKey(strName[0]))
                           {
                               AlarmPara para = new AlarmPara();
                               para.DeviceID = mDeviceID;
                               para.DeviceSN = mDeviceSN;
                               dicAlarmPara.Add(strName[0], new AlarmPara());
                           }

                           if (strName[1] == "High")
                           {
                               dicAlarmPara[strName[0]].High = double.Parse(txb.Text);
                           }
                           else if (strName[1] == "Low")
                           {
                               dicAlarmPara[strName[0]].Low = double.Parse(txb.Text);
                           }
                       }
                   }
                   else if (ctl is ComboBox)
                   {
                       ComboBox cmb = ctl as ComboBox;
                       string[] strName = cmb.Name.Split('_');
                       if (strName.Length > 1)
                       {
                           if (!dicAlarmPara.ContainsKey(strName[0]))
                           {
                               dicAlarmPara.Add(strName[0], new AlarmPara());
                           }

                           if (strName[1] == "isEnabled")
                           {
                               dicAlarmPara[strName[0]].isEnabled =cmb.SelectedIndex;
                           }
                           else if (strName[1] == "isRecord")
                           {
                               dicAlarmPara[strName[0]].isRecord = cmb.SelectedIndex;
                           }
                           else if (strName[1] == "Level")
                           {
                               dicAlarmPara[strName[0]].Level = cmb.SelectedIndex;
                           }
                       }
                   }
                   else if (ctl is Button&&ctl.Tag!=null)
                   {
                       Button btn=ctl as Button;
                       if (!dicAlarmPara.ContainsKey(btn.Name))
                       {
                           dicAlarmPara.Add(btn.Name, new AlarmPara());
                       }
                       dicAlarmPara[btn.Name].ParaName = btn.Name;
                       dicAlarmPara[btn.Name].DeviceID = mDeviceID;
                       dicAlarmPara[btn.Name].DeviceSN = mDeviceSN;
                   }

                }
                Save();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
            catch (Exception)
            {
                
             
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            try
            {
                foreach (AlarmPara para in dicAlarmPara.Values)
                {
                    if (para.ID <= 0)
                    {
                        AddAlarmPara(para);
                    }
                    else
                    {
                        if (mDeviceID!=0)
                        {
                            if (isHasVal)
                            {
                                UpdateAlarmPara(para);
                            }
                            else
                            {
                                AddAlarmPara(para);
                            }
                        }
                        else
                        {
                            UpdateAlarmPara(para);
                        }

                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        /// <summary>
        /// 增加告警参数
        /// </summary>
        private void AddAlarmPara(AlarmPara para)
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            string strSql = string.Format(@"insert into AlarmPara(ParaName,isEnabled,High,Low,isRecord,DeviceID,DeviceSN,Level)
                                                                  values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                              para.ParaName, para.isEnabled, para.High, para.Low, para.isRecord, para.DeviceID, para.DeviceSN, para.Level);
            DBConnect.SYS.ExecSQL(strSql);
        }

        /// <summary>
        /// 更新告警参数
        /// </summary>
        private void UpdateAlarmPara(AlarmPara para)
        {
            DBReader dr = new DBReader(DBConnect.SYS);
            string strSql = string.Format(@"update AlarmPara set ParaName='{0}',isEnabled='{1}',High='{2}',Low={3},isRecord='{4}',
                              DeviceID='{5}',DeviceSN='{6}',Level='{7}'where ID='{8}'",
                   para.ParaName, para.isEnabled, para.High, para.Low, para.isRecord, para.DeviceID, para.DeviceSN, para.Level, para.ID);
            DBConnect.SYS.ExecSQL(strSql);
        }

    }
}
