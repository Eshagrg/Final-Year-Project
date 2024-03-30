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
    public class SalesService_Repository: ISales
    {
        private readonly ConnectionStrings _connection;

        public SalesService_Repository(IOptions<ConnectionStrings> connection)
        {
            _connection = connection.Value;
        }


     

        public IEnumerable<Product> GetProductList(string search)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    var parameters = new { SearchTerm = search };
                    IEnumerable<Product> output = conn.Query<Product>("USP_GetProductListForSales", parameters, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool IsQuantityAvailable(int productId, int quantity)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    var parameters = new { ProductId = productId, Quantity = quantity };
                    bool isAvailable = conn.QuerySingle<bool>("USP_CheckQuantityAvailability", parameters, commandType: CommandType.StoredProcedure);
                    return isAvailable;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
