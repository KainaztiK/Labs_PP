using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entities.RequestFeatures;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }   

        public async Task<PagedList<Product>> GetProductsAsync(Guid clientId,
            ProductParameters productParameters, bool trackChanges)
        {
            var products = await FindByCondition(e => e.ClientId.Equals(clientId) &&
             (e.Price >= productParameters.MinPrice && e.Price <= productParameters.MaxPrice),
             trackChanges)
              .OrderBy(e => e.Name)
              .ToListAsync();
            return PagedList<Product>
             .ToPagedList(products, productParameters.PageNumber,
             productParameters.PageSize);
        }
        public async Task<Product> GetProductAsync(Guid clientId, Guid id, bool trackChanges) =>
            await FindByCondition(e => e.ClientId.Equals(clientId) &&
            e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
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
