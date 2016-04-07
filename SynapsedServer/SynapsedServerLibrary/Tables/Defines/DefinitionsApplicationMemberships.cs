using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.Tables.Defines
{
    public class DefinitionsApplicationMemberships : TableDefinitions
    {
        public new string TableName = "ApplicationMemberships";
        public new string Index = "ApplicationMembershipId";
        public const string FieldNameEntityId = "EntityId";
        public const string FieldNameApplicationId = "ApplicationId";


        public enum ApplicationMembershipTypes : int
        {
            Undefined,
            Owner,
            Administrator,
            Member
        };

        public static ApplicationMembershipTypes GetApplicationMembershipTypeFromString(string TypeString)
        {
            List<string> TypeStrings = new List<string>(Enum.GetNames(typeof(ApplicationMembershipTypes)));
            int EnumIndex = TypeStrings.IndexOf(TypeString);
            if (EnumIndex < 0)
            {
                throw new Utilities.Exceptions.ServerException("Invalid ApplicationMembershipTypes");
            }
            else
            {
                return (ApplicationMembershipTypes)EnumIndex;
            }
        }
    }
}
