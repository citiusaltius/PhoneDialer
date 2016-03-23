using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServer.NancyModules
{
    public class IndexModule : Nancy.NancyModule
    {
        public IndexModule() : base("/")
        {
            Get["/", true] = async (parameters, token) =>
            {
                StringBuilder ReturnedPage = new StringBuilder();
                // spins for a little
                await Task.Run(() => { for (int i = 0; i < 1; i++) { } });
                ReturnedPage.AppendLine(@"
                <html>
                <body>
                <br /> <a href=""/"">index</a> <br /> 
                <h1> synapsed </h1>
                <br /> 

                Welcome to <i>synapsed</i>, your connection to the interconnections in medicine.
                <br/> <a href=""/register"">register</a> <br/>
                <br/> <a href=""/login"">login</a> <br/> 

                </body>
                </html>
                ");
                return ReturnedPage.ToString();
            };
        }
    }
}
