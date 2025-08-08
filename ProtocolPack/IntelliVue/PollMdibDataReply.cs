using System.Net;
using System.Runtime.InteropServices;

namespace ProtocolPack.IntelliVue
{
    /**
     * 结构如下
     * {
            "PollMdibDataReply":{
                "poll_number":"",
                "rel_time_stamp":"",
                "abs_time_stamp":"",
                "polled_attr_grp":"",
                "count":"SingleContextPoll个数",
                "length":"后续报文长度",
                "SingleContextPoll":[
                    {
                        "context_id":"",
                        "count":"ObservationPoll个数",
                        "length":"ObservationPoll包长度",
                        "ObservationPoll":[
                            {
                                "obj_handle":"",
                                "AttributeList":{
                                    "count":"AVAType数量",
                                    "length":"AVAType包长度",
                                    "AVAType":[
                                        {
                                            "attribute_id":"参数id",
                                            "length":"value长度",
                                            "value":"参数值"
                                        }
                                    ]
                                }
                            }
                        ]
                    }
                ]
            }
        }
     */
    
    /**
     * 长度24字节
     */
    public struct PollMdibDataReply
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] poll_number;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] sequence_no;
        
        // public uint rel_time_stamp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] rel_time_stamp;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] abs_time_stamp;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] polled_obj_type;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] polled_attr_grp;
        
        // SingleContextPoll数量
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] count;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        // 后续所有报文长度
        public byte[] length;
        
    }

    public struct SingleContextPoll
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] context_id; 
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        // ObservationPoll数量
        public byte[] count;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        // 后续Observation Poll 报文长度
        public byte[] length;
        
    }


    public struct ObservationPoll
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] obj_handle; 
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        // AVAType数量
        public byte[] count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        // 后续AVAType 报文长度
        public byte[] length;
        
    }
    
    public struct AVAType
    {
        // 参数标识
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] attribute_id;
        // 值长度
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] length;
        
    }
}