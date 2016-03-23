using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using SynapsedServerLibrary.Tables.Defines;
using SynapsedServerLibrary.Utilities;

namespace SynapsedServerLibrary.Tables.Accessors
{
    public class Identities : TableObject
    {
        new DefinitionsIdentities ThisTableDefinition = new DefinitionsIdentities();

        public Model.Identity Create(Model.Identity ObjectIn)
        {
            Debug.WriteLine("===Identities.CreateCommunity===");
            // Request new IdentityId
            TableCounters TableCountersClient = new TableCounters();
            //int NewIdentityId = TableCountersClient.GetAndIncrement(ThisTableDefinition.TableName);
            //ObjectIn.IdentityId = (NewIdentityId);
            return (Model.Identity)Create(ObjectIn, ThisTableDefinition.TableName);
        }

        public Model.Identity Get(int IdentityId)
        {
            Debug.WriteLine("===Identities.GetInformation===");
            Model.Identity retval = new Model.Identity();
            retval.IdentityId = (IdentityId);
            GetItemRequest req = new GetItemRequest();
            req.Key.Add(ThisTableDefinition.Index, new AttributeValue() { N = IdentityId.ToString() });
            req.TableName = ThisTableDefinition.TableName;

            GetItemResponse resp;
            try
            {
                resp = client.GetItem(req);
            }
            catch (AmazonDynamoDBException e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }

            retval.LoadFromItem(resp.Item);
            return retval;
        }

        public List<Model.Identity> FindByEntityId(int EntityId)
        {

            QueryRequest req = new QueryRequest();
            req.TableName = ThisTableDefinition.TableName;
            req.IndexName = "EntityId-IdentityId-index";
            req.KeyConditionExpression = "EntityId = :v_EntityId"
                // + " and TimestampExpiration > :v_TimestampToday"
                ;
            req.ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                //{ ":v_TimestampToday", new AttributeValue {S = ServerDaemonLibrary.Defines.Global.UtcNowDateTimeFormatAmz() },
                { ":v_EntityId", new AttributeValue {N = EntityId.ToString() } }
            };
            req.ScanIndexForward = true;

            StringBuilder Output = new StringBuilder();

            QueryResponse resp = client.Query(req);
            List<Model.Identity> Identities = new List<Model.Identity>();
            foreach (Dictionary<string, AttributeValue> Item in resp.Items)
            {

                Model.Identity FoundIdentity = new Model.Identity();
                FoundIdentity.LoadFromItem(Item);
                Identities.Add(FoundIdentity);
            }
            return Identities;
        }

    }
}
