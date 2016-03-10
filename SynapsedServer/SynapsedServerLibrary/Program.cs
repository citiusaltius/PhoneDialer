using SynapsedServerLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary
{
    class Program
    {
        static void Main(string[] args)
        {

            Debug.SetNewSynchronizationContext();
            Debug.WriteLine(SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz());

        }
    }
}
