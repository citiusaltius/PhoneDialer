using System.Collections.Generic;
using System.Text;
using SynapsedServerLibrary.Tables.Defines;
using System.Reflection;

namespace SynapsedServerLibrary.Tables.Model
{
    public class ApplicationMembership : DataObject
    {
        private DefinitionsApplicationMemberships _ThisTableDefinition = new DefinitionsApplicationMemberships();

        public new DefinitionsApplicationMemberships ThisTableDefinition
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

        private int _ApplicationMembershipId;
        public int ApplicationMembershipId
        {
            get
            {
                return _ApplicationMembershipId;
            }
            set
            {
                _ApplicationMembershipId  = value;
            }
        }

        private int _EntityId;
        public int EntityId
        {
            get
            {
                return _EntityId;
            }
            set
            {
                _EntityId = value;
            }
        }


        private int _ApplicationId;
        public int ApplicationId
        {
            get
            {
                return _ApplicationId;
            }
            set
            {
                _ApplicationId = value;
            }
        }


        private DefinitionsApplicationMemberships.ApplicationMembershipTypes _ApplicationMembershipType;
        public DefinitionsApplicationMemberships.ApplicationMembershipTypes ApplicationMembershipType
        {
            get
            {
                return _ApplicationMembershipType;
            }
            set
            {
                _ApplicationMembershipType = value;
            }
        }
        
        public ApplicationMembership()
        {
            ThisTableDefinition = new DefinitionsApplicationMemberships();
            ApplicationMembershipType = DefinitionsApplicationMemberships.ApplicationMembershipTypes.Undefined;

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("===ApplicationMembership===");
            sb.AppendLine("ApplicationMembershipId:\t\t" + ApplicationMembershipId);
            sb.AppendLine("EntityId:\t" + EntityId);
            sb.AppendLine("ApplicationId:\t" + ApplicationId);
            sb.AppendLine("ApplicationMembershipType:\t" + ApplicationMembershipType.ToString());
            return sb.ToString() + base.ToString();
        }

        public string LoadFromItem(Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue> Item)
        {
            StringBuilder Output = new StringBuilder();
            Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue> UnprocessedProperties = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>();
            Output.AppendLine(this.LoadFromItem(Item, out UnprocessedProperties));
            Output.AppendLine("===end base===");
            Dictionary<string, PropertyInfo> PropertyDictionary = this.GetProperties();
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
                    case "ServerDaemonLibrary.Defines.Tables.DefinitionsApplicationMemberships+ApplicationMembershipTypes":
                        
                        this.ApplicationMembershipType = (DefinitionsApplicationMemberships.GetApplicationMembershipTypeFromString(Item[Key].S));
                        Output.AppendLine("Assigned: " + Key + " => " + PropertyDictionary[Key].GetValue(this));
                        break;


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
