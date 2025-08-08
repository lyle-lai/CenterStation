using System;
using System.Collections.Generic;
using System.Text;

namespace ObjPack
{
    public class AlarmPara
    {
        public AlarmPara()
        {
    
         }
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        public string ParaName { get; set; }

        /// <summary>
        /// 是否开关
        /// </summary>
        public int isEnabled { get; set; }


        /// <summary>
        /// 最大值
        /// </summary>
        public double High { get; set; }

       
        /// <summary>
        /// 最小值
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// 是否记录
        /// </summary>
        public int isRecord { get; set; }

       /// <summary>
       /// device主键
       /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceSN { get; set; }

        /// <summary>
        /// 告警级别
        /// </summary>
        public int Level { get; set; }
    }
}
