using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynapsedServerLibrary.Utilities;
using SynapsedServerLibrary.Tables.Accessors;

namespace SynapsedServer
{
    class Tests
    {
        public void PrintDivider()
        {
            string DividerLine = "================================================";
            SynapsedServerLibrary.Utilities.Debug.WriteLine(DividerLine);
            SynapsedServerLibrary.Utilities.Debug.WriteLine(SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz());
            SynapsedServerLibrary.Utilities.Debug.WriteLine(
                "Parsing test:\t" +
                SynapsedServerLibrary.Defines.Global.ParseAmzDateTimeFormat(SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz()).ToString()
                );
            SynapsedServerLibrary.Utilities.Debug.WriteLine(DividerLine);

        }

        //public void TestCognito()
        //{
        //    PrintDivider();
        //    SynapsedServerLibrary.Utilities.Debug.WriteLine("===Tests.TestCognito===");
        //    TestCognitoDescribeIdentityPool();
        //    TestCognitoGetOpenIdToken();
        //}

        //void TestCognitoDescribeIdentityPool()
        //{
        //    SynapsedServerLibrary.Utilities.Debug.WriteLine("===Tests.TestCognitoDescribeIdentityPool===");
        //    SynapsedServerLibrary.Tables.Identification.CognitoIdentity IdentityClient = new SynapsedServerLibrary.Tables.Identification.CognitoIdentity();
        //    IdentityClient.DescribeIdentityPool();
        //}

        //void TestCognitoGetOpenIdToken()
        //{
        //    Debug.WriteLine("===Tests.TestCognitoGetOpenIdToken===");
        //    string CognitoId;
        //    string OpenIdToken;
        //    SynapsedServerLibrary.Tables.Identification.CognitoIdentity CognitoIdentityClient = new SynapsedServerLibrary.Tables.Identification.CognitoIdentity();
        //    CognitoIdentityClient.RetrieveCognitoIdentityAndToken("entity:test.com",
        //        out CognitoId, out OpenIdToken);

        //    if (CognitoId != null && OpenIdToken != null)
        //    {
        //        Debug.WriteLine("CognitoId:\t" + CognitoId);
        //        Debug.WriteLine("OpenIdToken:\t" + OpenIdToken);
        //    }
        //    else
        //    {
        //        Debug.WriteLine("Invalid result");
        //    }
        //}

        public void TestTableCounter()
        {
            PrintDivider();
            SynapsedServerLibrary.Utilities.Debug.WriteLine("===TestTableCounter===");
            string TableCounterToCreate = "ZZ Test " + SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz();
            SynapsedServerLibrary.Tables.Accessors.TableCounters TableCountersClient = new SynapsedServerLibrary.Tables.Accessors.TableCounters();
            SynapsedServerLibrary.Utilities.Debug.WriteLine("Counter created:\t" + TableCounterToCreate + ":" + TableCountersClient.Create(TableCounterToCreate));

            int EntitiesCounter = TableCountersClient.Get("Entities");
            SynapsedServerLibrary.Utilities.Debug.WriteLine("Entities:\t" + EntitiesCounter);
            TableCountersClient.Increment("Entities", EntitiesCounter);
            SynapsedServerLibrary.Utilities.Debug.WriteLine("DoesNotExist:\t" + TableCountersClient.Get("DoesNotExist"));
        }

        public void TestEntities()
        {
            PrintDivider();
            Debug.WriteLine("===starting Entities test===");
            SynapsedServerLibrary.Tables.Model.Entity ToBeCreated = new SynapsedServerLibrary.Tables.Model.Entity();
            ToBeCreated.EntityName = ("ZZ Test " + SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz());
            ToBeCreated.EntityType = (SynapsedServerLibrary.Tables.Defines.DefinitionsEntities.EntityTypes.Person);
            ToBeCreated.DataVersion = (SynapsedServerLibrary.Defines.Global.DataVersionCurrent);
            Entities Client = new Entities();
            SynapsedServerLibrary.Tables.Model.Entity Created = Client.Create(ToBeCreated);

            SynapsedServerLibrary.Tables.Model.Entity Retrieved = Client.Get(Created.EntityId);
            Debug.WriteLine(Retrieved.ToString());

            Retrieved.EntityName = (Retrieved.EntityName + " updated");
            Client.Update(Retrieved);

            Retrieved = Client.Get(Created.EntityId);
            Debug.WriteLine(Retrieved.PrintProperties());

            Client.Delete(Retrieved);
            Debug.WriteLine("===completed Entities test===");
            Console.ReadLine();

        }

