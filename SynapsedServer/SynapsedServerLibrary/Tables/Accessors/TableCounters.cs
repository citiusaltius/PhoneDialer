using System;
using Amazon.DynamoDBv2.Model;
using SynapsedServerLibrary.Utilities;

namespace SynapsedServerLibrary.Tables.Accessors
{
    public class TableCounters : TableObject
    {


        /// <summary>
        /// Finds the current counter value for a table named TableName
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns>current counter value</returns>
        public int Get(string TableName)
        {
            //Debug.WriteLine("===ServerDaemonLibrary.TableCounters.Get===");
            GetItemRequest getItemReq = new GetItemRequest();
            getItemReq.TableName = Defines.DefinitionsTableCounter.TableName; // "TableCounters";
            getItemReq.Key.Add(Defines.DefinitionsTableCounter.Index, new AttributeValue() { S = TableName });
            getItemReq.ConsistentRead = true;
            GetItemResponse resp;
            try
            {
                resp = client.GetItem(getItemReq);
            }
            catch (Amazon.DynamoDBv2.AmazonDynamoDBException e)
            {
                Debug.WriteLine("Error in getting counter for '" + TableName + "'");
                throw e;
            }
            // if the table is found
            if (resp.IsItemSet == true)
            {
                int CounterValue = Int32.Parse(resp.Item[Defines.DefinitionsTableCounter.Value].N);

                if (TableName.Equals(resp.Item[Defines.DefinitionsTableCounter.Index].S) == false)
                {
                    throw new Utilities.Exceptions.ServerException("Tables do not match");
                }

                return CounterValue;
            }
            // if the table is not found
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// Safely increments a counter for a specified TableName
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ExpectedValue"></param>
        /// <returns>success or failure of update</returns>
        public bool Increment(string TableName, int ExpectedValue)
        {
            //Debug.WriteLine("===ServerDaemonLibrary.TableCounters.Increment===");
            UpdateItemRequest req = new UpdateItemRequest();
            req.TableName = Defines.DefinitionsTableCounter.TableName;
            req.Key.Add("TableName", new AttributeValue() { S = TableName });
            req.UpdateExpression = "set " + Defines.DefinitionsTableCounter.Value + " = " + Defines.DefinitionsTableCounter.Value + " + :incrementBy";
            req.ReturnValues = Amazon.DynamoDBv2.ReturnValue.ALL_NEW;
            req.ExpressionAttributeValues.Add(":incrementBy", new AttributeValue() { N = "1" });
            req.ExpressionAttributeValues.Add(":expectedCounterValue", new AttributeValue() { N = ExpectedValue.ToString() });
            req.ConditionExpression = Defines.DefinitionsTableCounter.Value + " = :expectedCounterValue";

            UpdateItemResponse resp;
            try
            {
                resp = client.UpdateItem(req);
            }
            catch (ConditionalCheckFailedException CheckFailedException)
            {
                Debug.WriteLine(CheckFailedException.Message);
                throw;
            }

            // if update succeeded
            if (resp.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Debug.WriteLine("TableName:\t" + resp.Attributes["TableName"].S);
                Debug.WriteLine("TableCounter:\t" + resp.Attributes["TableCounter"].N);
                return true;
            }
            // if update failed
            else
            {
                Debug.WriteLine("failed");
                return false;
            }
        }

        /// <summary>
        /// Gets a counter for a specified TableName and increments it automatically.
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns>counter to use</returns>
        public int GetAndIncrement(string TableName)
        {
            int CurrentCounter = Get(TableName);
            if (CurrentCounter < 0)
            {
                // If it doesn't exist, create a new counter and return the value
                int RetVal = Create(TableName);
                Increment(TableName, RetVal);
                return RetVal;
            }
            else
            {
                // If it does exist, increment it
                Increment(TableName, CurrentCounter);
                return CurrentCounter;
            }

        }

        /// <summary>
        /// Creates a new counter for a new table
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public int Create(string TableName)
        {
            //Debug.WriteLine("===Tables.Accessors.TableCounters.Create===");
            PutItemRequest req = new PutItemRequest();
            req.TableName = Defines.DefinitionsTableCounter.TableName;
            req.Item.Add(Defines.DefinitionsTableCounter.Index, new AttributeValue() { S = TableName });
            req.Item.Add(Defines.DefinitionsTableCounter.Value, new AttributeValue() { N = Defines.DefinitionsTableCounter.DefaultCounter.ToString() });

            PutItemResponse resp = client.PutItem(req);
            if (resp.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return Defines.DefinitionsTableCounter.DefaultCounter;
            }
            else
            {
                return -1;
            }
        }
    }
}
