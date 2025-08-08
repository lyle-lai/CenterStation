using System.Runtime.InteropServices;

namespace ProtocolPack.IntelliVue
{
    public struct NuObsValueCmp
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] count; 
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
      
        public byte[] length;
    }
}