using System;
using System.Collections.Generic;
using System.Text;

namespace ObjPack
{
    public class Device
    {
        public Device()
        {
         }

        public int ID { get; set; }
        public string DEVICEID { get; set; }
        public string BEDSN { get; set; }
        public string DEVICETYPE { get; set; }
        public string BARCODE { get; set; }
        public string DEVICEIP { get; set; }
        public string INHOSNUM { get; set; }
        public string ARCHIVESID { get; set; }
        public string BEDNUM { get; set; }
        public string PATIENTNAME { get; set; }

        private string mStatus=string.Empty;
        public string STATUS 
        {
            get{return mStatus;}
            set{mStatus=value;}
        }

        public string STATUSNAME
        {
            get 
            {
                return STATUS == "1" ? "启动" : "禁止";
            }
        }
        public int NUMSORT { get; set; }
    }
}
