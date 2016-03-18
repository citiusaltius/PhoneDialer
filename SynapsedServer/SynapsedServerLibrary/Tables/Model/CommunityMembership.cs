using System.Collections.Generic;
using System.Text;
using SynapsedServerLibrary.Tables.Defines;
using System.Reflection;

namespace SynapsedServerLibrary.Tables.Model
{
    public class CommunityMembership : DataObject
    {
        private DefinitionsCommunityMemberships _ThisTableDefinition = new DefinitionsCommunityMemberships();
        public new DefinitionsCommunityMemberships ThisTableDefinition
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

        public int CommunityMembershipId { get; private set; }
        public int CommunityId { get; private set; }
        public int IdentityId { get; private set; }

        public void SetCommunityMembershipId(int Id)
        {
            this.CommunityMembershipId = Id;
        }

        public void SetCommunityId(int Id)
        {
            CommunityId = Id;
        }

        public void SetIdentityId(int Id)
        {
            IdentityId = Id;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("===Community===");
            sb.AppendLine("CommunityId:\t" + CommunityId);
            sb.AppendLine("IdentityId:\t" + IdentityId);
            sb.AppendLine("CommunityMembershipId:\t" + CommunityMembershipId.ToString());
            return sb.ToString() + base.ToString();
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
