using System;
using System.Collections.Generic;

namespace SynapsedServerLibrary.Tables.Defines
{
    public class DefinitionsSecurityInformations : TableDefinitions
    {
        public new string TableName = "SecurityInformations";
        public new string Index = "SecurityInformationId";
        public const string FieldNameSecurityInformationsType = "SecurityInformationType";
        public const string FieldNameInformation = "Information";
        public const string FieldNameEntityId = "EntityId";
        public const string FieldNameNumberOfUses = "NumberOfUses";
        public const string FieldNameMaxNumberOfUses = "MaximumNumberOfUses";

        public enum SecurityInformationTypes : int
        {
            Undefined,
            HashedPassword
        };


        public static SecurityInformationTypes GetSecurityInformationsTypeFromString(string TypeString)
        {
            List<string> TypeStrings = new List<string>(Enum.GetNames(typeof(SecurityInformationTypes)));
            int EnumIndex = TypeStrings.IndexOf(TypeString);
            if (EnumIndex < 0)
            {
                throw new Utilities.Exceptions.ServerException("Invalid SecurityInformationsType");
            }
            else
            {
                return (SecurityInformationTypes)EnumIndex;
            }
        }

    }
}
