using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }
        //public IEnumerable<Product> GetProducts(bool trackChanges) =>
        //    FindAll(trackChanges).OrderBy(c => c.Name).ToList();
   

        public IEnumerable<Product> GetProducts(Guid clientId, bool trackChanges) =>
        FindByCondition(e => e.ClientId.Equals(clientId), trackChanges)
            .OrderBy(e => e.Name);
        public Product GetProduct(Guid clientId, Guid id, bool trackChanges) =>
            FindByCondition(e => e.ClientId.Equals(clientId) && e.Id.Equals(id) && e.Id.Equals(id),
            trackChanges).SingleOrDefault();
        public void CreateProductForClient(Guid clientId, Product product)
        {
            product.ClientId = clientId;
            Create(product);
        }
    }
}
