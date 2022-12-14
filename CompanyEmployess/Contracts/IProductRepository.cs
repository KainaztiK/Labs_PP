using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(Guid clientId, bool trackChanges);
        Task<Product> GetProductAsync(Guid clientId, Guid id, bool trackChanges);
        void CreateProductForClient(Guid clientId, Product product);
        void DeleteProduct(Product product);

    }
}
