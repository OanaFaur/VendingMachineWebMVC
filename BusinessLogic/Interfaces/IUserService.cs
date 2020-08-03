using DataAccess.Models;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        List<AdminUser> GetUserList();
        bool Login(UserLoginViewModel user);
        void Register(UserRegisterViewModel user);
    }
}
