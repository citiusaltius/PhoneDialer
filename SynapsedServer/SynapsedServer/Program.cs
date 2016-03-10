using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServer
{
    class Program
    {
        static void Main(string[] args)
        {

            SynapsedServerLibrary.Utilities.Debug.SetNewSynchronizationContext();
            // Debug.WriteLine(SynapsedServerLibrary.Defines.Global.UtcNowDateTimeFormatAmz());
            NancyModules.MainWebApi MainApi = new NancyModules.MainWebApi();
            MainApi.Start();
            SynapsedServerLibrary.Utilities.Debug.WriteLine("Please hit enter to end");
            Console.ReadLine();
            MainApi.Stop();
        }
    }
}
