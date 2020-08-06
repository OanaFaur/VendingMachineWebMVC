using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models
{
    public class ShoppingBasketItem
    {
        [Key]

        public int ShoppingBasketId { get; set; }

        public Products Product { get; set; }
        
        public int Quantity { get; set; }

       
    }
}
