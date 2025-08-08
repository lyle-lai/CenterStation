using System;
using System.Collections.Generic;
using System.Text;

namespace ProtocolPack
{
    public class MR_Packager
    {
        private static byte CMD_VERSION = 0x01;   // 版本
        private static byte CMD_WORD_GET = 0x01;   // 命令字类型（获取数据请求包）
        private static byte CMD_WORD_SET = 0x03;   // 命令字类型（设置数据请求包）


        /// <summary>
        /// 1.2.1 - 请求床边机支持的所有参数的实时传输数据开关
        /// </summary>
        /// <returns></returns>
        public static byte[] GetRealTimeParaTransState()
        {
            // 1.2 传输实时参数数据开关（命令字ID＝203），包含字段ID：570，571
            // （1）请求实时传输数据开关的参数个数为0时，表示请求床边机支持的所有参数的实时传输数据开关（只需要设置字段ID为570的字段）
            // （2）请求实时传输数据开关的参数个数不为0时，表示请求实时传输数据开关的参数实际个数（除了字段ID为570的字段，还需要设置字段ID为571的字段）
            // 本段代码只设置了570的字段，即请求实时传输数据开关的参数个数为0的情况

            // -------------------- 生成数据包的包体 --------------------
            Int16 cmdId = 203;         // 命令字ID
            byte[] body = new byte[8]; // 包体

            // -------------------- 字段1：字段ID为570 --------------------
            // 2个字节（字段ID）
            Int16 fieldId = 570;       // 字段ID为570	
            GlobalClass.BigEndian.SetInt(ref body, 0, fieldId);

            // 1个字节（数据类型）
            body[2] = 1;  // Byte对应1

            // 4个字节（值长度）
            GlobalClass.BigEndian.SetInt(ref body, 3, (Int32)1);  // Byte对应的值长度为1

            // n个字节（值）
            //body[7] = 0;  // 字段值为0时，表示请求床边机支持的所有参数的实时传输数据开关

            // -------------------- 生成完整的数据包 --------------------
            return GetFullPackage(CMD_VERSION, CMD_WORD_GET, cmdId, body);
        }


        // ------------------- 生成完整的数据包 -------------------
        private static byte[] GetFullPackage(byte cmdVer, byte cmdWord, Int16 cmdId, byte[] packageBody)
        {
            Int32 packLen = 24 + packageBody.Length;   // 4.包总长度：最小包头长度24字节 + 包体长度
            byte[] package = new byte[packLen];

            // ---------------------- 包头 ----------------------
            // 1.版本（1字节）：低4位为协议版本号，高4位为协议修订号
            package[0] = (byte)cmdVer;

            // 2.命令字类型（1字节）
            package[1] = (byte)cmdWord;

            // 3.命令字ID（2字节）
            GlobalClass.BigEndian.SetInt(ref package, 2, cmdId);

            // 4.包总长度（4字节）
            GlobalClass.BigEndian.SetInt(ref package, 4, packLen);

            // 5.包类型ID（4字节）：本版本暂不使用
            //GlobalClass.BigEndian.SetInt(ref packageHead, 8, (Int32)0);

            // 6.关联包类型ID（4字节）：本版本暂不使用
            //GlobalClass.BigEndian.SetInt(ref packageHead, 12, (Int32)0);

            // 7.校验和（2字节）
            //GlobalClass.BigEndian.SetInt(ref packageHead, 16, (Int16)0);

            // 8.标志位（2字节）：本版本暂不使用
            //GlobalClass.BigEndian.SetInt(ref packageHead, 18, (Int16)0);

            // 9.应答码（1字节）：如果是请求包或者是通知包，此字段无意义。
            //packageHead[20] = (byte)0;

            // 10.保留字段（1字节）：本版本暂不使用
            //packageHead[21] = (byte)0;

            // 11.Option项长度（2字节）
            //GlobalClass.BigEndian.SetInt(ref packageHead, 22, (Int16)0);

            // 12.Option项（非必需项）：本版本暂不使用

            // ---------------------- 包体 ----------------------
            Buffer.BlockCopy(packageBody, 0, package, 24, packageBody.Length);

            // ---------------------- 修改校验和 ----------------------
            Int16 checksum = Checksum(package);
            GlobalClass.BigEndian.SetInt(ref package, 16, checksum);
            //Int16 checksum2 = Checksum(package);

            return package;
        }
        // ------------------- 生成校验码
        public static Int16 Checksum(byte[] data)
        {
            UInt32 checksum = 0;      // 校验和初始值为0
            int packLen = data.Length;
            int pos = 0;

            while (pos < packLen - 1)
            {
                // 从pos开始取data相邻两个字节，将其转换成无符号16位整数，再将其转换成无符号32位整数累加到checksum中
                //Int32 tmpVal = GlobalClass.BigEndian.GetInt32(data, pos);
                // 【！！】应该用UInt16
                UInt16 tmpVal = GlobalClass.BigEndian.GetUInt16(data, pos);
                checksum += (UInt32)tmpVal; // Convert.ToUInt32(BitConverter.ToUInt16(data, pos));
                pos += 2;
            }

            // 如果为奇数，将最后一个字节扩展到4字节，再累加到checksum中
            if (pos == packLen - 1)
            {
                checksum += (UInt32)data[pos];
            }

            // 因为最后要返回16位字节码，所以将checksum高16位字节码加到低16位字节码中
            checksum = (checksum >> 16) + (checksum & 0xffff);

            // 将上一步溢出到高16位字节码加入到低16位字节码中
            checksum += (checksum >> 16);

            // 最后将字节码和取反返回
            return (Int16)~checksum;
        }



