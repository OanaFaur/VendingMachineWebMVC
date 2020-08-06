using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public IEnumerable<AdminUser> Users
        {
            get
            {
                using(var data = new DataContext()) {
                    return data.AdminUser.ToList();
                }
            }
        }
        
        public AdminUser GetById(int id)
        {
            using(var data = new DataContext())
            {
                return data.AdminUser.Find(id);
            }
        }
        public void AddUser(AdminUser user)
        {

            using (var data = new DataContext())
            {
                data.Add(user);
                data.SaveChanges();
            }
        }

        public void UpdateUser(AdminUser user)
        {
            using (DataContext data = new DataContext())
            {
                data.Update(user);
                data.SaveChanges();
            }
        }

        public AdminUser FindUser(string username)
        {
           using(var data = new DataContext())
            {
                return data.AdminUser.FirstOrDefault(x => x.Username == username);
            }
        }
    }
}
