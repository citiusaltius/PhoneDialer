using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.Tables.Defines
{
    public class DefinitionsEntities : TableDefinitions
    {

        public new string TableName = "Entities";
        public new string Index = "EntityId";
        public const string FieldNameEntityName = "EntityName";
        public const string FieldNameEntityType = "EntityType";
        public enum EntityTypes : int
        {
            Undefined,
            Person
        };

        public static EntityTypes GetEntityTypeFromString(string EntityTypeString)
        {
            List<string> TypeStrings = new List<string>(Enum.GetNames(typeof(EntityTypes)));
            int EnumIndex = TypeStrings.IndexOf(EntityTypeString);
            if (EnumIndex < 0)
            {
                throw new Utilities.Exceptions.ServerException("Invalid EntityType");
            }
            else
            {
                return (EntityTypes)EnumIndex;
            }
        }

    }
}
