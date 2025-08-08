using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using GlobalClass;
using System.Diagnostics;

namespace ProtocolPack
{
    /// <summary>
    /// 迈瑞iPM监护仪的ZMW数据解码助手
    /// </summary>
    /// <summary>
    /// WAVE ID枚举
    /// </summary>
    public enum WAVE_ID
    {
        WAVE_UNKNOWN = 0,
        WAVE_ECG_CH1 = 1101,
        WAVE_ECG_CH2,
        WAVE_ECG_CH3,
        WAVE_ECG_CH4,
        WAVE_ECG_CH5,
        WAVE_ECG_CH6,
        WAVE_ECG_CH7,
        WAVE_ECG_CH8,
        WAVE_ECG_CH9,
        WAVE_ECG_CH10,
        WAVE_ECG_CH11,
        WAVE_ECG_CH12,
        WAVE_ECG_I,
        WAVE_ECG_II,
        WAVE_ECG_III,
        WAVE_ECG_AVR,
        WAVE_ECG_AVL,
        WAVE_ECG_AVF,
        WAVE_ECG_V1,
        WAVE_ECG_V2,
        WAVE_ECG_V3,
        WAVE_ECG_V4,
        WAVE_ECG_V5,
        WAVE_ECG_V6,

        WAVE_RESP = 1151,
        WAVE_PLETH,
        IBP_ART = 1171
    };

