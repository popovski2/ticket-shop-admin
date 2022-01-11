using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _181010_IS_Homework1_AdminApp.Models
{
    public class User
    {
        public String Email { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Address { get; set; }
        public String Password { get; set; }
        public String ConfirmPassword { get; set; }
    }
}
