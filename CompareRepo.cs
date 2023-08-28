using Dapper;
using MySqlConnector;
using Npgsql;
using System.Data;
using WebApplication6.Data;
using WebApplication6.Models;

namespace WebApplication6.Repository

{
    public class Compare
    {
       
            private readonly DapperDbContext _dbContext;
            private readonly string _connectionString;
            public Compare(DapperDbContext dbcontext, IConfiguration configuration)
        { 
            string connectionString1 = "DefaultConnection";
            string connectionString2 = "DefaultConnection2";

            using (var connection = new NpgsqlConnection(connectionString1))
            using (var connection2 = new NpgsqlConnection(connectionString2))
            {
                connection.Open();
                connection2.Open();
                var table1 = connection.Query<string>("SELECT table_name FROM information_schema.tables WHERE table_schema = 'public';").AsList();
                var table2 = connection2.Query<string>("SELECT table_name FROM information_schema.tables WHERE table_schema = 'public';").AsList();


                var tablesDifference = new List<string>();

                foreach (var table in table1)
                {
                    if (!table2.Contains(table))
                    {
                        tablesDifference.Add(table);
                    }
                }

                foreach (var table in table2)
                {
                    if (!table1.Contains(table))
                    {
                        tablesDifference.Add(table);
                    }
                }
                Console.WriteLine("Tables with differences:");
                foreach (var table in tablesDifference)
                {
                    Console.WriteLine(table);
                }
            }
        }
    }
}






























