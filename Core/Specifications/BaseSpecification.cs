using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria {get;}

        public List<Expression<Func<T, object>>> Includes {get;} =  
            new List<Expression<Func<T, object>>>(); // Set to empty list on default (Need this to instantiate it)

        // Protected, can access method in this class and all child classes
        protected void AddToInclude(Expression<Func<T, object>> includeExpression) // Method to ADD to our List Expression
        {
            Includes.Add(includeExpression);

        }
    }
}