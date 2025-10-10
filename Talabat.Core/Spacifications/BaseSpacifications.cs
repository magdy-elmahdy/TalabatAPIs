using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Spacifications
{
    public class BaseSpacifications<T> : ISpacifications<T> where T : class
    {
        public Expression<Func<T, bool>>? Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

        public BaseSpacifications(){}
        public BaseSpacifications(Expression<Func<T, bool>> Spect)
        {
            Criteria =  Spect;
        }
    }
}
