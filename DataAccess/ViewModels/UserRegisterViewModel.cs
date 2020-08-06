using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.ViewModels
{
    public class UserRegisterViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string SecondName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Display(Name = "Secret code")]
        public string SecretCode { get; set; }
    }
}
