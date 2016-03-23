using System;

namespace SynapsedServerLibrary.AuthenticationAndIdentity.Model
{
    public class ServerCredentials : Base.BaseObject
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

        private string _AccessKey;
        public string AccessKey
        {
            get
            {
                return _AccessKey;
            }
            set
            {
                _AccessKey = value;
            }
        }

        private string _SecretAccessKey;
        public string SecretAccessKey
        {
            get
            {
                return _SecretAccessKey;
            }
            set
            {
                _SecretAccessKey = value;
            }
        }

        private string _SessionToken;
        public string SessionToken
        {
            get
            {
                return _SessionToken;
            }
            set
            {
                _SessionToken = value;
            }
        }

        private DateTime _DateTimeExpiration;
        [System.Web.Script.Serialization.ScriptIgnore]
        public DateTime DateTimeExpiration
        {
            get
            {
                return _DateTimeExpiration;
            }
            set
            {
                _DateTimeExpiration = value;
                _TimestampExpiration = value.ToString(SynapsedServerLibrary.Defines.Global.DateTimeFormatAmz);
            }
        }

        private string _TimestampExpiration;
        public string TimestampExpiration
        {
            get
            {
                return _TimestampExpiration;
            }
            set
            {
                _TimestampExpiration = value;
            }
        }

        private string _RoleSessionName;
        public string RoleSessionName
        {
            get
            {
                return _RoleSessionName;
            }
            set
            {
                _RoleSessionName = value;
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

        public void LoadFromAwsCredentials(Amazon.SecurityToken.Model.Credentials Credentials)
        {
            AccessKey = Credentials.AccessKeyId;
            SecretAccessKey = Credentials.SecretAccessKey;
            SessionToken = Credentials.SessionToken;
            DateTimeExpiration = Credentials.Expiration;
        }


    }
}
