using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<AdminUser> Users { get; }
        
        AdminUser GetById(int id);

        AdminUser FindUser(string username);

        void AddUser(AdminUser user);

        void UpdateUser(AdminUser user);
    }
}
