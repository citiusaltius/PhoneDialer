using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using SynapsedServerLibrary.Tables.Defines;
using SynapsedServerLibrary.Utilities;

namespace SynapsedServerLibrary.Tables.Accessors
{
    public class ApplicationMemberships : TableObject
    {
        new DefinitionsApplicationMemberships ThisTableDefinition = new DefinitionsApplicationMemberships();

        public ApplicationMemberships()
        {
            ThisTableDefinition = new DefinitionsApplicationMemberships();
        }


        public Model.ApplicationMembership Create(Model.ApplicationMembership ObjectIn)
        {
            Debug.WriteLine("===ApplicationMemberships.Create===");
            return (Model.ApplicationMembership)base.Create(ObjectIn, ThisTableDefinition.TableName);
        }

        public Model.ApplicationMembership Get(int ApplicationMembershipId)
        {
            Debug.WriteLine("===Application.Get===");
            Debug.WriteLine("Retrieving for " + ApplicationMembershipId);
            Model.ApplicationMembership retval = new Model.ApplicationMembership();
            retval.ApplicationMembershipId = (ApplicationMembershipId);
            GetItemRequest req = new GetItemRequest();
            req.Key.Add(ThisTableDefinition.Index, new AttributeValue() { N = ApplicationMembershipId.ToString() });
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

        public List<Model.ApplicationMembership> GetAll()
        {
            Debug.WriteLine("===ApplicationMembership.GetAll===");

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

            List<Model.ApplicationMembership> AllApplications = new List<Model.ApplicationMembership>();
            foreach (Dictionary<string, AttributeValue> Item in resp.Items)
            {
                Model.ApplicationMembership NewApplicationMembership = new Model.ApplicationMembership();
                NewApplicationMembership.LoadFromItem(Item);
                AllApplications.Add(NewApplicationMembership);
            }
            return AllApplications;
        }
    }
}
