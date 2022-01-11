using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _181010_IS_Homework1_AdminApp.Models
{
    public class ShopApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        // public  ShoppingCart UserShoppingCart { get; set; }
    }
}
