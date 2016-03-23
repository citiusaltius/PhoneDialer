using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Amazon.CognitoIdentity;
using Amazon.CognitoIdentity.Model;

namespace SynapsedServerLibrary.AuthenticationAndIdentity.Model
{
    public class CognitoIdentity
    {
        AmazonCognitoIdentityClient client = new AmazonCognitoIdentityClient(SynapsedServerLibrary.Defines.Global.CognitoIdRegion);

        public void DescribeIdentityPool()
        {
            Utilities.Debug.WriteLine("===CognitoIdentity.DescribeIdentityPool===");
            DescribeIdentityPoolRequest req = new DescribeIdentityPoolRequest();
            req.IdentityPoolId = Defines.CognitoIdentity.IdentityPoolId;
            DescribeIdentityPoolResponse resp = client.DescribeIdentityPool(req);
            Utilities.Debug.WriteLine(resp.HttpStatusCode.ToString());
            Utilities.Debug.WriteLine(resp.AllowUnauthenticatedIdentities.ToString());
            Utilities.Debug.WriteLine(resp.DeveloperProviderName);


        }

        /// <summary>
        /// Retrieves a CognitoId and OpenId Token for a IdentityValue string
        /// </summary>
        /// <param name="CognitoIdentifier"></param>
        /// <param name="CognitoId"></param>
        /// <param name="OpenIdToken"></param>
        public void RetrieveCognitoIdentityAndToken(string CognitoIdentifier,
            out string CognitoId, out string OpenIdToken)
        {
            Utilities.Debug.WriteLine("===CognitoIdentity.RetrieveCognitoIdentityAndToken===");
            GetOpenIdTokenForDeveloperIdentityRequest req = new GetOpenIdTokenForDeveloperIdentityRequest();

            req.IdentityPoolId = Defines.CognitoIdentity.IdentityPoolId;
            req.Logins.Add(
                Defines.CognitoIdentity.DeveloperProviderName,
                CognitoIdentifier);

            GetOpenIdTokenForDeveloperIdentityResponse resp;

            try
            {
                resp = client.GetOpenIdTokenForDeveloperIdentity(req);
            }
            catch (AmazonCognitoIdentityException e)
            {
                Utilities.Debug.WriteLine("===exception in GetOpenIdTokenForDeveloperIdentity===");
                throw e;
            }

            //Debug.WriteLine(resp.IdentityId);
            //Debug.WriteLine(resp.Token);

            // if we found/created an ID
            if (resp.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                // return resp.IdentityId;
                CognitoId = resp.IdentityId;
                OpenIdToken = resp.Token;
            }
            // if we didn't find it
            else
            {
                // return null;
                CognitoId = null;
                OpenIdToken = null;
            }


        }


        public string GetCognitoIdentifierForEntityId(int EntityId)
        {
            return "entity=" + EntityId;
        }
    }
}
