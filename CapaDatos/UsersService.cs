using EntityLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer
{
    public class UsersService
    {
        private NpgsqlDataSource dataSource;

        public UsersService(NpgsqlDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        public async Task<User> CreateUser(string username, string email, string password, string role)
        {
            NpgsqlCommand cmd = this.dataSource.CreateCommand($"SELECT * FROM create_user('{username}', '{email}', '{password}', '{role}')");
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (await reader.ReadAsync())
            {
                int userId = reader.GetInt32(0);
                string newUsername = reader.GetString(1);
                string newEmail = reader.GetString(2);
                string newPassword = reader.GetString(3);
                string newRole = reader.GetString(4);

                User user = new User(userId, newUsername, newEmail, newPassword, newRole);
                return user;
            }

            return null;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            NpgsqlCommand cmd = this.dataSource.CreateCommand($"SELECT * FROM find_user_by_email('{email}')");
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (await reader.ReadAsync())
            {
                int userId = reader.GetInt32(0);
                string newUsername = reader.GetString(1);
                string newEmail = reader.GetString(2);
                string newPassword = reader.GetString(3);
                string newRole = reader.GetString(4);

                User user = new User(userId, newUsername, newEmail, newPassword, newRole);
                return user;
            }

            return null;
        }
    }
}
