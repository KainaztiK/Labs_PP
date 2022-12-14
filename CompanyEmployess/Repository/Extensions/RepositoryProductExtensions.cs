using Entities.Models;
using Repository.Extensions.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class RepositoryProductExtensions
    {
        public static IQueryable<Product> FilterProducts(this IQueryable<Product>
        products, uint minPrice, uint maxPrice) =>
        products.Where(e => (e.Price >= minPrice && e.Price <= maxPrice));
        public static IQueryable<Product> Search(this IQueryable<Product>
       products,
        string searchPrice)
        {
            if (string.IsNullOrWhiteSpace(searchPrice))
                return products;
            var lowerCaseTerm = searchPrice.Trim().ToLower();
            return products.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Product> Sort(this IQueryable<Product> products,
        string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return products.OrderBy(e => e.Name);

            var orderQuery =
           OrderQueryBuilder.CreateOrderQuery<Product>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return products.OrderBy(e => e.Name);
            return products.OrderBy(orderQuery);
        }
    }
}
