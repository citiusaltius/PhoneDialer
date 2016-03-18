using System.Collections.Generic;
using System.Text;
using SynapsedServerLibrary.Tables.Defines;
using System.Reflection;

namespace SynapsedServerLibrary.Tables.Model
{
    public class Entity : DataObject
    {
        private DefinitionsEntities _ThisTableDefinition = new DefinitionsEntities();
        public new DefinitionsEntities ThisTableDefinition
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

        private string _EntityName;
        public string EntityName
        {
            get
            {
                return _EntityName;
            }
            set
            {
                _EntityName = value;
            }
        }

        private string _EntityPrefixName;
        public string EntityPrefixName
        {
            get
            {
                return _EntityPrefixName;
            }
            set
            {
                _EntityPrefixName = value;
            }
        }

        private string _EntityFirstName;
        public string EntityFirstName
        {
            get
            {
                return _EntityFirstName;
            }
            set
            {
                _EntityFirstName = value;
            }
        }

        private string _EntityMiddleName;
        public string EntityMiddleName
        {
            get
            {
                return _EntityMiddleName;
            }
            set
            {
                _EntityMiddleName = value;
            }
        }

        private string _EntityLastName;
        public string EntityLastName
        {
            get
            {
                return _EntityLastName;
            }
            set
            {
                _EntityLastName = value;
            }
        }

        private string _EntitySuffixName;
        public string EntitySuffixName
        {
            get
            {
                return _EntitySuffixName;
            }
            set
            {
                _EntitySuffixName = value;
            }
        }

        private string _EntitySalt;
        public string EntitySalt
        {
            get
            {
                return _EntitySalt;
            }
            set
            {
                _EntitySalt = value;
            }
        }

        private string _EntityCognitoId;
        public string EntityCognitoId
        {
            get
            {
                return _EntityCognitoId;
            }
            set
            {
                _EntityCognitoId = value;
            }
        }


        private DefinitionsEntities.EntityTypes _EntityType;
        public DefinitionsEntities.EntityTypes EntityType
        {
            get
            {
                return _EntityType;
            }
            set
            {
                _EntityType = value;
            }
        }

        public int LastLoginUserDeviceId { get; set; }

        public Entity()
        {
            EntityType = DefinitionsEntities.EntityTypes.Undefined;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("EntityId:\t" + EntityId.ToString());
            sb.AppendLine("EntityName:\t" + EntityName);
            sb.AppendLine("EntityType:\t" + EntityType.ToString());
            sb.AppendLine("DataVersion:\t" + DataVersion);
            return sb.ToString();
        }

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
                    case "ServerDaemonLibrary.Defines.Tables.DefinitionsEntities+EntityTypes":
                        this.EntityType = (DefinitionsEntities.GetEntityTypeFromString(Item[Key].S));
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
