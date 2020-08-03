using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VendingMachineWebMVC.DataContext;
using VendingMachineWebMVC.Models;


namespace VendingMachineWebMVC.Controllers
{
    public class ProductsController : Controller
    {
        
        public IProductService productService = new ProductService();
        

    // GET: Products
    public ActionResult Index()
    {
        ViewBag.products = productService.GetProductList();

        return View();
    }
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct()
        {
         

            return View();
        }
        
        [HttpPost]
        public IActionResult AddProduct(Products db)
    {
        productService.AddProduct(db);

        return View();
    }


    }

}
