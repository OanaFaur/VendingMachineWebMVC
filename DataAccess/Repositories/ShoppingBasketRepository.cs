using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    class ShoppingBasketRepository
    {
        public List<ShoppingBasketItem> ShoppingBasketItem { get; set; }

       
    }
}
