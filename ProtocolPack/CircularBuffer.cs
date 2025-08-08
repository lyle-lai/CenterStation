using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolPack
{
    internal class CircularBuffer
    {
        /// <summary>
        /// 存放内存的数组
        /// </summary>
        public byte[] Buffer { get; set; }
        /// <summary>
        /// 写入数据的大小
        /// </summary>
        public int DataCount { get; set; }
        /// <summary>
        /// 数据起始索引
        /// </summary>
        public int DataStart { get; set; }
        /// <summary>
        /// 数据结束索引
        /// </summary>
        public int DataEnd { get; set; }

        public CircularBuffer(int size)
        {
            DataCount = 0;
            DataStart = 0;
            DataEnd = 0;
            Buffer = new byte[size];
        }

        public byte this[int index]
        {
            get
            {
                if (index >= DataCount) throw new Exception("环形缓冲区异常，索引溢出");
                if (DataStart + index < Buffer.Length)
                {
                    return Buffer[DataStart + index];
                }
                else
                {
                    return Buffer[(DataStart + index) - Buffer.Length];
                }
            }
        }

        /// <summary>
        /// 获得当前写入的字节数
        /// </summary>
        /// <returns></returns>
        public int GetDataCount()
        {
            return DataCount;
        }

        /// <summary>
        /// 获得剩余的字节数
        /// </summary>
        /// <returns></returns>
        public int GetReserveCount()
        {
            return Buffer.Length - DataCount;
        }

        /// <summary>
        /// 清空缓冲区
        /// </summary>
        public void Clear()
        {
            DataCount = 0;
        }

        /// <summary>
        /// 清空指定大小的数据
        /// </summary>
        /// <param name="count"></param>
        public void Clear(int count)
        {
            if (count == 0) return;

            if (count >= DataCount)
            {
                DataCount = 0;
                DataStart = 0;
                DataEnd = 0;
            }
            else
            {
                if (DataStart + count > Buffer.Length)
                {
                    DataStart = (DataStart + count) - Buffer.Length;
                }
                else
                {
                    DataStart += count;
                }
                DataCount -= count;
            }
        }

        /// <summary>
        /// 指定位置写入缓存区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void WriteBuffer(byte[] buffer, int offset, int count)
        {
            int reserveCount = Buffer.Length - DataCount;
            if (reserveCount >= count)  // 可用空间够用
            {
                if (DataEnd + count < Buffer.Length) // 数据没有到结尾
                {
                    Array.Copy(buffer, offset, Buffer, DataEnd, count);
                    DataEnd += count;
                    DataCount += count;
                }
                else
                {
                    // 数据结束索引超出结尾，循环到开始的位置
                    System.Diagnostics.Debug.WriteLine("缓存重新开始...");
                    // 超出索引长度
                    int overflowIndexLength = (DataEnd + count) - Buffer.Length;
                    // 填充在末尾的数据长度
                    int endPushIndexLength = count - overflowIndexLength;
                    Array.Copy(buffer, offset, Buffer, DataEnd, endPushIndexLength);
                    DataEnd = 0;
                    offset += endPushIndexLength;
                    DataCount += endPushIndexLength;
                    if (overflowIndexLength != 0)
                    {
                        Array.Copy(buffer, offset, Buffer, DataEnd, overflowIndexLength);
                    }

                    // 结束索引
                    DataEnd += overflowIndexLength;
                    // 缓存大小
                    DataCount += overflowIndexLength;
                }
            }
            else
            {
                // 缓存溢出，不处理
            }
        }

        public int ReadBuffer(byte[] target, int offset, int count)
        {
            if (count > DataCount) count = DataCount;
            int tempDataStart = DataStart;
            if (DataStart + count < Buffer.Length)
            {
                Array.Copy(Buffer, DataStart, target, offset, count);
            }
            else
            {
                int overflowIndexLength = (DataStart + count) - Buffer.Length;
                int endPushIndexLength = count - overflowIndexLength;
                Array.Copy(Buffer, DataStart, target, offset, endPushIndexLength);

                offset += endPushIndexLength;
                if (overflowIndexLength != 0)
                {
                    Array.Copy(Buffer, 0, target, offset, overflowIndexLength);
                }
            }
            return count;
        }

        public void WriteBuffer(byte[] buffer)
        {
            WriteBuffer(buffer, 0, buffer.Length);
        }
    }
}
