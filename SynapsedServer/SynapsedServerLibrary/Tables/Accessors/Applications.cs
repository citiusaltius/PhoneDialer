﻿using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using SynapsedServerLibrary.Tables.Defines;
using SynapsedServerLibrary.Utilities;

namespace SynapsedServerLibrary.Tables.Accessors
{
    public class Applications : TableObject
    {
        new DefinitionsApplications ThisTableDefinition = new DefinitionsApplications();

        public Applications()
        {
            ThisTableDefinition = new DefinitionsApplications();
        }


        public Model.Application Create(Model.Application ObjectIn)
        {
            
            Debug.WriteLine("===Applications.Create===");
            return (Model.Application)base.Create(ObjectIn, ThisTableDefinition.TableName);
        }

        public Model.Application Get(int ApplicationId)
        {
            Debug.WriteLine("===Application.Get===");
            Debug.WriteLine("Retrieving for " + ApplicationId);
            Model.Application retval = new Model.Application();
            retval.ApplicationId = (ApplicationId);
            GetItemRequest req = new GetItemRequest();
            req.Key.Add(ThisTableDefinition.Index, new AttributeValue() { N = ApplicationId.ToString() });
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

        public List<Model.Application> GetAll()
        {
            Debug.WriteLine("===Applications.GetAll===");

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

            List<Model.Application> AllApplications = new List<Model.Application>();
            foreach (Dictionary<string, AttributeValue> Item in resp.Items)
            {
                Model.Application NewApplication = new Model.Application();
                NewApplication.LoadFromItem(Item);
                AllApplications.Add(NewApplication);
            }
            return AllApplications;
        }
    }
}
