using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.Tables.Defines
{
    public class DefinitionsApplications : TableDefinitions
    {
        public new string TableName = "Applications";
        public new string Index = "ApplicationId";
        public const string FieldNameApplicationName = "ApplicationName";
        public const string FieldNameApplicationContactName = "ApplicationContactName";
        public const string FieldNameApplicationContactInformation = "ApplicationContactInformation";
        public const string FieldNameApplicationDescription = "ApplicationDescription";
        
        


        public enum ApplicationTypes : int
        {
            Undefined,
            SynappsedCoreApplication
        };

        public static ApplicationTypes GetApplicationTypeFromString(string TypeString)
        {
            List<string> TypeStrings = new List<string>(Enum.GetNames(typeof(ApplicationTypes)));
            int EnumIndex = TypeStrings.IndexOf(TypeString);
            if (EnumIndex < 0)
            {
                throw new Utilities.Exceptions.ServerException("Invalid ApplicationType");
            }
            else
            {
                return (ApplicationTypes)EnumIndex;
            }
        }
    }
}
