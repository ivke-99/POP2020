using SF_16_POP2020.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_16_POP2020.Windows.ViewModel
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public EGender Gender { get; set; }
        public ERole Role { get; set; }
        public Address Address { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
