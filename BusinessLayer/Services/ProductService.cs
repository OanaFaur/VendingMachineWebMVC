using BusinessLayer.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Services
{
    public class ProductService : IProductService
    {
        private IProductsRepository repo = new ProductRepository();
        
        public List<Products> GetProductList()
        {
            return repo.Products.ToList();
        }

        public void AddProduct(Products db)
        {
            repo.AddProduct(db);
        }
        
    }
}
