using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SynapsedServerLibrary.Tables.Accessors;
using SynapsedServerLibrary.Tables.Model;

namespace SynapsedServer.NancyModules
{
    public class LoginModule : Nancy.NancyModule
    {
        public LoginModule() : base("/login")
        {


            Get["/", true] = async (parameters, token) =>
            {
                StringBuilder ReturnedPage = new StringBuilder();
                // spins for a little
                await Task.Run(() => { for (int i = 0; i < 1; i++) { } });
                ReturnedPage.AppendLine(@"
                <html>
                <body>
                <br /> <a href='/'>index</a> <br /> 
                <h1> registration </h1>
                <br /> 
                <form id='login' action='/login/submit' method='post' accept-charset='UTF-8'>
                <fieldset > 
                <legend>Login</legend> 
                <input type='hidden' name='submitted' id='submitted' value='1'/> 
                <table>");

                ReturnedPage.AppendLine(@"
                <tr>
                <td><label for= 'email' > e-mail address *:</label></td>
                <td><input type = 'text' name = 'email' id = 'email' maxlength = '50' /></td>
                </tr>
                <tr>
                <td><label for= 'password' > password *:</label ></td>
                <td><input type = 'password' name = 'password' id = 'password' maxlength = '50' /></td>
                </tr>

");

                ReturnedPage.AppendLine(@"
                </table>
                <input type = 'submit' name = 'Submit' value = 'Submit' />
                </fieldset >
                </body>
                </html>
");
                return ReturnedPage.ToString();
            };

            Post["/submit", true] = async (parameters, token) =>
            {

                StringBuilder ReturnedPage = new StringBuilder();
                ReturnedPage.AppendLine("<h1>login submitted</h1>");
                // spins for a little
                await Task.Run(() => { for (int i = 0; i < 1; i++) { } });
                ReturnedPage.AppendLine(new System.IO.StreamReader(this.Request.Body).ReadToEnd());
                string ValidationOutput;
                SynapsedServerLibrary.AuthenticationAndIdentity.Model.ServerCredentials RetrievedCredentials;
                bool IsValidated = ValidateLoginAndGetCredentials(this.Request.Form.Email, this.Request.Form.Password, this.Request.Form.DeviceId, out ValidationOutput, out RetrievedCredentials);

                if (IsValidated == false)
                {
                    ReturnedPage.AppendLine("<br/><h1>Login Failed</h1>");
                    return ReturnedPage.ToString();
                }

                ReturnedPage.AppendLine("<br/><h1>Login Successful</h1>");
                ReturnedPage.AppendLine(RetrievedCredentials.AsJson());
                ReturnedPage.AppendLine("<br/><hr><br/>");
                System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                ReturnedPage.AppendLine(JsonSerializer.Serialize(RetrievedCredentials));
                ReturnedPage.AppendLine(ValidationOutput);
                return ReturnedPage.ToString();
            };


            Post["/submit/mobile", true] = async (parameters, token) =>
            {
                StringBuilder ReturnedPage = new StringBuilder();
                // spins for a little
                await Task.Run(() => { for (int i = 0; i < 1; i++) { } });
                string ValidationOutput;
                SynapsedServerLibrary.AuthenticationAndIdentity.Model.ServerCredentials RetrievedCredentials = new SynapsedServerLibrary.AuthenticationAndIdentity.Model.ServerCredentials();
                bool IsValidated = false;

                // If the request is valid, then validate the login
                if (this.Request.Form.Email != "" &&
                    this.Request.Form.Password != "" &&
                    this.Request.Form.DeviceId != "")
                {
                    IsValidated = ValidateLoginAndGetCredentials(this.Request.Form.Email, this.Request.Form.Password, this.Request.Form.DeviceId, out ValidationOutput, out RetrievedCredentials);
                }

                ReturnedPage.AppendLine(RetrievedCredentials.AsJson());

                if (IsValidated == true)
                {
                    //ReturnedPage.AppendLine("<br/><h1>Login Successful</h1>");

                }
                else
                {
                    //ReturnedPage.AppendLine("<br/><h1>Login Failed</h1>");

                }
                return ReturnedPage.ToString();
            };

            Post["/submit/mobiletoken", true] = async (parameters, token) =>
            {
                StringBuilder ReturnedPage = new StringBuilder();
                // spins for a little
                await Task.Run(() => { for (int i = 0; i < 1; i++) { } });
                string ValidationOutput;
                SynapsedServerLibrary.AuthenticationAndIdentity.Model.IdentityIdOpenIdToken RetrievedToken = new SynapsedServerLibrary.AuthenticationAndIdentity.Model.IdentityIdOpenIdToken("", "");
                bool IsValidated = false;

                // If the request is valid, then validate the login
                if (this.Request.Form.Email != "" &&
                    this.Request.Form.Password != "" &&
                    this.Request.Form.DeviceId != "")
                {
                    IsValidated = ValidateLoginAndGetToken(this.Request.Form.Email, this.Request.Form.Password, this.Request.Form.DeviceId, out ValidationOutput, out RetrievedToken);
                }

                ReturnedPage.AppendLine(RetrievedToken.AsJson());

                if (IsValidated == true)
                {
                    //ReturnedPage.AppendLine("<br/><h1>Login Successful</h1>");

                }
                else
                {
                    //ReturnedPage.AppendLine("<br/><h1>Login Failed</h1>");

                }
                return ReturnedPage.ToString();
            };

        }

        private bool ValidateLogin(string Email, string Password, string DeviceId, out string Output, out string CognitoId, out string OpenIdToken, out Entity ThisEntity)
        {
            ThisEntity = new Entity();
            CognitoId = String.Empty;
            OpenIdToken = String.Empty;

            Entities EntityClient = new Entities();
            Identities IdentitiesClient = new Identities();
            ContactMethods ContactMethodsClient = new ContactMethods();
            SecurityInformations SecurityInformationClient = new SecurityInformations();
            UserDevices UserDevicesClient = new UserDevices();

            StringBuilder RetVal = new StringBuilder();

            List<ContactMethod> FoundContactMethods = ContactMethodsClient.FindByInformation(Email);

            List<ContactMethod> ValidContactMethods = new List<ContactMethod>();
            List<ContactMethod> ExpiredContactMethods = new List<ContactMethod>();
            foreach (ContactMethod FoundContactMethod in FoundContactMethods)
            {
                //SynapsedServerLibrary.Utilities.Debug.WriteLine(FoundContactMethod.PrintProperties(false));
                RetVal.AppendLine(FoundContactMethod.PrintProperties(true));
                DateTime DatetimeExpiration = SynapsedServerLibrary.Defines.Global.ParseAmzDateTimeFormat(FoundContactMethod.TimestampExpiration);
                if (DatetimeExpiration > DateTime.Now)
                {
                    ValidContactMethods.Add(FoundContactMethod);
                }
                else
                {
                    ExpiredContactMethods.Add(FoundContactMethod);
                }
            }

            if (ValidContactMethods.Count == 0)
            {
                RetVal.AppendLine("<br/>No matching valid contact methods");
                if (ExpiredContactMethods.Count > 0)
                {
                    RetVal.AppendLine("<br/>There are contact method entries that seem to have expired");
                    throw new NotImplementedException("TODO: implement contact method revalidation");
                }

                Output = RetVal.ToString();
                return false;
            }

            if (ValidContactMethods.Count > 1)
            {
                RetVal.AppendLine("<br/>Error: too many active matching contact methods");
                Output = RetVal.ToString();
                return false;
            }

            // If we get here, it's a valid entry
            ContactMethod ThisContactMethod = ValidContactMethods[0];
            if (ThisContactMethod.IsLoginable == false)
            {
                RetVal.AppendLine("<br/>Cannot be used to log in.");
                Output = RetVal.ToString();
                return false;
            }
            Identity ThisIdentity = IdentitiesClient.Get(ThisContactMethod.IdentityId);
            ThisEntity = EntityClient.Get(ThisIdentity.EntityId);

            SynapsedServerLibrary.Security.Hashing Hasher = new SynapsedServerLibrary.Security.Hashing();
            string HashedPassword = Hasher.PasswordHash(ThisEntity.EntitySalt, Password);
            List<SecurityInfo> SecurityInfos = SecurityInformationClient.FindByInformation(ThisEntity.EntityId, HashedPassword);

            List<SecurityInfo> ValidSecurityInfos = new List<SecurityInfo>();
            foreach (SecurityInfo ExaminedSecurityInfo in SecurityInfos)
            {
                // remove overused options
                if (ExaminedSecurityInfo.NumberOfUses >= ExaminedSecurityInfo.MaxNumberOfUses)
                {
                    continue;
                }

                ValidSecurityInfos.Add(ExaminedSecurityInfo);

            }
            Output = RetVal.ToString();
            SynapsedServerLibrary.Utilities.Debug.WriteLine(Output);
            // If no valid security information, break
            if (ValidSecurityInfos.Count == 0)
            {
                return false;
            }


            SynapsedServerLibrary.AuthenticationAndIdentity.Model.CognitoIdentity CognitoIdentityClient = new SynapsedServerLibrary.AuthenticationAndIdentity.Model.CognitoIdentity();

            // Get CognitoIdentity and Token
            CognitoIdentityClient.RetrieveCognitoIdentityAndToken(
                CognitoIdentityClient.GetCognitoIdentifierForEntityId(ThisEntity.EntityId),
                out CognitoId, out OpenIdToken);
            // Must have valid security info here

            // Create UserDevice
            SynapsedServerLibrary.Queues.UserDeviceQueue QueueToUse = new SynapsedServerLibrary.Queues.UserDeviceQueue();
            QueueToUse.CognitoId = CognitoId;
            QueueToUse.DeviceId = DeviceId;

            //SynapsedServerLibrary.Utilities.Debug.WriteLine(ThisEntity.EntityCognitoId.Replace(":", "-") + "_" +                    DeviceId + "\n" +

            //        SynapsedServerLibrary.Security.Hashing.Sha256(
            //            ThisEntity.EntityCognitoId.Replace(":", "-") + "_" +
            //            DeviceId
            //            ) + "\n" + 

            //        QueueToUse.DeviceQueueName
            //        );

            List<UserDevice> FoundUserDevices = UserDevicesClient.FindByEntityIdDeviceId(ThisEntity.EntityId, DeviceId);

            RetVal.AppendLine("Found: " + FoundUserDevices.Count + " devices");

            if (FoundUserDevices.Count == 0)
            {
                UserDevice NewDevice = new UserDevice();
                NewDevice.EntityId = ThisEntity.EntityId;
                NewDevice.DeviceId = DeviceId;
                NewDevice.LastLoginTimestamp = SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz();
                // TODO: Will need to confirm uniqueness of queue URL
                NewDevice.DeviceQueueName = SynapsedServerLibrary.Security.Hashing.Sha256(
                    ThisEntity.EntityCognitoId.Replace(":", "-") + "_" +
                    DeviceId
                    );
                NewDevice = UserDevicesClient.Create(NewDevice);
                ThisEntity.LastLoginUserDeviceId = NewDevice.UserDeviceId;
            }
            else if (FoundUserDevices.Count == 1)
            {
                UserDevice FoundDevice = FoundUserDevices[0];
                FoundDevice.LastLoginTimestamp = SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz();
                UserDevicesClient.Update(FoundDevice);
                ThisEntity.LastLoginUserDeviceId = FoundDevice.UserDeviceId;
            }
            else // Have more than one entry for a device - what do I do?
            {
                // TODO 
                throw new NotImplementedException();
            }
            // Update Entity with last login device
            EntityClient.Update(ThisEntity);
            return true;
        }

        private bool ValidateLoginAndGetCredentials(string Email, string Password, string DeviceId, out string Output, out SynapsedServerLibrary.AuthenticationAndIdentity.Model.ServerCredentials RetrievedCredentials)
        {
            StringBuilder RetVal = new StringBuilder();
            Entity ThisEntity = new Entity();
            string TempOutput = "";

            string CognitoId = null;
            string OpenIdToken = null;

            if (ValidateLogin(Email, Password, DeviceId, out TempOutput, out CognitoId, out OpenIdToken, out ThisEntity) == false)
            {
                RetrievedCredentials = new SynapsedServerLibrary.AuthenticationAndIdentity.Model.ServerCredentials();
                Output = TempOutput;
                return false;
            }
            RetVal.AppendLine(TempOutput);

            SynapsedServerLibrary.AuthenticationAndIdentity.Model.CognitoIdentity CognitoIdentityClient = new SynapsedServerLibrary.AuthenticationAndIdentity.Model.CognitoIdentity();

            //// Get CognitoIdentity and Token
            //CognitoIdentityClient.RetrieveCognitoIdentityAndToken(
            //    CognitoIdentityClient.GetCognitoIdentifierForEntityId(ThisEntity.EntityId),
            //    out CognitoId, out OpenIdToken);

            SynapsedServerLibrary.AuthenticationAndIdentity.Model.SecurityToken SecurityTokenClient = new SynapsedServerLibrary.AuthenticationAndIdentity.Model.SecurityToken();


            SecurityTokenClient.RetrieveCredentialsForToken(
                 CognitoIdentityClient.GetCognitoIdentifierForEntityId(ThisEntity.EntityId),
                 OpenIdToken, out RetrievedCredentials);
            RetrievedCredentials.IdentityId = CognitoId;
            RetVal.AppendLine(RetrievedCredentials.PrintProperties(true));
            Output = RetVal.ToString();
            return true;
        }

        private bool ValidateLoginAndGetToken(string Email, string Password, string DeviceId, out string Output, out SynapsedServerLibrary.AuthenticationAndIdentity.Model.IdentityIdOpenIdToken RetrievedCredentials)
        {
            StringBuilder RetVal = new StringBuilder();
            Entity ThisEntity = new Entity();
            string TempOutput = "";
            string CognitoId = "";
            string OpenIdToken = "";


            if (ValidateLogin(Email, Password, DeviceId, out TempOutput, out CognitoId, out OpenIdToken, out ThisEntity) == false)
            {
                RetrievedCredentials = new SynapsedServerLibrary.AuthenticationAndIdentity.Model.IdentityIdOpenIdToken("", "");
                Output = TempOutput;
                return false;
            }
            RetVal.AppendLine(TempOutput);



            RetrievedCredentials = new SynapsedServerLibrary.AuthenticationAndIdentity.Model.IdentityIdOpenIdToken(CognitoId, OpenIdToken);
            RetrievedCredentials.LoginSuccess = true;

            RetVal.AppendLine(RetrievedCredentials.PrintProperties(true));

            SynapsedServerLibrary.Queues.UserQueues DeviceQueues = new SynapsedServerLibrary.Queues.UserQueues();

            SynapsedServerLibrary.Queues.UserDeviceQueue DeviceSpecificQueue = new SynapsedServerLibrary.Queues.UserDeviceQueue();
            DeviceSpecificQueue.CognitoId = CognitoId;
            DeviceSpecificQueue.DeviceId = DeviceId;

            SynapsedServerLibrary.Utilities.Debug.WriteLine(
                "Email: " + Email + "\n" +
                // Password + "\n" +
                "DeviceId: " + DeviceId + "\n" +
                "CognitoId: " + DeviceSpecificQueue.CognitoId + "\n" +
                "DeviceId: " + DeviceSpecificQueue.DeviceId + "\n" +
                "DeviceQueueName: " + DeviceSpecificQueue.DeviceQueueName
            );

            List<string> Queues = DeviceQueues.List(DeviceSpecificQueue.DeviceQueueName);
            if (Queues.Count == 0)
            {
                DeviceQueues.Create(DeviceSpecificQueue.DeviceQueueName);
                SynapsedServerLibrary.Utilities.Debug.WriteLine("Queue created: " + DeviceSpecificQueue.DeviceQueueName);

                SynapsedServerLibrary.Queues.Model.ServerToDeviceQueueMessage QueueCreation = new SynapsedServerLibrary.Queues.Model.ServerToDeviceQueueMessage();
                QueueCreation.Type = "InformativeMessage";
                QueueCreation.Subtype = "QueueCreation";
                QueueCreation.Information = "Queue was created at " + SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz();

                DeviceQueues.MessageSend(DeviceSpecificQueue.DeviceQueueName, QueueCreation);


            }
            else
            {
                SynapsedServerLibrary.Utilities.Debug.WriteLine("Existing queues: " + Queues.Count);
                SynapsedServerLibrary.Queues.Model.ServerToDeviceQueueMessage QueueExistence = new SynapsedServerLibrary.Queues.Model.ServerToDeviceQueueMessage();
                QueueExistence.Type = "InformativeMessage";
                QueueExistence.Subtype = "QueueAlreadyExists";
                QueueExistence.Information = "Queue already created as of " + SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz();
                DeviceQueues.MessageSend(DeviceSpecificQueue.DeviceQueueName, QueueExistence);
            }
            Output = RetVal.ToString();
            return true;
        }

    }
}
