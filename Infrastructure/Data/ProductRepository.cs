
using System.Net.Http.Headers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context) // Inject our DbContext here -> Controller access Repo which can then access the Db (Abstraction)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
            .Include(pt => pt.ProductType)
            .Include(pb => pb.ProductBrand)
            .FirstOrDefaultAsync(x => x.Id == id); // gets the first match where our Id in our DB is equal to id being passed in parameter
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products
            // Can use LINQ to eager load additional information PER query we are going to make!
            .Include(pt => pt.ProductType)
            .Include(pb => pb.ProductBrand)
            .ToListAsync(); // Point which our Query is sent to SQL and we receive data back
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}