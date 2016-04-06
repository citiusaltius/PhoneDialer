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
    }
}
