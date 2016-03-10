using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServer.NancyModules
{
    public class MainWebApi
    {
        NancyHost Host;
        string uri = "http://localhost:8888";

        public MainWebApi()
        {
            Host = new NancyHost(new Uri(uri));
            Nancy.StaticConfiguration.DisableErrorTraces = false;
        }

        public void Start()
        {
            Console.WriteLine("Starting Nancy on " + uri);
            // initialize an instance of NancyHost
            Host.Start();  // start hosting
        }

        public void Stop()
        {
            Console.WriteLine("Stopping Nancy");
            Host.Stop();  // stop hosting
        }
    }
}
