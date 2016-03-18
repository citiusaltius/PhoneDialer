using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using SynapsedServerLibrary.Tables.Defines;
using SynapsedServerLibrary.Utilities;

namespace SynapsedServerLibrary.Tables.Accessors
{
    public class SecurityInformations : TableObject
    {
        new DefinitionsSecurityInformations ThisTableDefinition = new DefinitionsSecurityInformations();

        public Model.SecurityInfo Create(Model.SecurityInfo ObjectIn)
        {
            Debug.WriteLine("===SecurityInformation.Create===");
            // Request new SecurityInformationId
            TableCounters TableCountersClient = new TableCounters();
            int NewSecurityInformationId = TableCountersClient.GetAndIncrement(ThisTableDefinition.TableName);
            ObjectIn.SecurityInformationId = (NewSecurityInformationId);
            return (Model.SecurityInfo)Create(ObjectIn, ThisTableDefinition.TableName);
        }


        public Model.SecurityInfo Get(int Id)
        {
            Debug.WriteLine("===SecurityInformation.GetInformation===");
            Debug.WriteLine("Retrieving for " + Id);
            Model.SecurityInfo retval = new Model.SecurityInfo();
            retval.SecurityInformationId = (Id);
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

        public List<Model.SecurityInfo> FindByInformation(int EntityId, string SecurityInformation)
        {
            QueryRequest req = new QueryRequest();
            req.TableName = ThisTableDefinition.TableName;
            req.IndexName = "EntityId-SecurityInformation-index";
            req.KeyConditionExpression = "EntityId = :v_EntityId and SecurityInformation = :v_SecurityInformation";

            req.ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                { ":v_EntityId", new AttributeValue {N = EntityId.ToString() } },
                { ":v_SecurityInformation", new AttributeValue {S = SecurityInformation } }
            };
            req.ScanIndexForward = true;

            StringBuilder Output = new StringBuilder();

            QueryResponse resp = client.Query(req);
            List<Model.SecurityInfo> SecurityInfos = new List<Model.SecurityInfo>();
            foreach (Dictionary<string, AttributeValue> Item in resp.Items)
            {

                foreach (string Key in Item.Keys)
                {
                    Output.AppendLine("<br/>" + Key + " => " + Item[Key].S);
                }

                Model.SecurityInfo NewSecurityInfo = new Model.SecurityInfo();
                NewSecurityInfo.LoadFromItem(Item);
                SecurityInfos.Add(NewSecurityInfo);
            }
            return SecurityInfos;
        }
    }
}
