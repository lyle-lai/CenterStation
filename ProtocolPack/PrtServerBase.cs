using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
//using ViewPack;
using System.Threading;
using GlobalClass;
//----------------------------------------------------------------------------------------------
//DCS Protocol
//----------------------------------------------------------------------------------------------
namespace ProtocolPack
{
    //----------------------------------------------------------------------------------------------
    public class PrtServerBase
    {
        //----------------------------------------------------------------------------------------------
        public static List<PrtServerBase> ALL = new List<PrtServerBase>();
        //----------------------------------------------------------------------------------------------
        //private int ViewID = -1;
        //private ViewMonitor VM = null;
        public enum ConnectState { OffLine, Connect, GetData };
        public ConnectState State = ConnectState.OffLine;
        //private DateTime LastRecTime = DateTime.MinValue;
        public String RIP;
        public String IP4;
        public virtual Socket TCP { get; protected set; }
        //事件定义
        public delegate void DisConnectedCallback(Socket source);
        public DisConnectedCallback OnDisConnected = null;
        public delegate void OnConnected();
        public OnConnected onConnected = null;
        public delegate void OnGetParaGroup(String SN, short[] Values);
        public OnGetParaGroup onGetParaGroup = null;
        public delegate void OnGetWaveGroup(String SN, float[] Data, int sPos, int Len);
        public OnGetWaveGroup onGetWaveGroup = null;
        // 关闭客户端
        public virtual void Close()
        {
            TCP.Shutdown(SocketShutdown.Both);
            TCP.Close();
            TCP = null;
            //TCP.Dispose();
        }

        public DateTime LastDataTicks { get; set; }
        //----------------------------------------------------------------------------------------------
        public PrtServerBase(int Index)
        {
            ALL.Add(this);
            //ViewID = Index;
            //if (ViewID < 0 || ViewID >= ID_LMT) return;
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------
}