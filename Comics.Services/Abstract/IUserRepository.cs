using System;
using System.Collections.Generic;
using System.Text;
using Comics.Domain;

namespace Comics.Services.Abstract
{
    public interface IUserRepository
    {
        public User GetUserDb(string id);
        public void AddUser(User user);
        public void DeleteUser(User user);
        public User GetUserInfo(string id);
    }
}
