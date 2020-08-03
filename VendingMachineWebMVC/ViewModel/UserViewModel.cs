using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachineWebMVC.ViewModel
{
    public class UserViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string SecondName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
