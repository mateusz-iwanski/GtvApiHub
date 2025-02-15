using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.Exceptions
{
    internal class ApiConnectionException : CustomException
    {
        public ApiConnectionException(string message) : base(message)
        {
        }

        public ApiConnectionException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
