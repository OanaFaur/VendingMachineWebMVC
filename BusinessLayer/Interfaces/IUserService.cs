using DataAccess.Models;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        List<AdminUser> GetUserList();
        void Register(UserRegisterViewModel user);
    }
}
