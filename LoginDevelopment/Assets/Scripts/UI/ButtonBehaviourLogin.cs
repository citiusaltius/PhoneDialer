using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts.UI
{
    public class ButtonBehaviourLogin : MonoBehaviour
    {
        public InputField Email;
        public InputField Password;
        public Button ButtonLogin;
        public Text ButtonLoginText;
        public Text TextFeedback;
        static Coroutine Authenticator;
        static Coroutine LoginSuccessChecker;
        bool WasLoginButtonJustClicked = false;
        bool CancelWaitForLogin = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClick()
        {
            Utilities.Debug.WriteLine("login.");
            // One login underway
            if (LoginSuccessChecker != null)
            {
                CancelWaitForLogin = true;
            }
            // Not under login
            else
            {
                ServerInterface.ServerAuthentication.Creds.Email = Email.text;
                ServerInterface.ServerAuthentication.Creds.Password = Password.text;
                ServerInterface.ServerAuthentication.Creds.DeviceId = SystemInfo.deviceUniqueIdentifier;

                ServerInterface.ServerAuthentication.Creds.GetCredentialsAsync(ServerInterface.ServerAuthentication.Creds.AsyncCredentialsCallback);
                WasLoginButtonJustClicked = true;
                LoginSuccessChecker = StartCoroutine(CheckForLoginSuccessCognitoAwsCredentials());
            }
        }


        /// <summary>
        /// Checks if the login has failed
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckForLoginSuccessCognitoAwsCredentials()
        {
            ButtonLoginText.text = Defines.Global.ButtonLoginStateCancel;
            int i = 0;
            //ProjectPage.Scripts.Utils.DebugBehaviour.WriteLine("Checker: " + "\n" + 
            //    "IsCurrentlyRefreshing: " + GlobalBehaviour.Creds.IsCurrentlyRefreshing + "\n" + 
            //    "IsLoginSuccessful: " + GlobalBehaviour.Creds.IsLoginSuccessful);
            // wait for Coroutine to be started by checking both boolean values
            while (ServerInterface.ServerAuthentication.Creds.IsCurrentlyRefreshing == true || WasLoginButtonJustClicked == true)
            {
                if (ServerInterface.ServerAuthentication.Creds.IsCurrentlyRefreshing == true && WasLoginButtonJustClicked == true) { WasLoginButtonJustClicked = false; }
                if (CancelWaitForLogin == true)
                {
                    ServerInterface.ServerAuthentication.Creds.Clear();
                    CancelWaitForLogin = false;
                    LoginSuccessChecker = null;
                    TextFeedback.text = "login cancelled";
                    ButtonLoginText.text = Defines.Global.ButtonLoginStateLogin;
                    yield break;
                }
                ++i;
                if(i%6 < 2)
                {
                    TextFeedback.text = ".";
                }
                else if (i % 6 < 4)
                {
                    TextFeedback.text = "..";
                } 
                else
                {
                    TextFeedback.text = "...";
                }
                
                yield return new WaitForEndOfFrame();
            }
            if (ServerInterface.ServerAuthentication.Creds.IsLoginSuccessful == true)
            {
                TextFeedback.text = "" + i + ": " + "success";
                // Application.LoadLevel(Defines.Global.SceneNameConversationSelector);
            }
            else
            {

                switch (ServerInterface.ServerAuthentication.Creds.LastWebError.Trim())
                {
                    case "502 Bad Gateway":
                        TextFeedback.text = "cannot reach servers\nplease try again later";
                        break;

                    case "":
                        TextFeedback.text = "incorrect login\nor password";
                        break;

                    default:
                        TextFeedback.text = "login failed\n'" + ServerInterface.ServerAuthentication.Creds.LastWebError + "'";
                        break;
                }
            }
            ButtonLoginText.text = Defines.Global.ButtonLoginStateLogin;
            LoginSuccessChecker = null;
            yield break;

        }

    }
}