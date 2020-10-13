using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.AdapterProduct
{
    public class ProductAdapter : IProductsRepository
    {
        public readonly ProductRepository _product;

        public IEnumerable<Products> Products
        {
            get
            {
                return _product.Products;
            }
        }

        public ProductAdapter(ProductRepository product)
        {
            this._product = product;
        }

        public void AddProduct(Products db)
        {
            _product.AddProduct(db);
        }

        public Task Delete(int? id)
        {
            return _product.Delete(id);
        }

        public Products find(int id)
        {
            return _product.find(id);
        }

        public void Update(Products p)
        {
            _product.Update(p);
        }
    }
}
