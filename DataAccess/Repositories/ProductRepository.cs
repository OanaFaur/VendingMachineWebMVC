using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class ProductRepository : IProductsRepository
    {
        private readonly DataContext _dataContext;
        public ProductRepository()
        {

        }
        
        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<Products> Products
        {
            get
            {
                using (var data = new DataContext())
                {
                    return data.Products.ToList();
                }
            }
        }

        public void AddProduct(Products db)
        {
            using (var data = new DataContext())
            {
                data.Add(db);
                data.SaveChanges();
            }
        }
        
        public Products find(int id)
        {
            return Products.Where(p => p.Id == id).FirstOrDefault();
        }

        
    }
}
