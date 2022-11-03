using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<Product>> GetProductsAsync(Guid clientId, bool trackChanges) =>
        await FindByCondition(e => e.ClientId.Equals(clientId), trackChanges)
            .OrderBy(e => e.Name).ToListAsync();
        public async Task<Product> GetProductAsync(Guid clientId, Guid id, bool trackChanges) =>
            await FindByCondition(e => e.ClientId.Equals(clientId) && e.Id.Equals(id) && e.Id.Equals(id),
            trackChanges).SingleOrDefaultAsync();
        public void CreateProductForClient(Guid clientId, Product product)
        {
            product.ClientId = clientId;
            Create(product);
        }

        public void DeleteProduct(Product product)
        {
            Delete(product);
        }

        
    }
}
