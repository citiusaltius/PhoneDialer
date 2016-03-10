using UnityEngine;
using System.Collections;
using Amazon.CognitoIdentity;
using System.Collections.Generic;
using ThirdParty.Json.LitJson;
using System;
using System.Threading;
using Amazon.Runtime;

namespace Assets.Scripts.ServerInterface
{
    public class CustomCognitoCredentials : CognitoAWSCredentials
    {
        static string PROVIDER_NAME = Defines.Global.CognitoDeveloperProvider;
        static string IDENTITY_POOL = Defines.Global.CognitoIdentityPoolId;
        static readonly Amazon.RegionEndpoint REGION = Amazon.RegionEndpoint.USEast1;

        public string DeviceId
        {
            get;
            set;
        }

        private string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
            }
        }

        private string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }


        private object _RefreshLock = new object();
        public bool IsCurrentlyRefreshing = true;
        public bool IsLoginSuccessful = false;
        public string LastWebError = "";



        public CustomCognitoCredentials() : base(IDENTITY_POOL, REGION)
        {
            //Login = loginAlias;
            //Password = passwordAlias;
        }


        public void ClearLoginPassword()
        {
            _Email = "";
            _Password = "";
        }

        protected override IdentityState RefreshIdentity()
        {
            IdentityState state = null;
            ManualResetEvent waitLock = new ManualResetEvent(false);
            Utilities.MainThreadDispatcher.ExecuteCoroutineOnMainThread(
                ContactProvider((s) =>
                {
                    state = s;
                    waitLock.Set();
                }
            ));
            waitLock.WaitOne();
            return state;
        }



        IEnumerator ContactProvider(Action<IdentityState> callback)
        {
            lock (_RefreshLock)
            {
                IsCurrentlyRefreshing = true;
                IsLoginSuccessful = false;
                LastWebError = "";
            }

            WWWForm Form = new WWWForm();
            Form.AddField("Email", Email);
            Form.AddField("Password", Password);
            Form.AddField("DeviceId", DeviceId);
            Form.AddField("DeviceType", SystemInfo.deviceType.ToString());
            Form.AddField("DeviceModel", SystemInfo.deviceModel.ToString());
            Form.AddField("OperatingSystem", SystemInfo.operatingSystem.ToString());


            WWW www = new WWW(
                Defines.Global.UrlLoginEmailPassword,
                Form
                );
            yield return www;

            // Is there an error returned by the WWW class?
            if (string.IsNullOrEmpty(www.error) == false)
            {
                Utilities.Debug.WriteLine("Error: WWW Error (" + www.error.Length + "):'" + www.error + "'");
                lock (_RefreshLock)
                {
                    IsLoginSuccessful = false;
                    IsCurrentlyRefreshing = false;
                    LastWebError = www.error;
                }
                callback(new IdentityState("", PROVIDER_NAME, "", false));
                yield break;
            }
            else
            {
                
            }

            string response = www.text;
            //ProjectPage.Scripts.Utilities.Debug.WriteLine("ContactProvider response text: " + www.text);
            JsonData json = JsonMapper.ToObject(response);



            if (string.IsNullOrEmpty(json["LoginSuccess"].ToString()) == true ||
                string.IsNullOrEmpty(json["IdentityId"].ToString()) == true ||
                string.IsNullOrEmpty(json["OpenIdToken"].ToString()) == true
                )
            {
                Utilities.Debug.WriteLine("Error: improper data from login server.");
                lock (_RefreshLock)
                {
                    IsLoginSuccessful = false;
                    IsCurrentlyRefreshing = false;
                }
                callback(new IdentityState("", PROVIDER_NAME, "", false));
                yield break;
            }

            //The backend has to send us back an Identity and a OpenID token
            bool LoginSuccess = (bool)(json["LoginSuccess"]);
            string IdentityId = json["IdentityId"].ToString();
            string Token = json["OpenIdToken"].ToString();

            IdentityState state = new IdentityState(IdentityId, PROVIDER_NAME, Token, false);
            callback(state);
            // refresh complete and successful
            lock (_RefreshLock)
            {
                IsCurrentlyRefreshing = false;
                IsLoginSuccessful = LoginSuccess;
            }
        }

        public void AsyncCredentialsCallback(AmazonCognitoIdentityResult<ImmutableCredentials> result)
        {
            //Utilities.Debug.WriteLine(
            //    "AsyncCredentialsCallback:===== \n" +
            //    "IsCurrentlyRefreshing: " + IsCurrentlyRefreshing + "\n" +
            //    "IsLoginSuccessful: " + IsLoginSuccessful);
            if (IsCurrentlyRefreshing == false && IsLoginSuccessful == true)
            {
                Utilities.Debug.WriteLine("IdentityId: " + this.GetIdentityId());
            }
        }

        public new void Clear()
        {
            IsCurrentlyRefreshing = false;
            IsLoginSuccessful = false;
            base.Clear();
        }
    }
}