        public void TestIdentities()
        {
            PrintDivider();
            Debug.WriteLine("===starting Identities test===");
            SynapsedServerLibrary.Tables.Model.Identity ToBeCreated = new SynapsedServerLibrary.Tables.Model.Identity();
            ToBeCreated.IdentityName = ("ZZ Test " + SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz());

            ToBeCreated.IdentityType = (SynapsedServerLibrary.Tables.Defines.DefinitionsIdentities.IdentityTypes.Person);
            // use latest CommunityId
            TableCounters TableCountersClient = new TableCounters();
            ToBeCreated.CommunityId = (TableCountersClient.Get(new SynapsedServerLibrary.Tables.Defines.DefinitionsCommunities().TableName));

            ToBeCreated.DataVersion = (SynapsedServerLibrary.Defines.Global.DataVersionCurrent);
            Identities Client = new Identities();
            SynapsedServerLibrary.Tables.Model.Identity Created = Client.Create(ToBeCreated);

            SynapsedServerLibrary.Tables.Model.Identity Retrieved = Client.Get(Created.IdentityId);
            Debug.WriteLine(Retrieved.ToString());

            Retrieved.IdentityName = (Retrieved.IdentityName + " updated");
            Client.Update(Retrieved);

            Retrieved = Client.Get(Created.IdentityId);
            Debug.WriteLine(Retrieved.PrintProperties());

            Client.Delete(Retrieved);
            Debug.WriteLine("===completed Identities test===");
            Console.ReadLine();
        }

        public void TestCommunities()
        {
            PrintDivider();
            Debug.WriteLine("===starting Communities test===");

            SynapsedServerLibrary.Tables.Model.Community ToBeCreated = new SynapsedServerLibrary.Tables.Model.Community();
            ToBeCreated.CommunityName = ("ZZ Test " + SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz());

            ToBeCreated.CommunityType = (SynapsedServerLibrary.Tables.Defines.DefinitionsCommunities.CommunityTypes.Hospital);

            ToBeCreated.AcceptableWebDomains.Add("testdomain1.com");
            ToBeCreated.AcceptableWebDomains.Add("testdomain2.org");
            ToBeCreated.AcceptableWebDomains.Add("testdomain3.net");

            ToBeCreated.DataVersion = (SynapsedServerLibrary.Defines.Global.DataVersionCurrent);
            Communities Client = new Communities();
            SynapsedServerLibrary.Tables.Model.Community Created = Client.Create(ToBeCreated);

            SynapsedServerLibrary.Tables.Model.Community Retrieved = Client.Get(Created.CommunityId);
            Debug.WriteLine(Retrieved.PrintProperties());

            List<SynapsedServerLibrary.Tables.Model.Community> AllCommunities = Client.GetAll();
            foreach (SynapsedServerLibrary.Tables.Model.Community ACommunity in AllCommunities)
            {
                Debug.WriteLine(ACommunity.PrintProperties());
            }

            Retrieved.CommunityName = (Retrieved.CommunityName + " updated");
            Client.Update(Retrieved);

            Retrieved = Client.Get(Created.CommunityId);
            Debug.WriteLine(Retrieved.PrintProperties());

            Client.Delete(Retrieved);
            Debug.WriteLine("===completed Communities test===");
            Console.ReadLine();
        }

