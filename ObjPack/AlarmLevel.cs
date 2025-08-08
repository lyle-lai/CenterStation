using System;
using System.Collections.Generic;
using System.Text;

namespace ObjPack
{
    public  class AlarmLevel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParaName { get; set; }

        /// <summary>
        /// 告警级别
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Device主键
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceSN { get; set; }
    }
}
