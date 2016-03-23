using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SynapsedServerLibrary.Tables.Accessors;
using SynapsedServerLibrary.Tables.Model;

namespace SynapsedServer.NancyModules
{
    public class RegisterModule : Nancy.NancyModule
    {
        public RegisterModule() : base("/register")
        {
            Entities EntityClient = new Entities();
            Identities IdentitiesClient = new Identities();
            ContactMethods ContactMethodsClient = new ContactMethods();
            SecurityInformations SecurityInformationClient = new SecurityInformations();

            Get["/", true] = async (parameters, token) =>
            {
                StringBuilder ReturnedPage = new StringBuilder();
                ReturnedPage.AppendLine(@"
                <html>
                <body>
                <br /> <a href='/'>index</a> <br /> 
                <h1> registration </h1>
                <br /> 
                <form id='register' action='/register/submit' method='post' accept-charset='UTF-8'>
                <fieldset > 
                <legend>Register</legend> 
                <input type='hidden' name='submitted' id='submitted' value='1'/> 
                <table>");
                ReturnedPage.AppendLine(@"
                <tr>
                <td><label for='community' >community *: </label></td>
                <td><select name='community' id='community'> 
                ");
                List<Community> AllCommunities = new List<Community>();
                Communities CommunitiesClient = new Communities();
                await Task.Run(() => AllCommunities = CommunitiesClient.GetAll());

                foreach (Community ThisCommunity in AllCommunities)
                {
                    ReturnedPage.AppendLine("<option value=\"" + ThisCommunity.CommunityId + "\">" + ThisCommunity.CommunityName + "</option>");

                    foreach (string Domain in ThisCommunity.AcceptableWebDomains)
                    {
                        ReturnedPage.AppendLine("<option value=\"" + ThisCommunity.CommunityId + "\" disabled>" + "@" + Domain + "</option>");
                    }
                }

                ReturnedPage.AppendLine(@"
                </select></td>
                </tr>
                ");

                ReturnedPage.AppendLine(@" 
                <tr>
                <td><label for='firstname' >first name*: </label></td>
                <td><input type='text' name='firstname' id='firstname' maxlength='50' /> </td>
                </tr>
                <tr>
                <td><label for= 'lastname' > last name *: </label></td>
                <td><input type = 'text' name = 'lastname' id = 'lastname' maxlength = '50' /></td>
                </tr>
                <tr>
                <td><label for= 'telephone' > mobile phone number *: </label></td>
                <td><input type = 'text' name = 'telephone' id = 'telephone' maxlength = '10' /></td>
                </tr>
                <tr>
                <td><label for= 'email' > e-mail address *:</label></td>
                <td><input type = 'text' name = 'email' id = 'email' maxlength = '50' /></td>
                </tr>
                <tr>
                <td><label for= 'password' > password *:</label ></td>
                <td><input type = 'password' name = 'password' id = 'password' maxlength = '50' /></td>
                </tr>
                <tr>
                <td><label for= 'passwordConfirm' > confirm password *:</label ></td>
                <td><input type = 'password' name = 'passwordConfirm' id = 'passwordConfirm' maxlength = '50' /></td>
                </tr>
                </table>
                <input type = 'submit' name = 'Submit' value = 'Submit' />
                <br />
                </fieldset >
                </form >
                </body>
                </html>
                ");
                return ReturnedPage.ToString();
            };

            Post["/", true] = async (parameters, token) =>
            {
                StringBuilder ReturnedPage = new StringBuilder();
                ReturnedPage.AppendLine("<h1>Post base</h1>");
                // spins for a little
                await Task.Run(() => { for (int i = 0; i < 1; i++) { } });
                ReturnedPage.AppendLine(new System.IO.StreamReader(this.Request.Body).ReadToEnd());
                return ReturnedPage.ToString();
            };

            Post["/submit", true] = async (parameters, token) =>
            {
                StringBuilder ReturnedPage = new StringBuilder();
                ReturnedPage.AppendLine(@"
                <html>
                <body>
                <br /> <a href='/'>index</a> <br /> 
                <h1> registration </h1>
                <br /> 
                ");
                foreach (string Key in this.Request.Headers.Keys)
                {
                    ReturnedPage.AppendLine("<br /> Header: " + Key + " : " + this.Request.Headers[Key] + "<br/>");
                }

                ReturnedPage.AppendLine(@"<br /> <br />");
                ReturnedPage.AppendLine(new System.IO.StreamReader(this.Request.Body).ReadToEnd());
                ReturnedPage.AppendLine("<br/>submitted:\t" + this.Request.Form.submitted);
                ReturnedPage.AppendLine("<br/>community id:\t" + this.Request.Form.community);
                ReturnedPage.AppendLine("<br/>first name:\t" + this.Request.Form.firstname);
                ReturnedPage.AppendLine("<br/>last name:\t" + this.Request.Form.lastname);
                ReturnedPage.AppendLine("<br/>telephone:\t" + this.Request.Form.telephone);
                ReturnedPage.AppendLine("<br/>email:\t" + this.Request.Form.email);
                ReturnedPage.AppendLine("<br/>password:\t" + this.Request.Form.password);
                ReturnedPage.AppendLine("<br/>password:\t" + this.Request.Form.passwordConfirm);

                //sanity checks
                {
                    if (this.Request.Form.password.ToString().Equals(this.Request.Form.passwordConfirm.ToString()) == false)
                    {
                        StringBuilder IncorrectResponse = new StringBuilder();

                        //SynapsedServerLibrary.Utilities.Debug.WriteLine("Passwords do not match");
                        //Nancy.Responses.RedirectResponse resp = this.Context.GetRedirect("/register");

                        //// resp.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                        //resp.StatusCode = Nancy.HttpStatusCode.BadRequest;
                        //resp.ContentType = "application/x-www-form-urlencoded";
                        //return resp;

                        IncorrectResponse.AppendLine(@"<br /> Incorrect Response<br />");
                        IncorrectResponse.AppendLine(new System.IO.StreamReader(this.Request.Body).ReadToEnd());
                        IncorrectResponse.AppendLine("<br/>submitted:\t" + this.Request.Form.submitted);
                        IncorrectResponse.AppendLine("<br/>community id:\t" + this.Request.Form.community);
                        IncorrectResponse.AppendLine("<br/>first name:\t" + this.Request.Form.firstname);
                        IncorrectResponse.AppendLine("<br/>last name:\t" + this.Request.Form.lastname);
                        IncorrectResponse.AppendLine("<br/>telephone:\t" + this.Request.Form.telephone);
                        IncorrectResponse.AppendLine("<br/>email:\t" + this.Request.Form.email);
                        IncorrectResponse.AppendLine("<br/>password:\t" + this.Request.Form.password);
                        IncorrectResponse.AppendLine("<br/>password confirm:\t" + this.Request.Form.passwordConfirm);
                        IncorrectResponse.AppendLine(@"
                            <html>
                            <body>
                            <br /> <a href='/'>index</a> <br /> 
                            <h1> registration </h1>
                            <br /> 
                            <form id='register' action='/register/submit' method='post' accept-charset='UTF-8'>
                            <fieldset > 
                            <legend>Register</legend> 
                            <input type='hidden' name='submitted' id='submitted' value='1'/> 
                            <table>");
                        IncorrectResponse.AppendLine(@"
                            <tr> 
                            <td span=2>" + "Invalid submission. Please try again!" + @"</td>
                            </tr>
                         ");

                        IncorrectResponse.AppendLine(@"
                            <tr>
                            <td><label for='community' >community *: </label></td>
                            <td><select name='community' id='community'> 
                        ");
                        List<Community> AllCommunities = new List<Community>();
                        Communities CommunitiesClient = new Communities();
                        await Task.Run(() => AllCommunities = CommunitiesClient.GetAll());

                        foreach (Community ThisCommunity in AllCommunities)
                        {
                            IncorrectResponse.Append("<option value=\"" + ThisCommunity.CommunityId + "\"");
                            if (this.Request.Form.community == ThisCommunity.CommunityId.ToString())
                            {
                                IncorrectResponse.Append(" selected ");
                            }
                            IncorrectResponse.AppendLine(">" + ThisCommunity.CommunityName + "</option>");

                            foreach (string Domain in ThisCommunity.AcceptableWebDomains)
                            {
                                IncorrectResponse.AppendLine("<option value=\"" + ThisCommunity.CommunityId + "\" disabled>" + "@" + Domain + "</option>");
                            }
                        }

                        IncorrectResponse.AppendLine(@"
                            </select></td>
                            </tr>
                        ");

                        IncorrectResponse.AppendLine(@" 
                            <tr>
                            <td><label for='firstname' >first name*: </label></td>
                            <td><input type='text' name='firstname' id='firstname' maxlength='50' value='" + this.Request.Form.firstname + @"'/></td>
                            </tr>
                            <tr>
                            <td><label for= 'lastname' > last name *: </label></td>
                            <td><input type = 'text' name = 'lastname' id = 'lastname' maxlength = '50' /></td>
                            </tr>
                            <tr>
                            <td><label for= 'telephone' > mobile phone number *: </label></td>
                            <td><input type = 'text' name = 'telephone' id = 'telephone' maxlength = '10' /></td>
                            </tr>
                            <tr>
                            <td><label for= 'email' > e-mail address *:</label></td>
                            <td><input type = 'text' name = 'email' id = 'email' maxlength = '50' /></td>
                            </tr>
                            <tr>
                            <td><label for= 'password' > password *:</label ></td>
                            <td><input type = 'password' name = 'password' id = 'password' maxlength = '50' /></td>
                            </tr>
                            <tr>
                            <td><label for= 'passwordConfirm' > confirm password *:</label ></td>
                            <td><input type = 'password' name = 'passwordConfirm' id = 'passwordConfirm' maxlength = '50' /></td>
                            </tr>
                            </table>
                            <input type = 'submit' name = 'Submit' value = 'Submit' />
                            <br />
                            </fieldset >
                            </form >
                            </body>
                            </html>
                        ");
                        return IncorrectResponse.ToString();
                    }
                }



                List<ContactMethod> RetrievedContactMethods = new List<ContactMethod>();

                await Task.Run(() => RetrievedContactMethods = ContactMethodsClient.FindByInformation(this.Request.Form.email));

                //ReturnedPage.AppendLine("<br/>" + RetrievedContactMethods.Count);
                //foreach (ContactMethod RetrievedContactMethod in RetrievedContactMethods)
                //{
                //    ReturnedPage.AppendLine("<br/>retrieved contact method entry<br/>");
                //    ReturnedPage.AppendLine(RetrievedContactMethod.PrintProperties(true));
                //}
                // If nothing found, end here
                if (RetrievedContactMethods.Count > 0)
                {
                    ReturnedPage.AppendLine("<br/>Email already exists! Rejected. <br/>");

                    Identity RetrievedIdentity = new Identity();

                    await Task.Run(() =>
                            RetrievedIdentity = IdentitiesClient.Get(RetrievedContactMethods[0].IdentityId)
                            );
                    //ReturnedPage.AppendLine(RetrievedIdentity.PrintProperties(true));
                    //ReturnedPage.AppendLine(@"
                    //</body >
                    //</html >
                    //");
                    return ReturnedPage.ToString();
                }
                ReturnedPage.AppendLine("<br/>Email does not exist! Building... <br/>");



                Entity NewEntity = new Entity();
                NewEntity.EntityFirstName = (this.Request.Form.firstname);
                NewEntity.EntityLastName = (this.Request.Form.lastname);
                NewEntity = EntityClient.Create(NewEntity);
                string CognitoId = "";
                string OpenIdToken = "";
                SynapsedServerLibrary.AuthenticationAndIdentity.Model.CognitoIdentity CognitoIdentityClient = new SynapsedServerLibrary.AuthenticationAndIdentity.Model.CognitoIdentity();
                CognitoIdentityClient.RetrieveCognitoIdentityAndToken(
                    // "entity:" + NewEntity.EntityId
                    CognitoIdentityClient.GetCognitoIdentifierForEntityId(NewEntity.EntityId)
                    , out CognitoId, out OpenIdToken);
                NewEntity.EntityCognitoId = CognitoId;
                NewEntity.EntitySalt = SynapsedServerLibrary.Security.UniqueKeyGenerator.GetUniqueKey(512);
                EntityClient.Update(NewEntity);

                ReturnedPage.AppendLine("<br/>Updated Entity<br/>");
                //ReturnedPage.AppendLine(NewEntity.PrintProperties(true));


                Identity NewIdentity = new Identity();
                NewIdentity.CommunityId = (this.Request.Form.community);
                NewIdentity.IdentityType = (SynapsedServerLibrary.Tables.Defines.DefinitionsIdentities.IdentityTypes.Person);
                NewIdentity.EntityId = NewEntity.EntityId;
                NewIdentity.IdentityName = (this.Request.Form.firstname) + " " + (this.Request.Form.lastname)
                + " (" + (this.Request.Form.email) + ")";
                NewIdentity = IdentitiesClient.Create(NewIdentity);

                //ReturnedPage.AppendLine(NewIdentity.PrintProperties(true));

                ContactMethod NewContactMethod = new ContactMethod();
                NewContactMethod.ContactInformation = (this.Request.Form.email);
                NewContactMethod.ContactMethodType = (SynapsedServerLibrary.Tables.Defines.DefinitionsContactMethods.ContactMethodTypes.Email);
                NewContactMethod.IdentityId = (NewIdentity.IdentityId);
                NewContactMethod.IsLoginable = (true);
                NewContactMethod = ContactMethodsClient.Create(NewContactMethod);

                //ReturnedPage.AppendLine(NewContactMethod.PrintProperties(true));

                SecurityInfo NewSecurityInfo = new SecurityInfo();


                SynapsedServerLibrary.Security.Hashing Hasher = new SynapsedServerLibrary.Security.Hashing();

                NewSecurityInfo.SecurityInformation = Hasher.PasswordHash(NewEntity.EntitySalt, this.Request.Form.password);
                NewSecurityInfo.EntityId = (NewEntity.EntityId);
                NewSecurityInfo.SecurityInformationType = SynapsedServerLibrary.Tables.Defines.DefinitionsSecurityInformations.SecurityInformationTypes.HashedPassword;
                NewSecurityInfo.MaxNumberOfUses = Int32.MaxValue;
                NewSecurityInfo.TimestampExpiration = SynapsedServerLibrary.Defines.Global.UtcNowDateTimePlusTimeFormatAmz(SynapsedServerLibrary.Defines.Global.ExpirationTimespanDefault);
                NewSecurityInfo = SecurityInformationClient.Create(NewSecurityInfo);

                //ReturnedPage.AppendLine(NewSecurityInfo.PrintProperties(true));

                ReturnedPage.AppendLine("<br/><h1>Registration Successful!</h1>");

                ReturnedPage.AppendLine(@"
                    </body >
                    </html >
                    ");
                return ReturnedPage.ToString();
            };

            // end of function
        }
    }
}
