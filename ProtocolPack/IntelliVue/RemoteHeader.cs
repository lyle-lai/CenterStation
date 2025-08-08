
using System.Runtime.InteropServices;

namespace ProtocolPack.IntelliVue
{

    public struct ROapdus
    {
        /* ID for operation */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] ro_type;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        /* bytes to follow */
        public byte[] length;
    }
    
    public struct SPpdu
    {
        //固定0xE100
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] session_id;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] p_context_id;
    }
    
    public struct ROIVapdu
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] invoke_id; /* identifies the transaction */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] command_type; /* identifies type of command */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] length; /* no. of bytes in rest of message */
        
    }
    
    public struct RemoteHeader
    {
        public SPpdu SPpdu;
        public ROapdus ROapdus;
        public ROIVapdu RoiVapdu;
        public void InitFixedValue()
        {
            SPpdu.session_id = new byte[]{0xe1,0x00};//该字段默认值使用小端模式，内存拷贝时会使用小端模式
            SPpdu.p_context_id =new byte[]{0x00,0x02} ;
        }
    }
    
    public struct RemoteHeaderLinked
    {
        public SPpdu SPpdu;
        public ROapdus ROapdus;
        public ROIVapdu RoiVapdu;
        public void InitFixedValue()
        {
            SPpdu.session_id = new byte[]{0xe1,0x00};//该字段默认值使用小端模式，内存拷贝时会使用小端模式
            SPpdu.p_context_id =new byte[]{0x00,0x02} ;
        }
    }

    public struct ROLRSapdu
    {
        public byte state;
        public byte count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] invoke_id; /* identifies the transaction */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] command_type; /* identifies type of command */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] length; /* no. of bytes in rest of message */

    }

}