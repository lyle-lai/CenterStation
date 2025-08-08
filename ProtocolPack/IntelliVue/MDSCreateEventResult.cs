using System.Runtime.InteropServices;

namespace ProtocolPack.IntelliVue
{
    public struct MDSCreateEventResult
    {
        public RemoteHeader header;
        public EventReportResultArgument eventReportResult;
    }
    
    
    
    public struct EventReportResultArgument
    {
        public ManagedObject managed_object;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] event_time;//相对时间，相对电源启动时间，单位 1/8ms (125us).
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] event_type;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] length;

        // public void FieldToHostOrder()
        // {
        //     managed_object.m_obj_inst.context_id = managed_object.m_obj_inst.context_id.ToHostOrder();
        //     managed_object.m_obj_inst.handle = managed_object.m_obj_inst.handle.ToHostOrder();
        //     managed_object.m_obj_class = managed_object.m_obj_class.ToHostOrder();
        //     event_type = event_type.ToHostOrder();
        //     length = length.ToHostOrder();
        // }
    }


    public struct ManagedObject
    {
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] m_obj_class;
        public GlbHandle m_obj_inst;
    }

 
}