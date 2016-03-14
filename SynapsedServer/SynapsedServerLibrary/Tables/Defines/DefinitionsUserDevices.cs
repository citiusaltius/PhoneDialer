using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.Tables.Defines
{
    public class DefinitionsUserDevices : TableDefinitions
    {
        public new string   TableName = "UserDevices";
        public new string   Index = "UserDeviceId";
        public const string FieldNameEntityId = "EntityId";
        public const string FieldNameDeviceType = "DeviceType";
        public const string FieldNameDeviceId = "DeviceId";
        public const string FieldNameDeviceQueue = "DeviceQueue";

        public enum DeviceTypes : int
        {
            Undefined,
            DesktopClient,
            WebClientHtml5,
            MobileAndroid,
            MobileIos,
            MobileWindows
        };

    }
}
