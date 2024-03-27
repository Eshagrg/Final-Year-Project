﻿using Dapper;
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


        public string DeleteCategoryDetail(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
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
                    param.Add("@CategoryId", obj.CategoryId);
                    //param.Add("@UploadFile", obj.UploadImage);
                    string output = conn.ExecuteScalar<string>("USP_UpdateProductDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string DeleteProductDetail(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    string output = conn.ExecuteScalar<string>("USP_DeleteProductDetails", param, commandType: CommandType.StoredProcedure);
                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Product GetProductDetailsById(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connection.DbConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);
                    Product output = conn.QueryFirstOrDefault<Product>("USP_GetProductDetailById", param, commandType: CommandType.StoredProcedure);
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
