using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _181010_IS_Homework1_AdminApp.Models
{
    public class Ticket
    {
        public Guid TicketId { get; set; }

        [Required]
        public String Title { get; set; }

        public String Image { get; set; }

        public int Rating { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public int Seat { get; set; }

        [Required]
        public DateTime DateAndTime { get; set; }

        public ICollection<TicketInShoppingCart> TicketsInShoppingCart { get; set; }
    }
}
