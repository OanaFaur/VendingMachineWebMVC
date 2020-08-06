using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IProductService
    {
        List<Products> GetProductList();
        Products find(int id);
        void AddProduct(Products db);
    }
}
