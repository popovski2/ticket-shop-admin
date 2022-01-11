using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _181010_IS_Homework1_AdminApp.Models
{
    public class ShoppingCart
    {
        public Guid CartId { get; set; }
        public String ApplicationUserId { get; set; }
        public ICollection<TicketInShoppingCart> TicketsInShoppingCart { get; set; }
    }
}
