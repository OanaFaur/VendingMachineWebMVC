using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IProductsRepository
    {
        IEnumerable<Products> Products { get; }
        //IEnumerable<Products> Product(string item);
        
        void AddProduct(Products db);
        //void DeleteProduct(Products db);
        //void UpdateProducts(Products db);
    }
}
