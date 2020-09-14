using BusinessLayer.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.ViewModels;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserService:IUserService
    {
        IUserRepository repo = new UserRepository();

        public void Register(UserRegisterViewModel user)
        {
            ScryptEncoder encoder = new ScryptEncoder();
           
            var newUser = new AdminUser
            {
                Username = user.UserName,
                Password = encoder.Encode(user.Password),
                FirstName = user.FirstName,
                SecondName = user.SecondName
            };
            repo.AddUser(newUser);
        }
        
    }
}
