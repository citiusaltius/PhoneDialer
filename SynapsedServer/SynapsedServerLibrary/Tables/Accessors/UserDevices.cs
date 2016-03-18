using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using SynapsedServerLibrary.Tables.Defines;
using SynapsedServerLibrary.Utilities;

namespace SynapsedServerLibrary.Tables.Accessors
{
    public class UserDevices : TableObject
    {
        new DefinitionsUserDevices ThisTableDefinition = new DefinitionsUserDevices();

        public Model.UserDevice Create(Model.UserDevice ObjectIn)
        {
            Debug.WriteLine("===.Create===");
            // Request new SecurityInformationId
            TableCounters TableCountersClient = new TableCounters();
            int NewConversationInformationId = TableCountersClient.GetAndIncrement(ThisTableDefinition.TableName);
            ObjectIn.UserDeviceId = (NewConversationInformationId);
            return (Model.UserDevice)Create(ObjectIn, ThisTableDefinition.TableName);
        }


        public Model.UserDevice Get(int Id)
        {
            Debug.WriteLine("===.Get===");
            Debug.WriteLine("Retrieving for " + Id);
            Model.UserDevice retval = new Model.UserDevice();
            retval.UserDeviceId = (Id);
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

        public List<Model.UserDevice> FindByEntityIdDeviceId(int EntityId, string DeviceId)
        {
            QueryRequest req = new QueryRequest();
            req.TableName = ThisTableDefinition.TableName;
            req.IndexName = "EntityId-DeviceId-index";
            req.KeyConditionExpression = "DeviceId = :v_DeviceId and EntityId = :v_EntityId";
            req.ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                { ":v_EntityId", new AttributeValue {N = EntityId.ToString() } },
                { ":v_DeviceId", new AttributeValue {S = DeviceId } }
            };
            req.ScanIndexForward = true;

            StringBuilder Output = new StringBuilder();

            QueryResponse resp = client.Query(req);
            List<Model.UserDevice> UserDevices = new List<Model.UserDevice>();
            foreach (Dictionary<string, AttributeValue> Item in resp.Items)
            {

                foreach (string Key in Item.Keys)
                {
                    Output.AppendLine("<br/>" + Key + " => " + Item[Key].S);
                }

                Model.UserDevice NewMethod = new Model.UserDevice();
                NewMethod.LoadFromItem(Item);
                UserDevices.Add(NewMethod);
            }
            return UserDevices;
        }

        public List<Model.UserDevice> FindByEntityId(int EntityId, bool IsRecentOnly = false)
        {
            QueryRequest req = new QueryRequest();
            req.TableName = ThisTableDefinition.TableName;
            req.IndexName = "EntityId-DeviceId-index";
            req.KeyConditionExpression = "EntityId = :v_EntityId";


            req.ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                { ":v_EntityId", new AttributeValue {N = EntityId.ToString() } }
            };

            if (IsRecentOnly == true)
            {
                req.FilterExpression = "LastLoginTimestamp > :v_LastLoginTimestamp";
                // add LastLoginTimestamp limiter
                req.ExpressionAttributeValues.Add(":v_LastLoginTimestamp", new AttributeValue { S = SynapsedServerLibrary.Defines.Global.UtcNowDateTimePlusTimeFormatAmz(new System.TimeSpan(0, -55, 0)) });
            }

            req.ScanIndexForward = true;

            StringBuilder Output = new StringBuilder();

            QueryResponse resp = client.Query(req);
            List<Model.UserDevice> UserDevices = new List<Model.UserDevice>();
            foreach (Dictionary<string, AttributeValue> Item in resp.Items)
            {

                foreach (string Key in Item.Keys)
                {
                    Output.AppendLine("<br/>" + Key + " => " + Item[Key].S);
                }

                Model.UserDevice NewMethod = new Model.UserDevice();
                NewMethod.LoadFromItem(Item);
                UserDevices.Add(NewMethod);
            }
            return UserDevices;
        }

    }
}
