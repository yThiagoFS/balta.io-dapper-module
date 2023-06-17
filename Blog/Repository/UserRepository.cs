using Blog.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Blog.Repository
{
    public class UserRepository : Repository<User>
    {
        private readonly SqlConnection _connection;

        public UserRepository(SqlConnection connection) : base(connection)
            => _connection = connection;

        public List<User> GetWithRoles()
        {
            var  query = @"SELECT 
                            u.*
                            ,R.*
                        FROM [User] U
                        LEFT JOIN [UserRole] UR on U.Id = UR.UserId
                        LEFT JOIN [Role] R on R.Id = UR.RoleId";

            var users = new List<User>();

            var items = _connection.Query<User, Role, User>(query,
                (user, role) =>
                {
                    var usr = users.FirstOrDefault(x => x.Id == user.Id);

                    if(usr == null)
                    {
                        usr = user;
                        if(role != null)
                            usr.Roles.Add(role);

                        users.Add(usr);
                    }
                    else
                        usr.Roles.Add(role);

                    return user;
                }, splitOn: "Id");

            foreach(var user in users)
            {
                Console.WriteLine($"{user.Name}");
                foreach(var role in user.Roles)
                {
                    Console.WriteLine($"{role.Name}");
                }
            }

            return users;
        }
    }
}