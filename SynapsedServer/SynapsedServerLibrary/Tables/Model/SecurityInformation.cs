using System.Collections.Generic;
using System.Text;
using SynapsedServerLibrary.Tables.Defines;
using System.Reflection;

namespace SynapsedServerLibrary.Tables.Model
{
    public class SecurityInfo : DataObject
    {
        private DefinitionsSecurityInformations _ThisTableDefinition = new DefinitionsSecurityInformations();
        public new DefinitionsSecurityInformations ThisTableDefinition
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

        private int _SecurityInformationId;
        public int SecurityInformationId
        {
            get
            {
                return _SecurityInformationId;
            }
            set
            {
                _SecurityInformationId = value;
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

        private DefinitionsSecurityInformations.SecurityInformationTypes _SecurityInformationType;
        public DefinitionsSecurityInformations.SecurityInformationTypes SecurityInformationType
        {
            get
            {
                return _SecurityInformationType;
            }
            set
            {
                _SecurityInformationType = value;
            }
        }

        private string _SecurityInformation;
        public string SecurityInformation
        {
            get
            {
                return _SecurityInformation;
            }
            set
            {
                _SecurityInformation = value;
            }
        }

        private int _NumberOfUses;
        public int NumberOfUses
        {
            get
            {
                return _NumberOfUses;
            }
            set
            {
                _NumberOfUses = value;
            }
        }


        private int _MaxNumberOfUses;
        public int MaxNumberOfUses
        {
            get
            {
                return _MaxNumberOfUses;
            }
            set
            {
                _MaxNumberOfUses = value;
            }
        }

        private string _TimestampExpiration;
        public string TimestampExpiration
        {
            get
            {
                return _TimestampExpiration;
            }
            set
            {
                _TimestampExpiration = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("===SecurityInformation===");
            sb.AppendLine("SecurityInformationId:\t\t" + SecurityInformationId);
            sb.AppendLine("EntityId:\t\t\t" + EntityId);
            sb.AppendLine("SecurityInformationType:\t" + SecurityInformationType.ToString());
            sb.AppendLine("Information:\t\t\t" + SecurityInformation);
            sb.AppendLine("NumberOfUses:\t\t\t" + NumberOfUses);
            sb.AppendLine("MaxNumberOfUses:\t\t" + MaxNumberOfUses);
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
                    case "ServerDaemonLibrary.Defines.Tables.DefinitionsSecurityInformation+SecurityInformationTypes":
                        this.SecurityInformationType = (DefinitionsSecurityInformations.GetSecurityInformationsTypeFromString(Item[Key].S));
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
