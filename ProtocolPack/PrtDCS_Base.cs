using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using GlobalClass;
//----------------------------------------------------------------------------------------------
//DCS Protocol
//----------------------------------------------------------------------------------------------
namespace ProtocolPack
{
    //----------------------------------------------------------------------------------------------
    public class PrtDCS_Base
    {
        public const int SVR_PORT = 9006;
        public const int HEAD_LEN = 4;
        //protected static int CLN_PORT = 9005;
        //----------------------------------------------------------------------------------------------
        protected static UInt16 CheckSum(Byte[] Buf, int sPos, int Len)
        {   //2.3.4 包头校验和
            //指从包长度到包类型ID 共四个字节的校验和，校验和的计算方法为这4 个字节的16 位累加和取反，
            //计算示例程序如下：
            uint checksum = 0;
            for (int i = 0; i < Len; i++)
                checksum += Buf[sPos + i];
            checksum = (checksum >> 16) + (checksum & 0xffff);
            checksum += (checksum >> 16);
            checksum = ~checksum & 0xffff;
            return (UInt16)checksum;
        }
        //----------------------------------------------------------------------------------------------
        protected static void PushToBuffer(Byte[] Val, Byte[] Buf, int Pos)
        {
            for (int i = 0; i < Val.Length; i++)
                Buf[Pos + i] = Val[i];
        }
        //----------------------------------------------------------------------------------------------
        /*public static void Send(UdpClient Udp, String IP, int Port, Int32 ID, Int16[] ValueList)
        {
            Byte[] Buf = GetSendBuf(ID, ValueList);
            Udp.Send(Buf, Buf.Length, IP, Port);
        }*/
        //----------------------------------------------------------------------------------------------
        public PrtDCS_Base()
        {
        }
        //----------------------------------------------------------------------------------------------
        public static Byte[] GetSendBuf(Int32 ID, Int16[] ValueList)
        {
            int Len = HEAD_LEN + 4 + 2 * (ValueList == null ? 0 : ValueList.Length);
            //
            Byte[] SendBuf = new Byte[Len];
            //初始
            short Pos = 4;
            //对象ID
            PushToBuffer(BitConverter.GetBytes(ID), SendBuf, Pos);
            Pos += 4;
            //对象值
            if (ValueList != null)
            {
                for (int i = 0; i < ValueList.Length; i++)
                {
                    PushToBuffer(BitConverter.GetBytes(ValueList[i]), SendBuf, Pos);
                    Pos += 2;
                }
            }
            //填入长度
            PushToBuffer(BitConverter.GetBytes(Len), SendBuf, 0);
            //校验和
            UInt16 ckSum = CheckSum(SendBuf, 4, Len - 4);
            PushToBuffer(BitConverter.GetBytes(ckSum), SendBuf, 2);
            //
            return SendBuf;
            //Udp.Send(SendBuf, Len, IP, Port);
        }
        //----------------------------------------------------------------------------------------------
        private int UnPack(Byte[] Buf,  int Len)
        {
            int Pos = 0;
            //长度检查
            Int16 PLen = (Int16)(BitConverter.ToInt16(Buf, Pos));  //获取包长度
            if (Len != PLen)
            {
                return 0;   //长度不正确，抛弃
            }
            Pos += 2;
            //包头校验
            UInt16 ckSum = BitConverter.ToUInt16(Buf, Pos);        //校验和
            if (CheckSum(Buf, 4, Len - 4) != ckSum)
            {
                Debug.WriteLine("UnPack 包头校验和不正确");
                return 1;   //校验和不正确，跳到下一字节检查
            }
            Pos += 2;
            //获取数据包内容
            UInt32 ID = BitConverter.ToUInt32(Buf, Pos );        //校验和
            Pos += 4;

            int Count = (Len - Pos) / 2;
            Int16[] ValueList = new Int16[Count];
            for (int i = 0; i < Count; i++)
            {
                ValueList[i] = BitConverter.ToInt16(Buf, Pos);        //校验和
                Pos += 2;
            }
            Receive(ID, ValueList);
            //
            return PLen;    //解包完成，跳到下一包头
        }
        //----------------------------------------------------------------------------------------------
        protected void Receive(UInt32 ID, Int16[] ValueList)
        {
            ;
        }
        //----------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------
    
}