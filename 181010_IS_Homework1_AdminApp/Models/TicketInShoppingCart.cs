using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _181010_IS_Homework1_AdminApp.Models
{
    public class TicketInShoppingCart
    {
        public Guid TicketId { get; set; }
        public Guid CartId { get; set; }

        //[ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }

        //[ForeignKey("CartId")]
        public ShoppingCart ShoppingCart { get; set; }
        public int Quantity { get; set; }
    }
}
