using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext context = new DataContext();

        public IEnumerable<AdminUser> Users
        {
            get
            {
                return context.AdminUser.ToList();
            }
        }

        public AdminUser GetById(int id)
        {
            return context.AdminUser.Find(id);

        }
        public void AddUser(AdminUser user)
        {
            context.Add(user);
            context.SaveChanges();
        }


        public AdminUser FindUser(string username)
        {
            return context.AdminUser.FirstOrDefault(x => x.Username == username);
        }
    }
}
