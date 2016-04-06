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
        public const string FieldNameApplication = "ApplicationContactName";


        public enum ApplicationTypes : int
        {
            Undefined,
            SynappsedCoreApplication
        };
    }
}
