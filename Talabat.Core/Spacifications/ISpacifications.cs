using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Spacifications
{
    public interface ISpacifications<T> where T : class
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Expression<Func<T , Object>>> Includes { get; set; }
    }
}
