using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.ViewModels
{
    public class AddProductViewModel
    {

        public string Name { get; set; }

        public double Price { get; set; }

        [Display(Name = "Items Left")]
        public int ItemsLeft { get; set; }

        [Display(Name = "Items Sold")]
        public int ItemsSold { get; set; }

        public IFormFile Image { get; set; }

    }
}
