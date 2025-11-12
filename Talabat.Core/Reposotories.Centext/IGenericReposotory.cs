using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Spacifications;

namespace Talabat.Core.Reposotories.Centext
{
    public interface IGenericReposotory<T> where T : class
    {
        Task<T?> getAsync(int Id);
        Task<IReadOnlyList<T>> getAllAsync();

        Task<T?> getWithSpectAsync(ISpacifications<T> Spect);
        Task<IReadOnlyList<T>> getAllWithSpectAsync(ISpacifications<T> Spect);
        Task<int> GetCountAsync(ISpacifications<T> Spect);
    }
}
