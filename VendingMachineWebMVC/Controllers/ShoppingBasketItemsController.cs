using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models;
using VendingMachineWebMVC.Helpers;
using DataAccess.Repositories;
using Stripe;
using BusinessLayer.Interfaces;
using BusinessLayer.AdapterProduct;

namespace VendingMachineWebMVC.Controllers
{
    public class ShoppingBasketItemsController : Controller
    {
       
       // private IProductsRepository repo = new ProductRepository();

        //private IProductService productservice = new BusinessLayer.Services.ProductService();

        public IActionResult Index()
        {
            var basket = SessionHelper.GetObjectFromJson<List<ShoppingBasketItem>>(HttpContext.Session, "basket");

            ViewBag.basket = basket;
            ViewBag.total = basket.Sum(item => item.Product.Price * item.Quantity);

            return View();
        }
        
        public IActionResult BuyProduct(int id)
        {
            ProductRepository repo = new ProductRepository();
            IProductsRepository target = new ProductAdapter(repo);

            var product = target.find(id);

            if (SessionHelper.GetObjectFromJson<List<ShoppingBasketItem>>(HttpContext.Session, "basket") == null)
            {

                List<ShoppingBasketItem> basket = new List<ShoppingBasketItem>
                {
                    new ShoppingBasketItem
                    {
                        Product = repo.find(id),
                        Quantity = 1
                    }
                };
                
                product.ItemsLeft--;
                product.ItemsSold++;
                
                SessionHelper.SetObjectAsJson(HttpContext.Session, "basket", basket);
            }

            else
            {
                List<ShoppingBasketItem> basket = SessionHelper.GetObjectFromJson<List<ShoppingBasketItem>>(HttpContext.Session, "basket");
                int index = isExist(id);
                if (index != -1)
                {
                    List<Products> products = target.Products.ToList();
                    basket[index].Quantity++;
                }
                else
                {
                    basket.Add(new ShoppingBasketItem { Product = repo.find(id), Quantity = 1 });
                }
                
                product.ItemsLeft--;
                product.ItemsSold++;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "basket", basket);

               
            }
            repo.Update(product);
            return RedirectToAction("Index");
        }
     

        public IActionResult Remove(int id)
        {
            ProductRepository repo = new ProductRepository();
            IProductsRepository target = new ProductAdapter(repo);
            List<ShoppingBasketItem> basket = SessionHelper.GetObjectFromJson<List<ShoppingBasketItem>>(HttpContext.Session, "basket");
            int index = isExist(id);
            var product = target.find(id);
            if (basket[index].Quantity > 1)
            {
                basket[index].Quantity--;
                product.ItemsLeft++;
                product.ItemsSold--;
            }
            else if (basket[index].Quantity == 1)
            {
                basket.RemoveAt(index);
                product.ItemsLeft++;
                product.ItemsSold--;

            }

            repo.Update(product);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "basket", basket);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<ShoppingBasketItem> basket = SessionHelper.GetObjectFromJson<List<ShoppingBasketItem>>(HttpContext.Session, "basket");
            for (int i = 0; i < basket.Count; i++)
            {
                if (basket[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        


        private double GetTotal()
        {

            List<ShoppingBasketItem> basket = SessionHelper.GetObjectFromJson<List<ShoppingBasketItem>>(HttpContext.Session, "basket");

            double total = basket.Sum(item => item.Product.Price * item.Quantity);

            return total;
        }

        private int GetQuantity()
        {
            List<ShoppingBasketItem> basket = SessionHelper.GetObjectFromJson<List<ShoppingBasketItem>>(HttpContext.Session, "basket");
           
            int quantity = basket.Sum(item => item.Quantity);
            

            return quantity;
        }

        public IActionResult Purchase()
        {

            ViewBag.total = GetTotal();

            return View();
        }
        [HttpPost]
        public IActionResult Create(string stripeToken, int Id)
        {
            List<ShoppingBasketItem> basket = SessionHelper.GetObjectFromJson<List<ShoppingBasketItem>>(HttpContext.Session, "basket");
            
            double d = GetTotal();

            var chargeOptions = new ChargeCreateOptions()
            {
                Amount = (long)d * 100,
                Currency = "ron",
                Source = stripeToken,
            };

            var service = new ChargeService();
            Charge charge = service.Create(chargeOptions);

            if (charge.Status == "succeeded")
            {
               
                return View("Success");
                
            }

            return View("Failure");

        }
    }
}
