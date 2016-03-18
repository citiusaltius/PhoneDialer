using System.Collections.Generic;
using System.Text;
using SynapsedServerLibrary.Tables.Defines;
using System.Reflection;

namespace SynapsedServerLibrary.Tables.Model
{
    public class UserDevice : DataObject
    {
        private DefinitionsUserDevices _ThisTableDefinition = new DefinitionsUserDevices();
        public new DefinitionsUserDevices ThisTableDefinition
        {
            get
            {
                return _ThisTableDefinition;
            }
            set
            {
                _ThisTableDefinition = value;
            }
        }

        public int UserDeviceId { get; set; }
        public int EntityId { get; set; }
        public string DeviceId { get; set; }

        public string DeviceQueueName { get; set; }
        public string LastLoginTimestamp { get; set; }

        public string LoadFromItem(Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue> Item)
        {
            StringBuilder Output = new StringBuilder();
            Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue> UnprocessedProperties = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>();

            Output.AppendLine(this.LoadFromItem(Item, out UnprocessedProperties));
            Output.AppendLine("===end base===");
            Dictionary<string, System.Reflection.PropertyInfo> PropertyDictionary = this.GetProperties();
            Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue> StillUnprocessedProperties = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>();
            foreach (string Key in UnprocessedProperties.Keys)
            {
                if (PropertyDictionary.ContainsKey(Key) == false)
                {
                    StillUnprocessedProperties.Add(Key, Item[Key]);

                    Output.AppendLine("Unable to process " + Key + " of unrecognized type");
                    continue;
                }
                switch (PropertyDictionary[Key].PropertyType.ToString())
                {
                    //case "":
                    // break;

                    // If all else fails, return in UnprocessedProperties
                    default:
                        StillUnprocessedProperties.Add(Key, Item[Key]);
                        Output.AppendLine("Unable to inherently process " + Key + " of type " + PropertyDictionary[Key].PropertyType.ToString());
                        break;
                }
            }
            return Output.ToString();
        }
    }
}
