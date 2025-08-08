using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace ProtocolPack
{
    /// <summary>
    /// TCP接收数据状态对象
    /// </summary>
    public class StateObject
    {
        public PrtServerBase Worker = null;
        public const int BufferSize = 2 * 1024;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }
}
