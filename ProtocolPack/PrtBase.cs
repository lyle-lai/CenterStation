using System;
using System.Collections.Generic;
using System.Text;
//----------------------------------------------------------------------------------------------
namespace ProtocolPack
{
    //----------------------------------------------------------------------------------------------
    public class PrtBase
    {
        //----------------------------------------------------------------------------------------------
        public static void Initialize()
        {
        }
        //----------------------------------------------------------------------------------------------
        private int ID = -1;
        //床边机状态-已连接
        public delegate void OnConnected();
        public OnConnected onConnected = null;
        //接收到参数数据
        public delegate void OnPara(String FullSN, Int16 Value);
        public OnPara onPara = null;
        //接收到波形数据
        public delegate void OnWave(String FullSN, Byte[] Data);
        public OnWave onWave = null;
        //----------------------------------------------------------------------------------------------
        public PrtBase(int Index)
        {
            ID = Index;

        }
        //----------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------
}
