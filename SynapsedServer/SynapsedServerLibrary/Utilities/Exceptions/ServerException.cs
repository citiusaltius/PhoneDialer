using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.Utilities.Exceptions
{
    public class ServerException : Exception
    {
        public ServerException()
        {

        }

        public ServerException(string Message) :base (Message)
        {

        }

        public ServerException(string Message, Exception Inner) : base (Message, Inner)
        {

        }

    }
}
