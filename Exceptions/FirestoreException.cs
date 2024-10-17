using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.Exceptions
{
    public class FirestoreException : CustomException
    {
        public FirestoreException(string message) : base(message)
        {
        }

        public FirestoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
