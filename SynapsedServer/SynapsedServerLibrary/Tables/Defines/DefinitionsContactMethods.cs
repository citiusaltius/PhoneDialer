using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.Tables.Defines
{
    public class DefinitionsContactMethods : TableDefinitions
    {
        public new string TableName = "ContactMethods";
        public new string Index = "ContactMethodId";
        public const string FieldNameContactMethodName = "ContactMethodName";
        public const string FieldNameContactInformation = "ContactInformation";
        public const string FieldNameContactMethodType = "ContactType";     // what kind of contact method is this?
        public const string FieldNameIdentityId = "IdentityId";       // what identity does this contact method belong to?
        public const string FieldNameIsVerified = "IsVerified";         // has this been verified?
        public const string FieldNameIsLoginable = "IsLoginable";       // can someone use this to log in?

        public DefinitionsContactMethods()
        {
            TableName = "ContactMethods";
            Index = "ContactMethodId";
        }

        public enum ContactMethodTypes : int
        {
            Undefined,      // not yet designated
            Email,          // 
            MobilePhone,    // 
            Pager,          //
            OfficePhone,    // 
            Fax
        };

        public static ContactMethodTypes GetContactMethodTypeFromString(string TypeString)
        {
            List<string> TypeStrings = new List<string>(Enum.GetNames(typeof(ContactMethodTypes)));
            int EnumIndex = TypeStrings.IndexOf(TypeString);
            if (EnumIndex < 0)
            {
                throw new Utilities.Exceptions.ServerException("Invalid ContactMethodType");
            }
            else
            {
                return (ContactMethodTypes)EnumIndex;
            }
        }

        public enum Visibilities : int
        {
            Undefined,
            Public,
            PublicApprovedOnly,
            Community,
            CommunityApprovedOnly,
            Private
        }

    }
}
