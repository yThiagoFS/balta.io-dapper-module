using Blog.Models;
using Blog.Models.Entities;
using Blog.Repository;
using Microsoft.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Blog
{
    class Program 
    {
        private const string CONNECTION_STRING = "Data Source=DESKTOP-DIFT32I\\SQLEXPRESS;Initial Catalog=Blog;Integrated Security=True; TrustServerCertificate=True";

        private 
        static void Main(string[] args)
        {
            var connection = new SqlConnection(CONNECTION_STRING);

            connection.Open();

            var users = GetUsers(connection);

            Console.WriteLine("Users: ");
            foreach(var user in users)
                Console.WriteLine($"{user.Id} - {user.Name}");

            var roles = GetRoles(connection);

            Console.WriteLine("Roles: ");
            foreach(var role in roles)
                Console.WriteLine($"{role.Id} - {role.Name}");

            connection.Close();
        }

        static IEnumerable<User> GetUsers(SqlConnection connection) => new UserRepository(connection).GetWithRoles();

        static IEnumerable<Role> GetRoles(SqlConnection connection) => new Repository<Role>(connection).Get();

        static void CreateUser(SqlConnection connection)  
            => new Repository<User>(connection).Create(new User
        {
            Email = "email@abc.com",
            Bio = "Bio",
            Image = "imagem",
            Name = "name",
            PasswordHash = "hash",
            Slug = "Slug"
        });
    }
}




