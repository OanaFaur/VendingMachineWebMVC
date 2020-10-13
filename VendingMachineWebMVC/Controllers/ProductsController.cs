using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.AdapterProduct;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe;
using VendingMachineWebMVC.Models;


namespace VendingMachineWebMVC.Controllers
{
    public class ProductsController : Controller
    {
       // private IProductsRepository repo = new ProductRepository();
       // private IProductService productService = new BusinessLayer.Services.ProductService();
        private IHostingEnvironment HostingEnvironment;

        DataContext context = new DataContext();

        public ProductsController(IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }
   
    // GET: Products
        public ActionResult Index()
        {
            ProductRepository repo = new ProductRepository();
            IProductsRepository target = new ProductAdapter(repo);

            ViewBag.products = target.Products;

           return View();
        }

        [Authorize(Roles = "Admin")]
         public IActionResult AddProduct()
         {
            return View();
         }

        [HttpPost]
        public IActionResult AddProduct(AddProductViewModel db)
        {
            ProductRepository repo = new ProductRepository();
            IProductsRepository target = new ProductAdapter(repo);

            string stringFileName = UploadFile(db);
            var product = new Products()
            {
                Name = db.Name,
                Price = db.Price,
                ItemsLeft = db.ItemsLeft,
                ItemsSold = db.ItemsSold,
                Image = stringFileName
            };

            target.AddProduct(product);

            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            ProductRepository repo = new ProductRepository();
            IProductsRepository target = new ProductAdapter(repo);
            await  target.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        
        private string UploadFile(AddProductViewModel db)
        {
            string fileName = null;
            if (db.Image != null)
            {
                string uploadDir = Path.Combine(HostingEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + db.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    db.Image.CopyTo(fileStream);
                }
            }
            return fileName;
        }
    }

}
