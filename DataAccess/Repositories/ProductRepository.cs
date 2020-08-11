using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class ProductRepository : IProductsRepository
    {
        private DataContext context = new DataContext();

        public IEnumerable<Products> Products
        {
            get
            {
                return context.Products.ToList();
            }
        }

        public void AddProduct(Products db)
        {
            context.Add(db);
            context.SaveChanges();
        }

        public Products find(int id)
        {
            return Products.Where(p => p.Id == id).FirstOrDefault();
        }

        public async Task Delete(int? id)
        {
            var product = await context.Products.FindAsync(id);
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

    }
}
