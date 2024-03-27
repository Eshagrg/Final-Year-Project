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
        //public IEnumerable<Product> GetProductlist()
        //{
        //    try
        //    {
        //        using (var conn = new SqlConnection(_connection.DbConnection))
        //        {
        //            IEnumerable<Product> output = conn.Query<Product>("USP_GetProductList", commandType: CommandType.StoredProcedure);
        //            return output;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<List<Product>> GetProducts(string search)
        //{
        //    try
        //    {
        //        using (var conn = new SqlConnection(_connection.DbConnection))
        //        {
        //            // Construct the SQL query to filter products based on search criteria
        //            string query = @"
        //        SELECT p.*, c.*
        //        FROM Products p
        //        INNER JOIN Categories c ON p.CategoryId = c.Id
        //        WHERE p.IsActive = 1
        //            AND p.Quantity > 0
        //            AND CONCAT(p.BarCode, p.Brand, p.Description) LIKE @Search
        //    ";

        //            // Execute the query and return the results
        //            var products = await conn.QueryAsync<Product, Category, Product>(
        //                query,
        //                (product, category) =>
        //                {
        //                    product.IdCategoryNavigation = category;
        //                    return product;
        //                },
        //                new { Search = "%" + search + "%" },
        //                splitOn: "Id"
        //            );

        //            return products.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions
        //        throw ex;
        //    }
        //}

     

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
    }
}
