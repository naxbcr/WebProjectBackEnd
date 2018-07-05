using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project1.BLL.Model;
using Microsoft.AspNet.Identity;

namespace project1.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        User GetUserById(int id);
        User GetUserByEmail(string email);
        IEnumerable<User> GetAllUsers();
        int CreateUser(User newuser);
        bool UpdateUser(int id,User user);
        bool DeleteUser(int id);
        void ConfirmEmail(string email, bool confirm);

        Task <User> GetUserByEmailAndPassAsync(string email, string pass); //async fo auth
    }
}
