using System.Collections.Generic;
using System.Text;
using SynapsedServerLibrary.Tables.Defines;
using System.Reflection;

namespace SynapsedServerLibrary.Tables.Model
{
    public class ContactMethod : DataObject
    {
        private DefinitionsContactMethods _ThisTableDefinition = new DefinitionsContactMethods();
        public new DefinitionsContactMethods ThisTableDefinition
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

        private int _ContactMethodId;
        public int ContactMethodId
        {
            get
            {
                return _ContactMethodId;
            }
            set
            {
                _ContactMethodId = value;
            }
        }

        private string _ContactMethodName;
        public string ContactMethodName
        {
            get
            {
                return _ContactMethodName;
            }
            set
            {
                _ContactMethodName = value;
            }
        }

        private DefinitionsContactMethods.ContactMethodTypes _ContactMethodType;
        public DefinitionsContactMethods.ContactMethodTypes ContactMethodType
        {
            get
            {
                return _ContactMethodType;
            }
            set
            {
                _ContactMethodType = value;
            }
        }

        private string _ContactInformation;
        public string ContactInformation
        {
            get
            {
                return _ContactInformation;
            }

            set
            {
                _ContactInformation = value;
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

        private bool _IsValidated;
        public bool IsValidated
        {
            get
            {
                return _IsValidated;
            }
            set
            {
                _IsValidated = value;
            }
        }

        private bool _IsLoginable;
        public bool IsLoginable
        {
            get
            {
                return _IsLoginable;
            }
            set
            {
                _IsLoginable = value;
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

        public ContactMethod()
        {
            ThisTableDefinition = new DefinitionsContactMethods();
            ContactMethodType = DefinitionsContactMethods.ContactMethodTypes.Undefined;
            IsValidated = false;
            IsLoginable = false;
        }




        //public void SetContactMethodType(DefinitionsContactMethods.ContactMethodTypes Type)
        //{
        //    ContactType = Type;
        //}

        //public void SetContactInformation(string Information)
        //{
        //    ContactInformation = Information;
        //}

        //public void SetIdentityId(int Id)
        //{
        //    IdentityId = Id;
        //}

        //public void SetIsValidated(bool value)
        //{
        //    IsValidated = value;
        //}

        //public void SetIsLoginable(bool value)
        //{
        //    IsLoginable = value;
        //}

        //public void SetTimestampExpiration(string Timestamp)
        //{
        //    TimestampExpiration = Timestamp;
        //}

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.AppendLine("ContactMethodId:\t" + ContactMethodId);
            Output.AppendLine("ContactMethodName:\t" + ContactMethodName);
            Output.AppendLine("ContactMethodName:\t" + ContactMethodName);
            return base.ToString();
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
                    case "ServerDaemonLibrary.Defines.Tables.DefinitionsContactMethods+ContactMethodTypes":
                        this.ContactMethodType = (DefinitionsContactMethods.GetContactMethodTypeFromString(Item[Key].S));
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
