using System.Runtime.InteropServices;

namespace ProtocolPack.IntelliVue
{
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ConnectIndication
    {
        public int Nomenclature;
        public ROapdus ROapdus;
        public ROIVapdu ROIVapdu;
        public EventReportArgument EventReportArgument;
        public AttributeList ConnectIndInfo;
    }
    
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EventReportArgument
    {
        public ManagedObjectId managed_object; /* ident. of sender */
        public int event_time; /* event time stamp */
        public short event_type; /* identification of event */
        public short  length; /* size of appended data */
        
    }
    
    
}