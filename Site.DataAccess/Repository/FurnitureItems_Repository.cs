using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Site.DataAccess.DBConn;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DataAccess.Repository
{
    public class FurnitureItems_Repository : IFurnitureItems
    {
        private readonly ConnectionStrings _connection;
        private DateTime StartDate = DateTime.Now;
        public FurnitureItems_Repository(IOptions<ConnectionStrings> connection)
        {
            _connection = connection.Value;
            StartDate = StartDate.AddDays(-7);
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


        public string DeleteCategoryDetail(int id, string deletedBy)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    param.Add("@DeletedBy", deletedBy);
                    string output = conn.ExecuteScalar<string>("USP_DeleteCategoryDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
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

        //public IEnumerable<Product_VM> GetCategorylistDropDown()
        //{
        //    try
        //    {
        //        using (var conn = new SqlConnection(_connection.DbConnection))
        //        {
        //            IEnumerable<Product_VM> output = conn.Query<Product_VM>("USP_GetCategoryList", commandType: CommandType.StoredProcedure);
        //            return output;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public IEnumerable<Product> GetProductlist()
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    IEnumerable<Product> output = conn.Query<Product>("USP_GetProductList", commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string UpdateCategoryDetail(string obj, int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    param.Add("@CategoryName", obj);
                    string output = conn.ExecuteScalar<string>("USP_UpdateCategoryDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public string SaveProductData(Product obj, string createdBy)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Name", obj.Name);
                    param.Add("@Price", obj.Price);
                    param.Add("@CategoryId", obj.CategoryId);
                    param.Add("@CreatedBy", createdBy);
                    param.Add("@UploadFile", obj.UploadImage);
                    param.Add("@Quantity", obj.Quantity);
                    param.Add("@Brand", obj.Brand);
                    string output = conn.ExecuteScalar<string>("USP_SaveProductDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string UpdateProductDetail(Product obj, int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    param.Add("@Name", obj.Name);
                    param.Add("@Price", obj.Price);
                    param.Add("@Quantity", obj.Quantity);
                    param.Add("@Brand", obj.Brand);
                    param.Add("@CategoryId", obj.CategoryId);
                    param.Add("@UploadFile", obj.UploadImage);
                    string output = conn.ExecuteScalar<string>("USP_UpdateProductDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string DeleteProductDetail(int id,string deletedBy)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    param.Add("@DeletedBy", deletedBy);
                    string output = conn.ExecuteScalar<string>("USP_DeleteProductDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Product_VM GetProductDetailsById(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    Product_VM output = conn.QueryFirstOrDefault<Product_VM>("USP_GetProductDetailById", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //DashBoard 

        public async Task<int> TotalSalesLastWeek()
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    string sql = "SELECT COUNT(*) FROM Sale WHERE RegistrationDate >= @StartDate";
                    int total = await conn.ExecuteScalarAsync<int>(sql, new { StartDate = StartDate.Date });
                    return total;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> TotalProducts()
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    string sql = "SELECT COUNT(*) FROM Product Where isDeleted =0";
                    int total = await conn.ExecuteScalarAsync<int>(sql);
                    return total;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> TotalCategories()
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    string sql = "SELECT COUNT(*) FROM Category Where isDeleted=0";
                    int total = await conn.ExecuteScalarAsync<int>(sql);
                    return total;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> TotalUsers()
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    string sql = "SELECT COUNT(*) FROM Portal_Users";
                    int total = await conn.ExecuteScalarAsync<int>(sql);
                    return total;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<KeyValuePair<string, int>>> ProductsTopLastWeek()
        {
            try
            {
                using (var connection = new SqlConnection(_connection.DbConnection))
                {
                    // Write the SQL query to retrieve top product sales data
                    string sql = @"
                SELECT TOP 4 p.Name AS Product,
                    COUNT(*) AS Total
                    FROM Product p
                    Inner JOIN DetailSale ds On p.Id = ds.ProductId
                    INNER JOIN Sale s ON ds.SaleId = s.SaleId
                    WHERE s.RegistrationDate >= 4/9/1
                    GROUP BY p.Name
                    ORDER BY COUNT(*) DESC";
                    
                    // Execute the SQL query using Dapper
                    var salesData = await connection.QueryAsync<dynamic>(sql, new { StartDate });

                    // Process the query result to create the dictionary
                    var resultado = salesData.ToDictionary(row => (string)row.Product, row => (int)row.Total);

                    return resultado;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                throw new Exception("Error fetching top product sales data from the database", ex);
            }
        }

      

        async Task<IEnumerable<KeyValuePair<string, int>>> IFurnitureItems.SalesLastWeek()
        {
            try
            {
                using (var connection = new SqlConnection(_connection.DbConnection))
                {
                    // Write the SQL query to retrieve sales data for the last week
                    string sql = @"
                SELECT CONVERT(VARCHAR(10), RegistrationDate, 103) AS Date,
                       COUNT(*) AS Total
                FROM Sale
                WHERE RegistrationDate >= DATEADD(DAY, -7, GETDATE())
                GROUP BY CONVERT(VARCHAR(10), RegistrationDate, 103)
                ORDER BY CONVERT(VARCHAR(10), RegistrationDate, 103) DESC";

                    // Execute the SQL query using Dapper
                    var salesData = await connection.QueryAsync<dynamic>(sql);

                    // Process the query result to create the dictionary
                    var resultado = salesData.ToDictionary(row => (string)row.Date, row => (int)row.Total);

                    return resultado;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                throw new Exception("Error fetching sales data from the database", ex);

            }
        }

       
    }
}
