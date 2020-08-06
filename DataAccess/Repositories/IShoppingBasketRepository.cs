using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IShoppingBasketRepository
    {
        List<ShoppingBasketItem> ShoppingBasketItem { get; set; }
        
    }
}
