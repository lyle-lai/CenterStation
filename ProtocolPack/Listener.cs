using System;
//using System.Linq;
using System.Collections.Generic;
using System.Text;
//----------------------------------------------------------------------------------------------
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using GlobalClass;
//----------------------------------------------------------------------------------------------
namespace ProtocolPack
{
    //----------------------------------------------------------------------------------------------
    public class Listener
    {
        //private const int CNT_LMT = 256;
        //----------------------------------------------------------------------------------------------
        private Thread Thr = null;
        private TcpListener TcpLsn = null;
        //public TPeer[] Items = new TPeer[CNT_LMT];//TCP连接数组（数量受限）
        //--------------------------------------
        //事件定义-接收数据，检查合法性
        //public delegate Boolean EVENT_RECEIVED(TPeer Peer);
        public TPeer.EVENT_RECEIVED OnReceived = null;
        //--------------------------------------
        //事件定义-连接
        public delegate void EVENT_CONNECTED(Socket socket);
        public EVENT_CONNECTED OnConnected = null;
        //----------------------------------------------------------------------------------------------
        public Listener(int Port)//构造函数
        {
            TcpLsn = new TcpListener(Port); //在端口新建一个TcpListener对象
            TcpLsn.Start();

            Thr = new Thread(Listen);   //新建一个用于监听的线程
            Thr.Start();                //打开新线程
        }
        //----------------------------------------------------------------------------------------------
        public void Stop()
        {/*
            for (int i = 0; i < CNT_LMT; i++)
            {
                if (Items[i] == null) continue;
                Items[i].Free();
            }*/

            TcpLsn.Stop();
            Thr.Abort();//终止线程
        }
        //----------------------------------------------------------------------------------------------
        private void Listen()
        {
            try
            {
                while (true)//开始监听
                {//循环等待请求
                    if (!TcpLsn.Pending())
                    {//等待连接请求
                        Thread.Sleep(100);
                        continue;
                    }
                    //有连接请求时的处理：将连接分配给空项
                    //Log.d("***** Connect *******");
                    OnConnected(TcpLsn.AcceptSocket());
                    /*for (int i = 0; i < CNT_LMT; i++)
                    {
                        if (Items[i] != null) continue;
                        Debug.WriteLine("AcceptSocket : " + i.ToString());
                        //
                        Items[i] = new TPeer(TcpLsn.AcceptSocket());
                        Items[i].OnReceived = OnReceived;
                        Items[i].OnDisconnect = OnDisconnect;
                        break;
                    }*/
                }
            }
            catch (System.Security.SecurityException)
            {
                Debug.WriteLine("firewall says no no to application - application cries..");
            }
            catch (Exception ex)
            {
                Log.d(ex.Message);
            }
        }
        //----------------------------------------------------------------------------------------------
        /*private void OnDisconnect(TPeer Peer)
        {
            for (int i = 0; i < CNT_LMT; i++)
            {
                if (Items[i] != Peer) continue;
                Items[i].Free();
                Items[i] = null;//回收连接资源，以便下次重复使用。
                Debug.WriteLine("Socket " + i.ToString() + " DisConnect, Free it.");
                break;
            }
        }*/
        //----------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------
}
