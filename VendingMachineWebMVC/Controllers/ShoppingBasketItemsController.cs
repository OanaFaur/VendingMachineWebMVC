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

namespace VendingMachineWebMVC.Controllers
{
    public class ShoppingBasketItemsController : Controller
    {
       
        public IProductsRepository repo = new ProductRepository();

        IProductService productservice = new BusinessLayer.Services.ProductService();

        public IActionResult Index()
        {
            var basket = SessionHelper.GetObjectFromJson<List<ShoppingBasketItem>>(HttpContext.Session, "basket");

            ViewBag.basket = basket;
            ViewBag.total = basket.Sum(item => item.Product.Price * item.Quantity);

            return View();
        }

        public IActionResult BuyProduct(int id)
        {

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
                SessionHelper.SetObjectAsJson(HttpContext.Session, "basket", basket);
            }

            else
            {
                List<ShoppingBasketItem> basket = SessionHelper.GetObjectFromJson<List<ShoppingBasketItem>>(HttpContext.Session, "basket");
                int index = isExist(id);
                if (index != -1)
                {
                    basket[index].Quantity++;

                }
                else
                {
                    basket.Add(new ShoppingBasketItem { Product = repo.find(id), Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "basket", basket);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingBasketItem> cart = SessionHelper.GetObjectFromJson<List<ShoppingBasketItem>>(HttpContext.Session, "basket");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "basket", cart);
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
