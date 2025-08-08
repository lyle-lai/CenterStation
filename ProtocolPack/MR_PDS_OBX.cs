using GlobalClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProtocolPack
{
    public class MR_PDS_OBX
    {
        public string ValueType { get; set; }

        public string ObservationIdentifier { get; set; }

        public string ObservationSubID { get; set; }

        public string ObservationResults { get; set; }

        public string Units { get; set; }

        public string ReferenceRange { get; set; }

        public string ObservationResultsStatus { get; set; }

        public string UserDefinedAccessChecks { get; set; }

        public string DateTimeOfObservation { get; set; }

        public static MR_PDS_OBX Parse(byte[] buffer)
        {
            string message = System.Text.Encoding.ASCII.GetString(buffer);
            return MR_PDS_OBX.Parse(message);
        }

        public static MR_PDS_OBX Parse(string message)
        {
            string[] fields = message.Trim().Split('|');
            if (!"OBX".Equals(fields[0].ToUpper())) return null;
            MR_PDS_OBX obx = new MR_PDS_OBX();
            try
            {
                obx.ValueType = fields[2];
                obx.ObservationIdentifier = fields[3];
                obx.ObservationSubID = fields[4];
                obx.ObservationResults = fields[5];
                obx.Units = fields[6];
                obx.ReferenceRange = fields[7];
                obx.ObservationResultsStatus = fields[11];
                //obx.UserDefinedAccessChecks = fields[13];
                //obx.DateTimeOfObservation = fields[14];
            }
            catch(Exception ex)
            {
                Log.d(ex.Message);
            }
            return obx;
        }
    }
}
