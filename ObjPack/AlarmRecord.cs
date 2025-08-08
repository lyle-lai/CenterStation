using System;
using System.Collections.Generic;
using System.Text;

namespace ObjPack
{
    public class AlarmRecord
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// DeviceID
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceSN { get; set; }

        /// <summary>
        /// 告警类型（超上限告警或超下限告警）
        /// </summary>
        public string AlarmType { get; set; }

        /// <summary>
        /// 床号
        /// </summary>
        public string BedNum { get; set; }


        /// <summary>
        /// 病人姓名
        /// </summary>
        public string PatientName { get; set; }

        /// <summary>
        /// 告警级别
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 告警值
        /// </summary>
        public double Val { get; set; }

        /// <summary>
        /// 参数名
        /// </summary>
        public string ParaName { get; set; }

        /// <summary>
        /// 高值
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// 低值
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// 告警时间
        /// </summary>
        public DateTime AlarmTime { get; set; }
    }
}
