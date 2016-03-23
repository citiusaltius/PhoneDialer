using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.AuthenticationAndIdentity.Model
{
    public class IdentityIdOpenIdToken : Base.BaseObject
    {
        private bool _LoginSuccess = false;
        public bool LoginSuccess
        {
            get
            {
                return _LoginSuccess;
            }
            set
            {
                _LoginSuccess = value;
            }
        }

        private string _IdentityId;
        public string IdentityId
        {
            get
            {
                return _IdentityId;
            }
            set
            {
                _IdentityId = value;
            }
        }


        private string _OpenIdToken;
        public string OpenIdToken
        {
            get
            {
                return _OpenIdToken;
            }
            set
            {
                _OpenIdToken = value;
            }
        }

        public IdentityIdOpenIdToken(string Id, string Token)
        {
            IdentityId = Id;
            OpenIdToken = Token;
        }
    }
}
