using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _181010_IS_Homework1_AdminApp.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ShopApplicationUser OrderedBy { get; set; }
        public IEnumerable<TicketInOrder> Tickets { get; set; }
    }
}
