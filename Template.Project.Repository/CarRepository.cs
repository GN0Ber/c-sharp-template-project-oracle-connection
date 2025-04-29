using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Template.Project.Model;

namespace Template.Project.Repository
{
    public class CarRepository
    {
        private readonly string _connectionString =
            "User Id=XXXXX;Password=XXXXX;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));";

        public void Add(Car car)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                const string query = @"
                    INSERT INTO cars (id, brand, model, year, price, created_by_user_id)
                    VALUES (cars_seq.NEXTVAL, :b, :m, :y, :p, :u)";

                connection.Open();
                using (var cmd = new OracleCommand(query, connection))
                {
                    cmd.Parameters.Add("b", car.Brand);
                    cmd.Parameters.Add("m", car.Model);
                    cmd.Parameters.Add("y", car.Year);
                    cmd.Parameters.Add("p", car.Price);
                    cmd.Parameters.Add("u", car.CreatedByUserId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Car> GetAll()
        {
            var cars = new List<Car>();
            using (var connection = new OracleConnection(_connectionString))
            {
                const string query = @"
                    SELECT id, brand, model, year, price, created_by_user_id
                      FROM cars
                  ORDER BY id";

                connection.Open();
                using (var cmd = new OracleCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cars.Add(new Car
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Brand = reader["brand"].ToString(),
                            Model = reader["model"].ToString(),
                            Year = Convert.ToInt32(reader["year"]),
                            Price = Convert.ToDecimal(reader["price"]),
                            CreatedByUserId = Convert.ToInt32(reader["created_by_user_id"])
                        });
                    }
                }
            }
            return cars;
        }

        public Car GetById(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                const string query = @"
                    SELECT id, brand, model, year, price, created_by_user_id
                      FROM cars
                     WHERE id = :id";

                connection.Open();
                using (var cmd = new OracleCommand(query, connection))
                {
                    cmd.Parameters.Add("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Car
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Brand = reader["brand"].ToString(),
                                Model = reader["model"].ToString(),
                                Year = Convert.ToInt32(reader["year"]),
                                Price = Convert.ToDecimal(reader["price"]),
                                CreatedByUserId = Convert.ToInt32(reader["created_by_user_id"])
                            };
                        }
                    }
                }
                return null;
            }
        }

        public void Update(Car car)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                const string query = @"
                    UPDATE cars
                       SET brand = :b,
                           model = :m,
                           year  = :y,
                           price = :p
                     WHERE id    = :id";

                connection.Open();
                using (var cmd = new OracleCommand(query, connection))
                {
                    cmd.Parameters.Add("b", car.Brand);
                    cmd.Parameters.Add("m", car.Model);
                    cmd.Parameters.Add("y", car.Year);
                    cmd.Parameters.Add("p", car.Price);
                    cmd.Parameters.Add("id", car.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                const string query = "DELETE FROM cars WHERE id = :id";
                connection.Open();
                using (var cmd = new OracleCommand(query, connection))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
