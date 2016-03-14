namespace SynapsedServerLibrary.Tables.Defines
{
    public class DefinitionsCommunityMemberships : TableDefinitions
    {
        public new string TableName = "CommunityMemberships";
        public new string Index = "CommunityMembershipId";
        public const string FieldNameCommunityId = "CommunityId";
        public const string FieldNameIdentityId = "IdentityId";

    }
}
