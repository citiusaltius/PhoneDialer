using System;
using System.Collections.Generic;

namespace SynapsedServerLibrary.Tables.Defines
{
    public class DefinitionsCommunities : TableDefinitions
    {

        public new string   TableName = "Communities";
        public new string   Index = "CommunityId";
        public const string FieldNameCommunityName = "CommunityName";
        public const string FieldNameCommunityType = "CommunityType";
        public new bool IsHashAndRange = false;

        public enum CommunityTypes : int
        {
            Undefined,
            Personal,
            Public,
            Hospital
        };


        public static CommunityTypes GetCommunityTypeFromString(string TypeString)
        {
            List<string> TypeStrings = new List<string>(Enum.GetNames(typeof(CommunityTypes)));
            int EnumIndex = TypeStrings.IndexOf(TypeString);
            if (EnumIndex < 0)
            {
                throw new Utilities.Exceptions.ServerException("Invalid CommunityTypeType");
            }
            else
            {
                return (CommunityTypes)EnumIndex;
            }
        }
    }
}
