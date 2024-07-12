using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        // Generic Methods -> Func takes a type (T), and we specify the type it will return (bool)
        Expression<Func<T, bool>> Criteria {get;} // Similar to 'Where' in LINQ
        List<Expression<Func<T, object>>> Includes {get;} // Similar to 'Include' in LINQ
        
    }
}