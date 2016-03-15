using System.Collections.Generic;
using System.Text;
using SynapsedServerLibrary.Tables.Defines;
using SynapsedServerLibrary.Utilities;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace SynapsedServerLibrary.Tables.Accessors
{
    public abstract class TableObject : Base.BaseObject
    {

        public TableDefinitions ThisTableDefinition;

        public static AmazonDynamoDBClient client = new AmazonDynamoDBClient(SynapsedServerLibrary.Defines.Global.DynamoDbRegion);

        public Model.DataObject Create(Model.DataObject ObjectIn, string TableName)
        {
            Debug.WriteLine("===" + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + "===");
            PutItemRequest req = new PutItemRequest();
            TableCounters TableCounterClient = new TableCounters();
            int NewId = TableCounterClient.GetAndIncrement(TableName);

            // Update Object first
            string TimestampCreation = SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz();
            ObjectIn.TimestampCreation = (TimestampCreation);
            ObjectIn.TimestampModified = (TimestampCreation);
            ObjectIn.DataVersion = SynapsedServerLibrary.Defines.Global.DataVersionCurrent;
            ObjectIn.ModifiedByEntityId = SynapsedServerLibrary.Defines.Global.ServerDaemonEntityId;
            req.TableName = TableName;

            req.Item = ObjectIn.ConvertToItem();
            Debug.WriteLine(ObjectIn.PrintProperties());
            PutItemResponse resp;
            try
            {
                resp = client.PutItem(req);
            }
            catch (AmazonDynamoDBException e)
            {
                Debug.WriteLine("===exception in Create===");
                Debug.WriteLine(ObjectIn.PrintProperties());
                throw e;
            }
            return ObjectIn;
        }

        public void Update(Model.DataObject ObjectIn)
        {
            Debug.WriteLine("===" + System.Reflection.MethodBase.GetCurrentMethod().Name + "===");
            Debug.WriteLine("===" + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + "===");

            //Update item first
            ObjectIn.ModifiedByEntityId = SynapsedServerLibrary.Defines.Global.ServerDaemonEntityId;
            ObjectIn.TimestampModified = SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz();

            Dictionary<string, System.Reflection.PropertyInfo> Properties = ObjectIn.GetProperties();
            dynamic TableDefinition = (Properties["ThisTableDefinition"].GetValue(ObjectIn));

            UpdateItemRequest req = new UpdateItemRequest();
            UpdateItemResponse resp = new UpdateItemResponse();
            Dictionary<string, AttributeValue> Item = ObjectIn.ConvertToItem();

            req.TableName = TableDefinition.TableName;
            req.Key.Add(TableDefinition.Index, Item[TableDefinition.Index]);

            // Create update expression
            StringBuilder UpdateExpressionBuilder = new StringBuilder();
            UpdateExpressionBuilder.Append("SET ");
            foreach (string Key in Item.Keys)
            {
                // skip updating the key value
                if (Key == TableDefinition.Index) { continue; }

                string NewKey = ":new" + Key;
                UpdateExpressionBuilder.Append(" " + Key + " = " + NewKey + ",");
                req.ExpressionAttributeValues.Add(NewKey, Item[Key]);
            }
            // remove trailing comma
            UpdateExpressionBuilder.Remove(UpdateExpressionBuilder.Length - 1, 1);
            Debug.WriteLine(UpdateExpressionBuilder.ToString());

            req.UpdateExpression = UpdateExpressionBuilder.ToString();
            try
            {
                resp = client.UpdateItem(req);
            }
            catch (AmazonDynamoDBException e)
            {
                Debug.WriteLine("===exception in Update===");
                Debug.WriteLine(ObjectIn.PrintProperties());
                throw e;
            }
            Debug.WriteLine("Update status: " + resp.HttpStatusCode);

        }

        public Model.DataObject Get(int Id, out Dictionary<string, AttributeValue> UnprocessedProperties)
        {
            Debug.WriteLine("===" + System.Reflection.MethodBase.GetCurrentMethod().Name + "===");
            Debug.WriteLine("===" + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + "===");
            Debug.WriteLine("Retrieving for " + Id);
            Model.DataObject retval = new Model.DataObject();

            Dictionary<string, System.Reflection.PropertyInfo> Properties = this.GetProperties();
            dynamic TableDefinition = (Properties["ThisTableDefinition"].GetValue(this));

            GetItemRequest req = new GetItemRequest();
            req.Key.Add(TableDefinition.Index, new AttributeValue() { N = Id.ToString() });
            req.TableName = TableDefinition.TableName;

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
            retval.LoadFromItem(resp.Item, out UnprocessedProperties);
            return retval;
        }

        public void Delete(Model.DataObject ObjectIn)
        {
            Debug.WriteLine("===" + System.Reflection.MethodBase.GetCurrentMethod().Name + "===");
            Debug.WriteLine("===" + System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName + "===");

            Dictionary<string, System.Reflection.PropertyInfo> Properties = ObjectIn.GetProperties();
            dynamic TableDefinition = (Properties["ThisTableDefinition"].GetValue(ObjectIn));

            DeleteItemRequest req = new DeleteItemRequest();
            DeleteItemResponse resp;

            Dictionary<string, AttributeValue> Item = ObjectIn.ConvertToItem();
            Debug.WriteLine(TableDefinition.Index);
            Debug.WriteLine(Item[TableDefinition.Index].N);

            req.Key.Add(TableDefinition.Index, Item[TableDefinition.Index]);
            if (ThisTableDefinition.IsHashAndRange == true)
            {
                req.Key.Add(TableDefinition.Range, Item[TableDefinition.Range]);
            }
            req.TableName = TableDefinition.TableName;
            try
            {
                resp = client.DeleteItem(req);
            }
            catch (AmazonDynamoDBException e)
            {
                Debug.WriteLine("===exception in Delete===");
                Debug.WriteLine(ObjectIn.PrintProperties());
                throw e;
            }
            Debug.WriteLine("Delete status: " + resp.HttpStatusCode);

        }


    }
}
