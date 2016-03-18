using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using SynapsedServerLibrary.Tables.Defines;
using SynapsedServerLibrary.Utilities;

namespace SynapsedServerLibrary.Tables.Accessors
{
    public class Communities : TableObject
    {
        new DefinitionsCommunities ThisTableDefinition = new DefinitionsCommunities();

        public Communities()
        {
            ThisTableDefinition = new DefinitionsCommunities();
        }

        public Model.Community Create(Model.Community ObjectIn)
        {
            Debug.WriteLine("===Communities.Create===");
            // Request new CommunityId
            TableCounters TableCounterClient = new TableCounters();
            int NewCommunityId = TableCounterClient.GetAndIncrement(ThisTableDefinition.TableName);
            ObjectIn.CommunityId = (NewCommunityId);

            return (Model.Community)base.Create(ObjectIn, ThisTableDefinition.TableName);
        }

        public Model.Community Get(int CommunityId)
        {
            Debug.WriteLine("===Communities.GetInformation===");
            Debug.WriteLine("Retrieving for " + CommunityId);
            Model.Community retval = new Model.Community();
            retval.CommunityId = (CommunityId);
            GetItemRequest req = new GetItemRequest();
            req.Key.Add(ThisTableDefinition.Index, new AttributeValue() { N = CommunityId.ToString() });
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
            //// Get EntityName
            //retval.SetCommunityName(resp.Item[DefinitionsCommunities.FieldNameCommunityName].S);
            //// Convert string to CommunityType
            //retval.SetCommunityType(DefinitionsCommunities.GetCommunityTypeFromString(resp.Item[DefinitionsCommunities.FieldNameCommunityType].S));

            //SetObjectBasePropertiesFromItem(retval, resp.Item);

            retval.LoadFromItem(resp.Item);
            return retval;
        }

        public List<Model.Community> GetAll()
        {
            Debug.WriteLine("===Communities.GetAll===");

            ScanRequest req = new ScanRequest();
            req.TableName = ThisTableDefinition.TableName;

            ScanResponse resp;

            try
            {
                resp = client.Scan(req);
            }
            catch (AmazonDynamoDBException e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                throw e;
            }

            List<Model.Community> AllCommunities = new List<Model.Community>();
            foreach (Dictionary<string, AttributeValue> Item in resp.Items)
            {
                Model.Community NewCommunity = new Model.Community();
                NewCommunity.LoadFromItem(Item);
                AllCommunities.Add(NewCommunity);
            }
            return AllCommunities;
        }
    }
}
