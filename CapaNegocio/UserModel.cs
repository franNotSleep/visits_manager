using DataLayer;
using EntityLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class UserModel
    {
        UsersService usersService;

        public UserModel(NpgsqlDataSource dataSource)
        {
            this.usersService = new UsersService(dataSource);
        }
        public async Task<User> Login(string email, string password)
        {
            User user = await usersService.GetUserByEmail(email);

            if (user != null && user.Password == password)
            {
                return user;
            }
            return null;
        }
        public async Task<User> CreateUser(string username, string email, string password, string role)
        {
            return await usersService.CreateUser(username, email, password, role);
        }
    }
}
