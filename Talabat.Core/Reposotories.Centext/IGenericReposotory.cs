using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Reposotories.Centext
{
    public interface IGenericReposotory<T> where T : class
    {
        Task<T?> getAsync(int Id);
        Task<IEnumerable<T>> getAllAsync();
    }
}
