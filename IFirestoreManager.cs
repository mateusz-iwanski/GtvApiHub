using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GtvApiHub
{
    public interface IFirestoreManager
    {
        Task SyncFirestoreAsync(bool continueOnError, bool skipApiNullData = true);
    }
}
