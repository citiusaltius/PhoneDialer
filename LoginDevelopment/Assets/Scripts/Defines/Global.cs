using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Defines
{
    public class Global
    {
        public const string UrlLoginEmailPassword       = "http://52.9.134.236/login/submit/mobiletoken";
        public const string CognitoDeveloperProvider    = "login.iyerwong.synappsed";
        public const string CognitoIdentityPoolId       = "us-east-1:bf188f97-1103-440d-9773-0af13b61f79b";
        // arn:aws:iam::201495243221:role/Cognito_SynappsedAuth_Role
        // arn:aws:iam::201495243221:role/Cognito_SynappsedUnauth_Role
        public const string DateTimeFormatAmz           = "yyyyMMddTHHmmssZ";

        #region SceneLogin
        public static string ButtonLoginStateLogin = "login";
        public static string ButtonLoginStateCancel = "cancel";
        #endregion
    }
}
