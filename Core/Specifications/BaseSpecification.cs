using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification() {} // Empty Constructore to remove errors in type specific spec classes
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria; // Critera here set to whatever this expression being passed in is (we specify the critera when creating the specific ctor)
        }

        public Expression<Func<T, bool>> Criteria {get;}

        public List<Expression<Func<T, object>>> Includes {get;} =  
            new List<Expression<Func<T, object>>>(); // Set to empty list on default (Need this to instantiate it)

        public Expression<Func<T, object>> OrderBy {get; private set;} // private since we want to allow setting ONLY in this class

        public Expression<Func<T, object>> OrderByDescending {get; private set;}

        // Protected, can access method in this class and all child classes
        // These methods have to be EVALUATED by our specification evaluator -> So can be added to Queryable, which we can pass to our method which will call this
        protected void AddToInclude(Expression<Func<T, object>> includeExpression) // Method to ADD to our List Expression
        {
            Includes.Add(includeExpression);

        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

          protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
    }
}