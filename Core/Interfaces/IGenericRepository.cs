
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    // Can have GENERIC methods which which replace the T with specific Type at Compile time (reduces # repeating/ similar methods)
    public interface IGenericRepository<T> where T : BaseEntity // Constraint Generic repo using the 'where' and ':' to make it useable by classes DERIVED from Base entity
    {
        Task<T> GetProductByIdAsync(int id);

        Task<IReadOnlyList<T>> ListAllProductsAsync();

        Task<T> GetEntityWithSpecification(ISpecification<T> specification);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification);

        
    }
}