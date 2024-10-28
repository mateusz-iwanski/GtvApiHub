using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub
{
    public interface IFirestoreBaseService
    {
        Task<IEnumerable<T>> GetAll<T>();
    }
}
