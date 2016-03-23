using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;

namespace SynapsedServerLibrary.AuthenticationAndIdentity.Model
{
    public class SecurityToken
    {
        AmazonSecurityTokenServiceClient client = new AmazonSecurityTokenServiceClient(SynapsedServerLibrary.Defines.Global.SecurityTokenRegion);

        public bool RetrieveCredentialsForToken(string RoleSessionName, string IdentityToken,
            out ServerCredentials ServerCredentials)
        {
            AssumeRoleWithWebIdentityRequest Req = new AssumeRoleWithWebIdentityRequest();
            AssumeRoleWithWebIdentityResponse Resp;

            Req.DurationSeconds = SynapsedServerLibrary.Defines.Global.SecurityTokenDuration;
            Req.RoleArn = Defines.SecurityToken.RoleArn;
            Req.RoleSessionName = RoleSessionName;
            Req.WebIdentityToken = IdentityToken;

            try
            {
                Resp = client.AssumeRoleWithWebIdentity(Req);
            }
            catch (AmazonSecurityTokenServiceException e)
            {
                Utilities.Debug.WriteLine("Error in retrieving AWS credentials for Token");
                Utilities.Debug.WriteLine(e.Message);
                Utilities.Debug.WriteLine(e.Source);
                Utilities.Debug.WriteLine(e.StackTrace);
                throw;
            }

            Utilities.Debug.WriteLine(Resp.AssumedRoleUser.AssumedRoleId);


            ServerCredentials = new ServerCredentials();
            ServerCredentials.LoadFromAwsCredentials(Resp.Credentials);
            ServerCredentials.RoleSessionName = Resp.AssumedRoleUser.AssumedRoleId;
            if (Resp.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                ServerCredentials.LoginSuccess = true;
                return true;
            }
            else
            {
                ServerCredentials = new ServerCredentials();
                return false;
            }

        }
    }
}
