using System.Runtime.InteropServices;

namespace ProtocolPack.IntelliVue
{
    public struct NuObsValue
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] physio_id; 
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
      
        public byte[] state;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
     
        public byte[] unit_code;
        
        
        public sbyte exponent;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] mantissa;

        public int GeteMantissaValue()
        {
            return (mantissa[0] <<16) +(mantissa[1] <<8)+mantissa[2];
        }
    }
}