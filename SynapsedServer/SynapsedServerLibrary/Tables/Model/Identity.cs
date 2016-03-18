using System.Collections.Generic;
using System.Text;
using SynapsedServerLibrary.Tables.Defines;
using System.Reflection;

namespace SynapsedServerLibrary.Tables.Model
{
    public class Identity : DataObject
    {
        private DefinitionsIdentities _ThisTableDefinition = new DefinitionsIdentities();
        [System.Web.Script.Serialization.ScriptIgnore]
        public new DefinitionsIdentities ThisTableDefinition
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

        private int _IdentityId;
        public int IdentityId
        {
            get
            {
                return _IdentityId;
            }
            set
            {
                _IdentityId = value;
            }
        }

        private string _IdentityName;
        public string IdentityName
        {
            get
            {
                return _IdentityName;
            }
            set
            {
                _IdentityName = value;
            }
        }

        private DefinitionsIdentities.IdentityTypes _IdentityType;
        public DefinitionsIdentities.IdentityTypes IdentityType
        {
            get
            {
                return _IdentityType;
            }
            set
            {
                _IdentityType = value;
            }
        }

        private int _CommunityId;
        public int CommunityId
        {
            get
            {
                return _CommunityId;
            }
            set
            {
                _CommunityId = value;
            }
        }

        private bool _IsAssumable;
        public bool IsAssumable
        {
            get
            {
                return _IsAssumable;
            }
            set
            {
                _IsAssumable = value;
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

        public Identity()
        {
            IdentityType = DefinitionsIdentities.IdentityTypes.Undefined;
            IsAssumable = false;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("===Identity===");
            sb.AppendLine("IdentityId:\t" + IdentityId);
            sb.AppendLine("IdentityName:\t" + IdentityName);
            sb.AppendLine("IdentityType:\t" + IdentityType.ToString());
            sb.AppendLine("CommunityId:\t" + CommunityId);
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
                    case "ServerDaemonLibrary.Defines.Tables.DefinitionsIdentities+IdentityTypes":
                        this.IdentityType = (DefinitionsIdentities.GetIdentityTypeFromString(Item[Key].S));
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
