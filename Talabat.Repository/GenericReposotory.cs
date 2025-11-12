using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Reposotories.Centext;
using Talabat.Core.Spacifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericReposotory<T>:IGenericReposotory<T> where T : class
    {
        private readonly StoreContext _storeContext;

        public GenericReposotory(StoreContext StoreContext)
        {
            _storeContext = StoreContext;
        }

        public async Task<IReadOnlyList<T>> getAllAsync()
        {
           return await _storeContext.Set<T>().ToListAsync();
        }


        public async Task<T?> getAsync(int Id)
        {
            return await _storeContext.Set<T>().FindAsync(Id);
        }


        public async Task<IReadOnlyList<T>> getAllWithSpectAsync(ISpacifications<T> Specs)
        {
            return await SpacificationEvaluator<T>.getQuery(_storeContext.Set<T>(), Specs).AsNoTracking().ToListAsync();
        }  
        public async Task<T?> getWithSpectAsync(ISpacifications<T> Spect)
        {
            return await SpacificationEvaluator<T>.getQuery(_storeContext.Set<T>(), Spect).FirstOrDefaultAsync();

        }
    }
}
