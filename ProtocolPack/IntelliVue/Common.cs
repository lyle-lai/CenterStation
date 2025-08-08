using System;
using System.Net;
using System.Runtime.InteropServices;

namespace ProtocolPack.IntelliVue
{
    
    public static class ShortExtensions
    {
        public static short ToHostOrder(this short source)
        {
            return IPAddress.HostToNetworkOrder(source);
        }
    }

    public static class ByteArrExtensions
    {
        public static ushort ToHostOrderShort(this byte[] source)
        {
            return (ushort)((source[0] <<8)+source[1]);
        }
        
        public static int ToHostOrderInt(this byte[] source)
        {
            return (source[0] <<24) +(source[1] <<16) +(source[2] <<8)+source[3];
        }
        
    }

    // public static class StructExtensions
    // {
    //     public static ValueType ToHostOrder(this ValueType source)
    //     {
    //         var fieldInfos = source.GetType().GetFields();
    //         for (int i = 0; i < fieldInfos.Length; i++)
    //         {
    //             var value = fieldInfos[i].GetValue(source);
    //             var type = fieldInfos[i].FieldType;
    //             if (!type.IsPrimitive && !type.IsEnum && type.IsValueType)
    //             {
    //                 ToHostOrder((ValueType)value );
    //             }
    //
    //             // var isAssignableFrom = type.IsAssignableFrom(typeof(ValueType));
    //             //
    //             // if (isAssignableFrom)
    //             // {
    //             //     ToHostOrder((ValueType)value );
    //             // }
    //             else
    //             {
    //                 if (type.Name .Contains("UInt16") )
    //                 {
    //                     fieldInfos[i].SetValue(source,IPAddress.HostToNetworkOrder(UInt16.Parse(value+"") ));
    //                 }
    //                 else if (type.Name .Contains("Int16"))
    //                 {
    //                     fieldInfos[i].SetValue(source,IPAddress.HostToNetworkOrder(Int16.Parse(value+"")));
    //                 }
    //                 else if (type.Name .Contains("Int32"))
    //                 {
    //                     fieldInfos[i].SetValue(source,IPAddress.HostToNetworkOrder(Int32.Parse(value+"")));
    //                 }
    //             }
    //         }
    //
    //         return source;
    //     }
    // }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ManagedObjectId
    {
        public short m_obj_class;
        public GlbHandle m_obj_inst;
    }

    public struct GlbHandle
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] context_id;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] handle;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AttributeList
    {
        public short count;
        public short  length;

        public AVAType[] value;
    }


    
}