using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.Tables.Defines
{
    public class DefinitionsIdentities : TableDefinitions
    {
        public new string TableName = "Identities";
        public new string Index = "IdentityId";
        public const string FieldNameIdentityName = "IdentityName";
        public const string FieldNameIdentityType = "IdentityType";     // what kind of identity is this?
        public const string FieldNameCommunityId = "CommunityId";       // what community does this identity belong to?
        public const string FieldNameIsVerified = "IsVerified";         // has this Identity been verified?
        public const string FieldNameIsLoginable = "IsLoginable";       // can someone use this identity (and associated contact methods) to log in?
        public const string FieldNameAssumingIdentites = "AssumingIdentities";  // which identities are assuming this identity? 


        public enum IdentityTypes : int
        {
            Undefined,  // not yet designated
            Person,     // a person, like Arun Iyer
            Position    // a position, like 
        };

        public static IdentityTypes GetIdentityTypeFromString(string IdentityTypeString)
        {
            List<string> TypeStrings = new List<string>(Enum.GetNames(typeof(IdentityTypes)));
            int EnumIndex = TypeStrings.IndexOf(IdentityTypeString);
            if (EnumIndex < 0)
            {
                throw new Utilities.Exceptions.ServerException("Invalid IdentityType");
            }
            else
            {
                return (IdentityTypes)EnumIndex;
            }
        }

    }
}
