using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using SynapsedServerLibrary.Tables.Defines;
using SynapsedServerLibrary.Utilities;

namespace SynapsedServerLibrary.Tables.Accessors
{
    public class Entities : TableObject
    {
        new DefinitionsEntities ThisTableDefinition = new DefinitionsEntities();

        /// <summary>
        /// Creates an Entity
        /// </summary>
        /// <param name="EntityName"></param>
        /// <param name="EntityType"></param>
        /// <returns></returns>
        public Model.Entity Create(
            Model.Entity ObjectIn
            )
        {
            Debug.WriteLine("===Entities.CreateEntity===");
            PutItemRequest req = new PutItemRequest();
            TableCounters TableCounterClient = new TableCounters();
            int NewEntityId = TableCounterClient.GetAndIncrement(ThisTableDefinition.TableName);
            ObjectIn.EntityId = (NewEntityId);
            return (Model.Entity)base.Create(ObjectIn, ThisTableDefinition.TableName);
        }

        public Model.Entity Get(int EntityId)
        {
            Debug.WriteLine("===Entities.GetInformation===");
            Model.Entity retval = new Model.Entity();
            retval.EntityId = (EntityId);
            GetItemRequest req = new GetItemRequest();
            req.Key.Add(ThisTableDefinition.Index, new AttributeValue() { N = EntityId.ToString() });
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


        public List<Model.Entity> FindByCognitoId(string CognitoId)
        {

            QueryRequest req = new QueryRequest();
            req.TableName = ThisTableDefinition.TableName;
            req.IndexName = "EntityCognitoId-index";
            req.KeyConditionExpression = "EntityCognitoId = :v_CognitoId"
                // + " and TimestampExpiration > :v_TimestampToday"
                ;
            req.ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                //{ ":v_TimestampToday", new AttributeValue {S = ServerDaemonLibrary.Defines.Global.UtcNowDateTimeFormatAmz() },
                { ":v_CognitoId", new AttributeValue {S = CognitoId } }
            };
            req.ScanIndexForward = true;

            StringBuilder Output = new StringBuilder();

            QueryResponse resp = client.Query(req);
            List<Model.Entity> Entities = new List<Model.Entity>();
            foreach (Dictionary<string, AttributeValue> Item in resp.Items)
            {
                foreach (string Key in Item.Keys)
                {
                    Output.AppendLine("<br/>" + Key + " => " + Item[Key].S);
                }

                Model.Entity FoundEntity = new Model.Entity();
                FoundEntity.LoadFromItem(Item);
                Entities.Add(FoundEntity);
            }
            return Entities;
        }

    }
}
