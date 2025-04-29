using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Oracle.ManagedDataAccess.Client;
using Template.Project.Model;

namespace Template.Project.Repository
{
    public class UserRepository
    {
        private readonly string _connectionString =
            "User Id=XXXXX;Password=XXXXX;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));";

        public User ValidateLogin(string username, string password)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                const string query = @"
                    SELECT id, username, fullname, password, email, signondate
                      FROM users
                     WHERE username = :u
                       AND password = :p";

                connection.Open();
                using (var cmd = new OracleCommand(query, connection))
                {
                    cmd.Parameters.Add("u", username);
                    cmd.Parameters.Add("p", password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Username = reader["username"].ToString(),
                                FullName = reader["fullname"].ToString(),
                                Password = reader["password"].ToString(),
                                Email = reader["email"].ToString(),
                                SignOnDate = Convert.ToDateTime(reader["signondate"])
                            };
                        }
                    }
                }
                return null;
            }
        }
    }
}
