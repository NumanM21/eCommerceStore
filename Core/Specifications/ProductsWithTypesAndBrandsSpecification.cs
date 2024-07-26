using Core.Entities;

namespace Core.Specifications
{
    // Be SPECIFIC with class names for what exactly we want back -> Used in our product controller to query specific info using generic methods in repo
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        // Can use Ctor without parameters, for getting ALL products (ListAsync)
        public ProductsWithTypesAndBrandsSpecification(string productsToSort)
        {
            AddToInclude(x => x.ProductType);
            AddToInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name); // Default Ordering (so Alphabetical)
        }

        // Can use Ctor WITH parameters, to specify the product ID which we want to return (GetProduct) -> Both derive from BaseSpecification class

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddToInclude(x => x.ProductType);
            AddToInclude(x => x.ProductBrand);
        }

       
    }
}