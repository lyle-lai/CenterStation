using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GlobalClass;
using System.Xml;
//-----------------------------------------------------------------------------
namespace Settings
{
    //-----------------------------------------------------------------------------
    public partial class FrmBed : Form
    {
        public string Id = string.Empty;
        public string strPatientName = string.Empty;
        public string strBedSN = string.Empty;
        public string strDevCode = string.Empty;
        private WebTrans.Transfusion trans;
        //-----------------------------------------------------------------------------
        public FrmBed()
        {
            InitializeComponent();
            this.Load += FrmBed_Load;
        }

        private  void FrmBed_Load(object sender, EventArgs e)
        {
            try
            {
                txbID.Text = string.Empty;
                txbIndptNo.Text = string.Empty;
               trans=new WebTrans.Transfusion();
            }
            catch (Exception ex)
            {
                
           
            }
        }
        //-----------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {   
            //病人信息保存
            strBedSN = txbBedNum.Text;
            strPatientName = txbPatient.Text;
            DBConnect.SYS.BeginTransaction();
            DBConnect.SYS.AddPara("BedNum", txbBedNum.Text);
            DBConnect.SYS.AddPara("PatientName", txbPatient.Text);
         //   DBConnect.SYS.ExecSQL("Update BedDevMapping Set BedSN=@BedSN,patientname=@PatientName  Where IP='" + ViewIP + "'");
            DBConnect.SYS.ExecSQL("Update Device Set BEDNUM=@BedNum,patientname=@PatientName  Where ID='" + Id + "'");
            DBConnect.SYS.Commit();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 回车就显示病人信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetpatientInfo(txbID.Text.Trim(),0);
            }
        }

        /// <summary>
        /// 键盘leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbID_Leave(object sender, EventArgs e)
        {
            try
            {
              //  GetpatientInfo();
            }
            catch (Exception ex)
            {
            }
        }

        private void GetpatientInfo(string txbVal,int i)
        {
            if (trans != null && !string.IsNullOrEmpty(txbVal))
            {
                string str = string.Empty;
               string strReturnVal =string.Empty;
                if (i == 0)
                {
                    if (txbVal.Length <= 8) return;
                     str = string.Format(@"<RequestRoot><FunCode>18</FunCode><XmlPara><patientId>{0}</patientId></XmlPara></RequestRoot>", txbVal.Trim());
                }
                else if (i == 1)
                {
                    if (txbVal.Length <= 8) return;
                    str = string.Format(@"<RequestRoot><FunCode>95</FunCode><XmlPara> <inp_no>{0}</inp_no></XmlPara></RequestRoot>", txbVal.Trim());
                }
                if (string.IsNullOrEmpty(str)) return;
                strReturnVal = trans.runfun(str);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strReturnVal);

                XmlNodeList nodes = doc.GetElementsByTagName("ResultCode");
                if (nodes != null && nodes.Count > 0)
                {
                    XmlNode node = nodes[0];
                    if (node.InnerText == "0")//表示返回成功
                    {
                        XmlNodeList items = doc.GetElementsByTagName("Item");
                        foreach (XmlNode n in items)
                        {
                            XmlNodeList list = n.ChildNodes;
                            foreach (XmlNode child in list)
                            {
                                if (child.Name == "bed_label")
                                {
                                    txbBedNum.Text = child.InnerText;
                                }
                                else if (child.Name == "NAME")
                                {
                                    txbPatient.Text = child.InnerText;
                                }
                            }
                        }
                    }
                }
            }

        }


        /// <summary>
        /// 回车开始查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbIndptNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetpatientInfo(txbIndptNo.Text.Trim(),1);
            }
        }
        //-----------------------------------------------------------------------------
    }
    //-----------------------------------------------------------------------------
}
