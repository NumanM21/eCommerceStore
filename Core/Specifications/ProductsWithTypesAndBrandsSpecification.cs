using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    // Be SPECIFIC with class names for what exactly we want back -> Used in our product controller to query specific info using generic methods in repo
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        // Can use Ctor without parameters, for getting ALL products (ListAsync)
        public ProductsWithTypesAndBrandsSpecification()
        {
            AddToInclude(x => x.ProductType);
            AddToInclude(x => x.ProductBrand);
        }

        // Can use Ctor WITH parameters, to specify the product ID which we want to return (GetProduct) -> Both derive from BaseSpecification class

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddToInclude(x => x.ProductType);
            AddToInclude(x => x.ProductBrand);
        }

       
    }
}