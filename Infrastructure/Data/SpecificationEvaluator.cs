using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity // Constraining only with use with our actual entity classes
    {
        // Static -> Method we can use without creating an instance of the class its in
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var query = inputQuery;

            if (specification.Criteria != null) // Want to evaluate our query
            {   
                query = query.Where(specification.Criteria);
            }

              if (specification.OrderBy != null)
            {   
                query = query.OrderBy(specification.Criteria); // This OrderBy is the LINQ one, we are re-creating this so match what we want specifically!
            }

              if (specification.OrderByDescending != null) // Want to evaluate our query
            {   
                query = query.OrderByDescending(specification.Criteria);
            }

            // Now want to evaluate our Inlcudes method | Aggregate -> Similar to accumilating all our .Include method in LINQ
            // CurrentEntity -> Entity we are passing in here. include -> Expression of our includes statement  
            query = specification.Includes.Aggregate(query, (currentEntity, include) => currentEntity.Include(include));

            // Will pass this query (IQueryable) to a method, which will Query our DB and return a result (similar to what LINQ does, but manually making it)
            return query;

        } 


    }
}