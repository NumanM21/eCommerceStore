using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity // Also have to MATCH constrains in child class
    {
        private readonly StoreContext _context;
        public GenericRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<T> GetEntityWithSpecification(ISpecification<T> specification)
        { 
            // Method returns IQueryable -> so can pass this to a method which will execute our Query against the DB
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<T> GetProductByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id); // .Set<> will set the type of whatever we want to get ('T' replaced with type)
        }

        public async Task<IReadOnlyList<T>> ListAllProductsAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            // T replaced with our Type (I.e. Product) since we are in our gen repo
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);

        }

    }
}