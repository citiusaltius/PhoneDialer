using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using SynapsedServerLibrary.Tables.Defines;
using SynapsedServerLibrary.Utilities;

namespace SynapsedServerLibrary.Tables.Accessors
{
    public class CommunityMemberships : TableObject
    {
        new DefinitionsCommunityMemberships ThisTableDefinition = new DefinitionsCommunityMemberships();

        public Model.CommunityMembership Create(Model.CommunityMembership ObjectIn)
        {
            Debug.WriteLine("===CommunityMemberships.Create===");
            // Request new CommunityId
            TableCounters TableCounterClient = new TableCounters();
            int NewCommunityMembershipId = TableCounterClient.GetAndIncrement(ThisTableDefinition.TableName);
            ObjectIn.SetCommunityMembershipId(NewCommunityMembershipId);

            return (Model.CommunityMembership)Create(ObjectIn, ThisTableDefinition.TableName);
        }

        public Model.CommunityMembership Get(int Id)
        {
            Debug.WriteLine("===CommunityMemberships.GetInformation===");
            Debug.WriteLine("Retrieving for " + Id);
            Model.CommunityMembership retval = new Model.CommunityMembership();
            retval.SetCommunityMembershipId(Id);
            GetItemRequest req = new GetItemRequest();
            req.Key.Add(ThisTableDefinition.Index, new AttributeValue() { N = Id.ToString() });
            req.TableName = ThisTableDefinition.TableName;

            GetItemResponse resp;
            try
            {
                resp = client.GetItem(req);
            }
            catch (AmazonDynamoDBException e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                throw e;
            }
            retval.LoadFromItem(resp.Item);
            return retval;
        }
    }
}
