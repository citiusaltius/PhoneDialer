using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using SynapsedServerLibrary.Tables.Defines;
using SynapsedServerLibrary.Utilities;


namespace SynapsedServerLibrary.Tables.Accessors
{
    public class ContactMethods : TableObject
    {
        new DefinitionsContactMethods ThisTableDefinition = new DefinitionsContactMethods();

        public Model.ContactMethod Create(
            Model.ContactMethod ObjectIn
            )
        {

            Debug.WriteLine("===ContactMethods.Create===");
            TableCounters TableCounterClient = new TableCounters();
            //Debug.WriteLine("TableName: '" + ThisTableDefinition.TableName + "'");
            //int NewContactMethodId = TableCounterClient.GetAndIncrement(ThisTableDefinition.TableName);
            //ObjectIn.ContactMethodId = (NewContactMethodId);
            ObjectIn.TimestampExpiration = (SynapsedServerLibrary.Defines.Global.UtcNowDateTimePlusTimeFormatAmz(SynapsedServerLibrary.Defines.Global.ExpirationTimespanDefault));

            return (Model.ContactMethod)base.Create(ObjectIn, ThisTableDefinition.TableName);

        }

        public Model.ContactMethod Get(int ContactMethodId)
        {
            Debug.WriteLine("===ContactMethods.GetInformation===");
            Model.ContactMethod retval = new Model.ContactMethod();
            retval.ContactMethodId = (ContactMethodId);
            GetItemRequest req = new GetItemRequest();
            req.Key.Add(ThisTableDefinition.Index, new AttributeValue() { N = ContactMethodId.ToString() });
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
            // Get Name
            retval.LoadFromItem(resp.Item);
            return retval;
        }


        public List<Model.ContactMethod> FindByInformation(string Information)
        {
            QueryRequest req = new QueryRequest();
            req.TableName = ThisTableDefinition.TableName;
            req.IndexName = "ContactInformation-TimestampExpiration-index";
            req.KeyConditionExpression = "ContactInformation = :v_Information"
                // + " and TimestampExpiration > :v_TimestampToday"
                ;
            req.ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                //{ ":v_TimestampToday", new AttributeValue {S = ServerDaemonLibrary.Defines.Global.UtcNowDateTimeFormatAmz() },
                { ":v_Information", new AttributeValue {S = Information } }
            };
            req.ScanIndexForward = true;

            StringBuilder Output = new StringBuilder();

            QueryResponse resp = client.Query(req);
            List<Model.ContactMethod> ContactMethods = new List<Model.ContactMethod>();
            foreach (Dictionary<string, AttributeValue> Item in resp.Items)
            {

                foreach (string Key in Item.Keys)
                {
                    Output.AppendLine("<br/>" + Key + " => " + Item[Key].S);
                }

                Model.ContactMethod NewMethod = new Model.ContactMethod();
                NewMethod.LoadFromItem(Item);
                ContactMethods.Add(NewMethod);
            }
            return ContactMethods;
        }
    }
}
