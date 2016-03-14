using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.Tables.Defines
{
    public class TableDefinitions : Base.BaseObject
    {
        public string   TableName = null;
        public string   Index = null;
        public bool     IsHashAndRange = false;
        public string   Range = null;
        public const string FieldNameDataVersion = "DataVersion";
        public const string FieldNameTimestampCreation = "TimestampCreation";
        public const string FieldNameTimestampModified = "TimestampModified";
        public const string FieldNameTimestampExpiration = "TimestampExpiration";
        public const string FieldNameModifiedByEntityId = "ModifiedByEntityId";

        public enum ModifiedByEstablishedEntityIds
        {
            Undefined,
            ServerDaemon,
            WebServer
        }

        public override string ToString()
        {
            return TableName + " " + base.ToString();
        }

        public int GetEnumFromString(string EnumString, Type EnumType)
        {
            List<string> TypeStrings = new List<string>(Enum.GetNames(EnumType));
            int EnumIndex = TypeStrings.IndexOf(EnumString);
            if (EnumIndex < 0)
            {
                throw new Utilities.Exceptions.ServerException("Invalid ConversationType");
            }
            else
            {
                return (int)EnumIndex;
            }
        }
    }
}
