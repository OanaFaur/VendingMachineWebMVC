using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IProductsRepository
    {
        IEnumerable<Products> Products { get; }
        void AddProduct(Products db);
        Task Delete(int? id);
        Products find(int id);
    }
}
