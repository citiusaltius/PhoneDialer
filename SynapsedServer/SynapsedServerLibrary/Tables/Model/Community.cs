using System.Collections.Generic;
using System.Text;
using SynapsedServerLibrary.Tables.Defines;
using System.Reflection;

namespace SynapsedServerLibrary.Tables.Model
{
    public class Community : DataObject
    {
        private DefinitionsCommunities _ThisTableDefinition = new DefinitionsCommunities();
        public new DefinitionsCommunities ThisTableDefinition
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

        //public void SetCommunityId(int Id)
        //{
        //    this.CommunityId = Id;
        //}

        private string _CommunityName;
        public string CommunityName
        {
            get
            {
                return _CommunityName;
            }
            set
            {
                _CommunityName = value;
            }
        }

        private DefinitionsCommunities.CommunityTypes _CommunityType;
        public DefinitionsCommunities.CommunityTypes CommunityType
        {
            get
            {
                return _CommunityType;
            }
            set
            {
                _CommunityType = value;
            }
        }
        public string AddressStreet { get; private set; }
        public string AddressAdditional { get; private set; }
        public string AddressCity { get; private set; }
        public string AddressStateRegionProvince { get; private set; }
        public string AddressCountry { get; private set; }
        public List<string> AcceptableWebDomains { get; private set; }

        public Community()
        {
            ThisTableDefinition = new DefinitionsCommunities();
            CommunityType = DefinitionsCommunities.CommunityTypes.Undefined;
            AcceptableWebDomains = new List<string>();
            AddressAdditional = "";
            AddressCity = "";
            AddressCountry = "";
            AddressStateRegionProvince = "";
            AddressStreet = "";
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
            sb.AppendLine("===Community===");
            sb.AppendLine("CommunityId:\t\t" + CommunityId);
            sb.AppendLine("CommunityName:\t" + CommunityName);
            sb.AppendLine("CommunityType:\t" + CommunityType.ToString());
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
                    case "ServerDaemonLibrary.Defines.Tables.DefinitionsCommunities+CommunityTypes":
                        this.CommunityType = (DefinitionsCommunities.GetCommunityTypeFromString(Item[Key].S));
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
