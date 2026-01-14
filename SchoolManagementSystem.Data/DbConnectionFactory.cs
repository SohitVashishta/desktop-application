using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data
{
    public static class DbConnectionFactory
    {
        private static readonly string _connectionString =
            "Server=.;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True";

        public static IDbConnection Create()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
