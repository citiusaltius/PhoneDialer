using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.AuthenticationAndIdentity.Defines
{
    class CognitoIdentity
    {
        public const string IdentityPoolId          = "us-east-1:bf188f97-1103-440d-9773-0af13b61f79b";
        // arn:aws:iam::201495243221:role/Cognito_SynappsedAuth_Role
        public const string DeveloperProviderName   = "login.iyerwong.synappsed";
    }
}
