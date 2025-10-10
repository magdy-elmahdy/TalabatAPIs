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
        public static IQueryable<TEntity> getQuery(IQueryable<TEntity> inputQuery , ISpacifications<TEntity> Spec)
        {
            var Query = inputQuery;
            if(Spec.Criteria != null)
            {
                Query = Query.Where(Spec.Criteria);
            }
            Query = Spec.Includes.Aggregate(Query, (acc, Expression) => acc.Include(Expression));


            return Query; 
        }
    }
}