        public static byte[] SetPatientInfo()
        {
            // 1.5接收病人（命令字ID＝104）
            // 此指令用于床边机接收病人或者中央站控制接收病人时通知对方进行同步接收

            // -------------------- 生成数据包的包体 --------------------
            int len = 39;
            Int16 cmdId = 104;             // 命令字ID
            byte[] body = new byte[len];   // 包体
            int pos = 0;

            // -------------------- 字段1：字段ID为385(病人GUID,byte[],32 -- 2+1+4+32=39) --------------------
            //{
            //    // 2个字节（字段ID）
            //    Int16 fieldId = 385;       // 字段ID为385	
            //    GlobalClass.BigEndian.SetInt(ref body, pos, fieldId);
            //    pos += 2;

            //    // 1个字节（数据类型）
            //    body[pos] = 1;  // byte[]对应1
            //    pos++;

            //    // 4个字节（值长度）
            //    GlobalClass.BigEndian.SetInt(ref body, pos, (Int32)32);  // 
            //    pos += 4;

            //    // n个字节（值）
            //    string guid = System.Guid.NewGuid().ToString("N");
            //    foreach (char c in guid)
            //    {
            //        body[pos++] = System.Convert.ToByte("0" + c, 16);
            //    }
            //}

            // -------------------- 字段2：字段ID为386(病人的姓（First Name）,Char[],32 -- 2+1+4+32=39) --------------------
            {
                // 2个字节（字段ID）
                Int16 fieldId = 386;       // 字段ID为386	
                GlobalClass.BigEndian.SetInt(ref body, pos, fieldId);
                pos += 2;

                // 1个字节（数据类型）
                body[pos] = 2;  // Char[]对应2
                pos++;

                // 4个字节（值长度）
                GlobalClass.BigEndian.SetInt(ref body, pos, (Int32)32);  // 
                pos += 4;

                // n个字节（值）
                string sVal = "Jack Jones";
                foreach (char c in sVal)
                {
                    body[pos++] = (byte)c;
                }
            }
            /*
            // -------------------- 字段3：字段ID为388(病历号,Char[],32 -- 2+1+4+32=39) --------------------
            {
                // 2个字节（字段ID）
                Int16 fieldId = 388;       // 字段ID为388	
                GlobalClass.BigEndian.SetInt(ref body, pos, fieldId);
                pos += 2;

                // 1个字节（数据类型）
                body[pos] = 2;  // Char[]对应2
                pos++;

                // 4个字节（值长度）
                GlobalClass.BigEndian.SetInt(ref body, pos, (Int32)32);  // 
                pos += 4;

                // n个字节（值）
                string sVal = "ABCDEFEDCBA";
                foreach (char c in sVal)
                {
                    body[pos++] = (byte)c;
                }
            }*/

            // -------------------- 生成完整的数据包 --------------------
            //return GetFullPackage(CMD_VERSION, CMD_WORD_GET, cmdId, body);
            return GetFullPackage(CMD_VERSION, CMD_WORD_SET, cmdId, body);
        }
		
        public static byte[] GetPatientInfo()
        {
            Int16 cmdId = 7;             // 命令字ID

            return GetFullPackage(CMD_VERSION, CMD_WORD_GET, cmdId, new byte[0]);
        }

        public static byte[] SetNULL()
        {
            return GetFullPackage(CMD_VERSION, CMD_WORD_SET, 0, new byte[0]);
        }

        // 1.7 NIBP测量命令（命令字ID＝502）
        public static byte[] SetMeasureNIBPCmd()
        {
            // 1.7 NIBP测量命令（命令字ID＝502），包含字段ID：1115

            // -------------------- 生成数据包的包体 --------------------
            Int16 cmdId = 502;         // 命令字ID
            byte[] body = new byte[8]; // 包体

            // -------------------- 字段1：字段ID为1115(NIBP测量命令,Byte,1 -- 2+1+4+1=8) --------------------
            // 2个字节（字段ID）
            Int16 fieldId = 1115;       // 字段ID为1115	
            GlobalClass.BigEndian.SetInt(ref body, 0, fieldId);

            // 1个字节（数据类型）
            body[2] = 0;  // Byte对应1

            // 4个字节（值长度）
            GlobalClass.BigEndian.SetInt(ref body, 3, (Int32)1);  // Byte对应的值长度为1

            // n个字节（值）
            body[7] = 1;  // 开始充气=7, 开始测量=1

            // -------------------- 生成完整的数据包 --------------------
            return GetFullPackage(CMD_VERSION, CMD_WORD_SET, cmdId, body);
        }
    }
}
