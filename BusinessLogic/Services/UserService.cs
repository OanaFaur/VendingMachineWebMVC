using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        IUserRepository repo = new UserRepository();

        private DataContext _context;
        
        public UserService(DataContext context)
        {
            _context = context;
        }

        public UserService()
        {

        }
        
        public AdminUser GetById(int id)
        {
            return _context.AdminUser.Find(id);
        }
        
        public List<AdminUser> GetUserList()
        {
            return repo.Users.ToList();
        }

        public void Register(UserRegisterViewModel user)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);
            
            var newUser = new AdminUser
            {
                Username=user.UserName,
                Password = encoder.Encode(user.Password),
               // PasswordHash=passwordHash,
                //PasswordSalt=passwordSalt,
                FirstName = user.FirstName,
                SecondName = user.SecondName
            };

            repo.AddUser(newUser);
        }
        
         public bool Login(UserLoginViewModel user)
         {
            var founduser = repo.FindUser(user.Username);

            if (founduser == null)
            {
                return false;

            }
            if(!VerifyPasswordHash(user.Password, founduser.PasswordHash, founduser.PasswordSalt))
            {
                return false;
            }
            return true;
         }

        [NonAction]
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
