using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Spacifications;

namespace Talabat.Repository
{
    internal static class SpacificationEvaluator<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> getQuery(IQueryable<TEntity> inputQuery , ISpacifications<TEntity> Specs)
        {
            var Query = inputQuery;
            if(Specs.Criteria != null)
            {
                Query = Query.Where(Specs.Criteria);
            }
            if(Specs.OrderBy != null)
            {
                Query = Query.OrderBy(Specs.OrderBy);
            }
            if (Specs.OrderByDesc != null)
            {
                Query = Query.OrderByDescending(Specs.OrderByDesc);
            }
                
            Query = Specs.Includes.Aggregate(Query, (acc, Expression) => acc.Include(Expression));

            if (Specs.IsPaginationEnabled)
            {
                Query = Query.Skip(Specs.Skip).Take(Specs.Take);
            }


            return Query; 
        }
    }
}