        public void TestContactMethods()
        {
            PrintDivider();
            Debug.WriteLine("===starting ContactMethods test===");
            SynapsedServerLibrary.Tables.Model.ContactMethod ToBeCreated = new SynapsedServerLibrary.Tables.Model.ContactMethod();
            ToBeCreated.ContactMethodName = ("ZZ Test " + SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz());
            ToBeCreated.ContactInformation = ("a@a.com");
            ToBeCreated.ContactMethodType = (SynapsedServerLibrary.Tables.Defines.DefinitionsContactMethods.ContactMethodTypes.Email);
            // use latest IdentityId

            TableCounters TableCountersClient = new TableCounters();
            ToBeCreated.IdentityId = (TableCountersClient.Get(new SynapsedServerLibrary.Tables.Defines.DefinitionsCommunities().TableName));

            ToBeCreated.DataVersion = (SynapsedServerLibrary.Defines.Global.DataVersionCurrent);


            ContactMethods Client = new ContactMethods();
            SynapsedServerLibrary.Tables.Model.ContactMethod Created = Client.Create(ToBeCreated);

            SynapsedServerLibrary.Tables.Model.ContactMethod Retrieved = Client.Get(Created.ContactMethodId);
            Debug.WriteLine(Retrieved.ToString());



            List<SynapsedServerLibrary.Tables.Model.ContactMethod> RetrievedContactMethods = Client.FindByInformation("a@a.com");
            Debug.WriteLine(RetrievedContactMethods.Count + " records found");
            foreach (SynapsedServerLibrary.Tables.Model.ContactMethod RetrievedContactMethod in RetrievedContactMethods)
            {
                Debug.WriteLine(RetrievedContactMethod.PrintProperties());
            }

            Retrieved.ContactMethodName = (Retrieved.ContactMethodName + " updated");
            Client.Update(Retrieved);

            Retrieved = Client.Get(Created.ContactMethodId);
            Debug.WriteLine(Retrieved.PrintProperties());

            Client.Delete(Retrieved);
            Debug.WriteLine("===completed ContactMethods test===");
            Console.ReadLine();
        }


        public void TestCommunityMemberships()
        {
            PrintDivider();
            Debug.WriteLine("===starting CommunityMemberships test===");
            SynapsedServerLibrary.Tables.Model.CommunityMembership ToBeCreated = new SynapsedServerLibrary.Tables.Model.CommunityMembership();
            ToBeCreated.SetCommunityId(1);
            ToBeCreated.SetIdentityId(1);

            ToBeCreated.DataVersion = (SynapsedServerLibrary.Defines.Global.DataVersionCurrent);
            CommunityMemberships Client = new CommunityMemberships();
            SynapsedServerLibrary.Tables.Model.CommunityMembership Created = Client.Create(ToBeCreated);

            SynapsedServerLibrary.Tables.Model.CommunityMembership Retrieved = Client.Get(Created.CommunityMembershipId);
            Debug.WriteLine(Retrieved.ToString());


            Client.Update(Retrieved);

            Retrieved = Client.Get(Created.CommunityMembershipId);
            Debug.WriteLine(Retrieved.PrintProperties());

            Client.Delete(Retrieved);
            Debug.WriteLine("===completed CommunityMemberships test===");
            Console.ReadLine();
        }

        public void TestSecurityInformation()
        {
            PrintDivider();
            Debug.WriteLine("===starting SecurityInformation test===");
            SynapsedServerLibrary.Tables.Model.SecurityInfo ToBeCreated = new SynapsedServerLibrary.Tables.Model.SecurityInfo();
            ToBeCreated.EntityId = (1);
            ToBeCreated.SecurityInformationType = (SynapsedServerLibrary.Tables.Defines.DefinitionsSecurityInformations.SecurityInformationTypes.HashedPassword);
            ToBeCreated.SecurityInformation = ("Information");
            ToBeCreated.NumberOfUses = (10);
            ToBeCreated.MaxNumberOfUses = (100);

            ToBeCreated.DataVersion = (SynapsedServerLibrary.Defines.Global.DataVersionCurrent);
            SecurityInformations Client = new SecurityInformations();
            SynapsedServerLibrary.Tables.Model.SecurityInfo Created = Client.Create(ToBeCreated);

            SynapsedServerLibrary.Tables.Model.SecurityInfo Retrieved = Client.Get(Created.SecurityInformationId);
            Debug.WriteLine(Retrieved.ToString());


            Client.Update(Retrieved);

            Retrieved = Client.Get(Created.SecurityInformationId);
            Debug.WriteLine(Retrieved.PrintProperties());

            Client.Delete(Retrieved);

            Debug.WriteLine("===completed SecurityInformation test===");
            Console.ReadLine();
        }



    }
}
