using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Models
{
    public class Products
    {
        public int Id { get; set; }

        public string Name { get; set; }
      
        public double Price { get; set; }

        [Display(Name = "Items Left")]
        public int ItemsLeft { get; set; }

        [Display(Name = "Items Sold")]
        public int ItemsSold { get; set; }

        public string Image { get; set; }

    }
}
