using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetProductsAsync(Guid clientId, ProductParameters
            productParameters, bool trackChanges);
        Task<Product> GetProductAsync(Guid clientId, Guid id, bool trackChanges);
        void CreateProductForClient(Guid clientId, Product product);
        void DeleteProduct(Product product);

    }
}
