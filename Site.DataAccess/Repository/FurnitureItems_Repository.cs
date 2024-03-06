using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Site.DataAccess.DBConn;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DataAccess.Repository
{
    public class FurnitureItems_Repository : IFurnitureItems
    {
        private readonly ConnectionStrings _connection;

        public FurnitureItems_Repository(IOptions<ConnectionStrings> connection)
        {
            _connection = connection.Value;
        }

        public bool AddCategory(string categoryname, string createdBy)
        {
            using (var connection = new SqlConnection(_connection.DbConnection))
            {
                connection.Open();

                var parameters = new
                {
                    Name = categoryname,
                    CreatedBY = createdBy
                };

                // Call the stored procedure using Dapper's Execute method
                int rowsAffected = connection.Execute("dbo.USP_AddCategory", parameters, commandType: CommandType.StoredProcedure);

                // Check if the update was successful
                return rowsAffected > 0;
            }
        }

        public IEnumerable<Category> GetCategorylist()
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    IEnumerable<Category> output = conn.Query<Category>("USP_GetCategoryList", commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