    public enum DATA_TYPE
    {
        DATA_TYPE_UNKNOWN = 0,
        DATA_TYPE_BYTE,
        DATA_TYPE_INT8,
        DATA_TYPE_BOOL,
        DATA_TYPE_UINT16,
        DATA_TYPE_INT16,
        DATA_TYPE_UINT32,
        DATA_TYPE_INT32,
        DATA_TYPE_FLOAT,
        DATA_TYPE_DOUBLE
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class STU_WAVE_PKG_HEADER
    {
        public short CheckSum;
        public ushort WaveChannelID;
        public byte DataSize;
        public ushort Sample;
        public byte Reserve;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class DECODED_WAVE_DATA
    {
        public WAVE_ID ChannelID;
        public ushort SampleRate;
        public DATA_TYPE DataType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public float[] DataBuffer;
    }

    public static class MR_ZMW
    {
        /// <summary>
        /// 解析波形数据
        /// </summary>
        /// <param name="InBuffer"></param>
        /// <returns></returns>
        public static DECODED_WAVE_DATA Parse(byte[] InBuffer)
        {
            // 检查消息格式，如果不是ZMW｜，则不是要解析的ZMW消息。
            string flag = System.Text.Encoding.ASCII.GetString(InBuffer.Take(4).ToArray());
            if (!flag.ToUpper().Equals("ZMW|")) return null;

            STU_WAVE_PKG_HEADER _WAVE_PKG_HEADER = new STU_WAVE_PKG_HEADER();
            DECODED_WAVE_DATA _WAVE_DATA = new DECODED_WAVE_DATA();

            byte[] DecodedData = DecodeWaveData(InBuffer.Skip(4).ToArray());
            if (null == DecodedData) return null;

            ushort iChecksum = CalcCheckSum(DecodedData);
            if (0 != iChecksum) return null; // 数据包校验失败了。

            // 将字节数组转为消息头结构
            object boxedHeader = (object)_WAVE_PKG_HEADER;
            int sizeOfHeader = Marshal.SizeOf(boxedHeader);
            StructConver.ByteArrayToStructure(DecodedData.Take(sizeOfHeader).ToArray(), ref boxedHeader, 0);
            _WAVE_PKG_HEADER = (STU_WAVE_PKG_HEADER)boxedHeader;
            _WAVE_PKG_HEADER.Sample = (ushort)IPAddress.NetworkToHostOrder((short)_WAVE_PKG_HEADER.Sample);
            _WAVE_PKG_HEADER.WaveChannelID = (ushort)IPAddress.NetworkToHostOrder((short)_WAVE_PKG_HEADER.WaveChannelID);

            // 将余下的字节数组转为波形数据
            _WAVE_DATA.SampleRate = _WAVE_PKG_HEADER.Sample;
            _WAVE_DATA.ChannelID = (WAVE_ID)Enum.ToObject(typeof(WAVE_ID), _WAVE_PKG_HEADER.WaveChannelID);
            //Debug.WriteLine("编码前字节数组：");
            //foreach (var item in InBuffer)
            //{
            //    Debug.Write(item + ",");
            //}
            //Debug.WriteLine("-------");
            //Debug.WriteLine("编码后字节数组：");
            //foreach (var item1 in DecodedData)
            //{
            //    Debug.Write(item1 + ",");
            //}
            //Debug.WriteLine("-------");
            switch (_WAVE_PKG_HEADER.DataSize)
            {
                case 1:
                    {
                        _WAVE_DATA.DataType = DATA_TYPE.DATA_TYPE_BYTE;
                        var data = DecodedData.Skip(sizeOfHeader).ToArray();
                        _WAVE_DATA.DataBuffer = new float[data.Length];
                        for (int i = 0; i < data.Length; i++)
                        {
                            _WAVE_DATA.DataBuffer[i] = data[i];
                        }

                        break;
                    }
                case 2:
                    {
                        _WAVE_DATA.DataType = DATA_TYPE.DATA_TYPE_INT16;
                        byte[] temp = DecodedData.Skip(sizeOfHeader).ToArray();
                        _WAVE_DATA.DataBuffer = new float[_WAVE_PKG_HEADER.Sample];
                        for (int i = 0; i < _WAVE_PKG_HEADER.Sample; i ++)
                        {
                            //    ushort iWaveData = BitConverter.ToUInt16(temp, i);
                            //    iWaveData = (ushort)IPAddress.NetworkToHostOrder((short)iWaveData);
                            //    _WAVE_DATA.DataBuffer[i + 1] = (byte)(iWaveData >> 8);
                            //    _WAVE_DATA.DataBuffer[i] = (byte)(iWaveData & 0xFF);
                            short s = 0;   //一个16位整形变量，初值为 0000 0000 0000 0000
                            s = (short)(s ^ temp[2*i]);  //将temp[2*i]赋给s的低8位
                            s = (short)(s << 8);  //s的低8位移动到高8位
                            s = (short)(s ^ temp[2*i + 1]); //把temp[2*i+1]赋给s的低8位
                            _WAVE_DATA.DataBuffer[i] = s;
                        }
                        break;
                    }
                default:
                    {
                        // 不支持其他数据格式
                        return null;
                    }
            }
            //Debug.WriteLine("编码后字节数组转换为对象：");
            //Debug.WriteLine($"_WAVE_DATA.ChannelID：{_WAVE_DATA.ChannelID}");
            //Debug.WriteLine($"_WAVE_DATA.DataType：{_WAVE_DATA.DataType}");
            //Debug.WriteLine($"_WAVE_DATA.SampleRate：{_WAVE_DATA.SampleRate}");
            //Debug.WriteLine($"_WAVE_DATA.DataBuffer：{_WAVE_DATA.DataBuffer.Length}");
            foreach (var item2 in _WAVE_DATA.DataBuffer)
            {
                Debug.Write(item2 + ",");
            }
            return _WAVE_DATA;
        }

        /// <summary>
        /// 解码波形数据
        /// 
        /// Each Data Block consists of 8 bytes (Byte1 to Byte8), each of which then consists 8 bits (Bit 
        /// [0] to Bit[7]). The most significant bit is 1 (Bit[7] = 1). Byte8 is used to save the value of
        /// Bit[7] of the former 7 bytes(Byte1 to Byte7) before they are encoded.
        /// </summary>
        /// <param name="InBuffer"></param>
        /// <returns></returns>
        private static byte[] DecodeWaveData(byte[] InBuffer)
        {
            int iBlockCount;
            int iInBufferSize = InBuffer.Length;

            // 根据迈瑞协议，波形数据区域的长度只能是8的整数倍
            if ((0 != (iInBufferSize % 8)) || 0 == iInBufferSize)
            {
                return null;
            }

            iBlockCount = iInBufferSize / 8;
            // 输出的字节数组空间约等于数据块的7倍
            byte[] OutBuffer = new byte[iBlockCount * 7];

            int iOffset = 0;
            for (int i = 0; i < iBlockCount; i++)
            {
                // 逐个数据块解析
                for (int ii = 0; ii < 7; ii++)
                {
                    byte b = InBuffer[iOffset + ii];
                    // 检查字节的最高位，最高位始终为1，如果不是，则协议数据有问题。
                    if (0 == (b & 0x80))
                    {
                        return null;
                    }
                    byte temp = InBuffer[iOffset + 7];
                    temp = (byte)(temp << (7 - ii));
                    temp |= 0x7F;
                    InBuffer[iOffset + ii] &= temp;
                }
                Array.Copy(InBuffer, iOffset, OutBuffer, i * 7, 7);
                iOffset += 8;
            }
            return OutBuffer;
        }

        /// <summary>
        /// 计算校验位
        /// </summary>
        /// <param name="InBuffer"></param>
        /// <returns></returns>
        private static ushort CalcCheckSum(byte[] InBuffer)
        {
            byte[] bCheckSum = new byte[2];
            uint iCksum = 0;
            for (int i = 0; i < InBuffer.Length; i += 2)
            {
                ushort temp = BitConverter.ToUInt16(InBuffer, i);
                temp = (ushort)IPAddress.NetworkToHostOrder((short)temp);
                iCksum += temp;
            }
            if (0 != InBuffer.Length % 2)
            {
                iCksum += InBuffer[InBuffer.Length - 1];
            }

            iCksum = (iCksum >> 16) + (iCksum & 0xFFFF);
            iCksum += (iCksum >> 16);

            bCheckSum[0] = (byte)(~iCksum / 256);
            bCheckSum[1] = (byte)(~iCksum % 256);
            //unsafe
            //{
            //    int iPacketLen = InBuffer.Length;
            //    short iTemp;
            //    uint iCksum = 0;
            //    fixed (byte* pData = InBuffer)
            //    {
            //        short* pData1 = (short*)pData;

            //        while (1 < iPacketLen)
            //        {
            //            iTemp = *pData1;
            //            pData1++;

            //            iTemp = IPAddress.NetworkToHostOrder(iTemp);
            //            iCksum += (uint)iTemp;
            //            iPacketLen -= sizeof(short);
            //        }

            //        if (0 < iPacketLen)
            //        {
            //            iCksum += *(byte*)pData1;
            //        }

            //        iCksum = (iCksum >> 16) + (iCksum & 0xFFFF);
            //        iCksum += (iCksum >> 16);

            //        bCheckSum[0] = (byte)(~iCksum / 256);
            //        bCheckSum[1] = (byte)(~iCksum % 256);
            //    }
            //}
            return BitConverter.ToUInt16(bCheckSum, 0);
        }

        #region 一个测试函数，用来测试和校验功能是否正确
        public static void TestCalCheckSum()
        {
            unsafe
            {
                byte[] ucData = new byte[12] { 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                byte[] ucCheckSum = new byte[2] { 0, 0 };

                ushort sum = CalcCheckSum(ucData);
                byte[] temp = BitConverter.GetBytes(sum);

                ucData[0] = temp[0];
                ucData[1] = temp[1];

                ushort res = CalcCheckSum(ucData);
                byte[] temp2 = BitConverter.GetBytes(res);
            }
        }
        #endregion
    }
}
