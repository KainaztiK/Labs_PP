using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts(Guid clientId, bool trackChanges);
        Product GetProduct(Guid clientId, Guid id, bool trackChanges);
        void CreateProductForClient(Guid clientId, Product product);
        void DeleteProduct(Product product);

    }
}
