using System.Collections.Generic;
using System.Text;
using SynapsedServerLibrary.Tables.Defines;
using System.Reflection;

namespace SynapsedServerLibrary.Tables.Model
{
    public class Application : DataObject
    {
        private DefinitionsApplications _ThisTableDefinition = new DefinitionsApplications();

        public new DefinitionsApplications ThisTableDefinition
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

        private string _ApplicationName;
        public string ApplicationName
        {
            get
            {
                return _ApplicationName;
            }
            set
            {
                _ApplicationName = value;
            }
        }

        private DefinitionsApplications.ApplicationTypes _ApplicationType;
        public DefinitionsApplications.ApplicationTypes ApplicationType
        {
            get
            {
                return _ApplicationType;
            }
            set
            {
                _ApplicationType = value;
            }
        }
        public string   ApplicationContactName { get; private set; }
        public int      ApplicationContactEntityId { get; private set; }
     

        public Application()
        {
            ThisTableDefinition = new DefinitionsApplications();
            ApplicationType = DefinitionsApplications.ApplicationTypes.Undefined;
            ApplicationContactName = "";
            ApplicationContactEntityId = -1;
        }


        //public void SetCommunityName(string Name)
        //{
        //    this.CommunityName = Name;
        //}

        //public void SetCommunityType(DefinitionsCommunities.CommunityTypes TypeIn)
        //{
        //    this.CommunityType = TypeIn;
        //}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("===Application===");
            sb.AppendLine("ApplicationId:\t\t" + ApplicationId);
            sb.AppendLine("ApplicationName:\t" + ApplicationName);
            sb.AppendLine("ApplicationType:\t" + ApplicationType.ToString());
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
                    case "ServerDaemonLibrary.Defines.Tables.DefinitionsApplications+ApplicationTypes":
                        this.ApplicationType = (DefinitionsApplications.GetApplicationTypeFromString(Item[Key].S));
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
