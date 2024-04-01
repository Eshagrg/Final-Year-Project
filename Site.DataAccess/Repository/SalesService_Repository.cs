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

        //public async Task<int> RegisterSale(VMSale entity)
        //{
        //    using (IDbConnection dbConnection = new SqlConnection(_connection.DbConnection))
        //    {

        //        if (dbConnection.State == ConnectionState.Closed)
        //            dbConnection.Open();

        //        var parameters = new
        //        {
        //            // Map your entity properties to stored procedure parameters
        //            TypeDocumentSaleId = entity.TypeDocumentSaleId,
        //            CustomerDocument = entity.CustomerDocument,
        //            ClientName = entity.ClientName,
        //            // Map other properties as needed...
        //        };

        //        var result = await dbConnection.ExecuteAsync("USP_RegisterSale", parameters, commandType: CommandType.StoredProcedure);

        //        return result;
        //    }
        //}

        //public async Task<Sale> Register(Sale entity)
        //{
        //    using (IDbConnection dbConnection = new SqlConnection(_connection.DbConnection))
        //    {
        //        dbConnection.Open();
        //        using (var transaction = dbConnection.BeginTransaction())
        //        {
        //            try
        //            {
        //                foreach (DetailSale dv in entity.DetailSales)
        //                {
        //                    // Fetch product information using Dapper
        //                    string query = "SELECT * FROM Product WHERE Id = @ProductId";
        //                    var product_found = await dbConnection.QueryFirstOrDefaultAsync<Product>(query, new { Productid = dv.ProductId }, transaction);

        //                    // Update product quantity
        //                    product_found.Quantity -= dv.Quantity;


        //                    // Update product quantity using Dapper
        //                    string updateQuery = "UPDATE Product SET Quantity = @Quantity WHERE Id = @Id";
        //                    await dbConnection.ExecuteAsync(updateQuery, product_found, transaction);
        //                }

        //                // Update CorrelativeNumber using Dapper
        //                string correlativeQuery = "SELECT * FROM CorrelativeNumber WHERE Management = 'Sale'";
        //                var correlative = await dbConnection.QueryFirstOrDefaultAsync<CorrelativeNumber>(correlativeQuery, transaction);
        //                correlative.LastNumber++;
        //                correlative.DateUpdate = DateTime.Now;

        //                string updateCorrelativeQuery = "UPDATE CorrelativeNumber SET LastNumber = @LastNumber, DateUpdate = @DateUpdate WHERE Id = @Id";
        //                await dbConnection.ExecuteAsync(updateCorrelativeQuery, correlative, transaction);

        //                // Generate sale number
        //                string ceros = new string('0', (int)correlative.QuantityDigits);
        //                string saleNumber = ceros + correlative.LastNumber.ToString();
        //                saleNumber = saleNumber.Substring((int)(saleNumber.Length - correlative.QuantityDigits));

        //                entity.SaleNumber = saleNumber;

        //                // Insert sale using Dapper
        //                string insertQuery = "INSERT INTO Sales (saleNumber, idTypeDocumentSale, idUsers, customerDocument, clientName, Subtotal, totalTaxes, total, registrationDate) VALUES (@SaleNumber, @IdTypeDocumentSale, @IdUsers, @CustomerDocument, @ClientName, @Subtotal, @TotalTaxes, @Total, @RegistrationDate); SELECT CAST(SCOPE_IDENTITY() as int)";
        //                int saleId = await dbConnection.QueryFirstOrDefaultAsync<int>(insertQuery, entity, transaction);

        //                // Commit transaction
        //                transaction.Commit();

        //                return entity;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //}

        //public async Task<Sale> Register(Sale entity)
        //{
        //    using (IDbConnection dbConnection = new SqlConnection(_connection.DbConnection))
        //    {
        //        dbConnection.Open();
        //        var transaction = dbConnection.BeginTransaction();

        //            try
        //            {
        //                foreach (DetailSale dv in entity.DetailSales)
        //                {
        //                    // Fetch product information using Dapper
        //                    string query = "SELECT * FROM Product WHERE Id = @ProductId";
        //                    var product_found = await dbConnection.QueryFirstOrDefaultAsync<Product>(query, new { ProductId = dv.ProductId }, transaction);

        //                    // Update product quantity
        //                    if (product_found != null)
        //                    {
        //                        product_found.Quantity -= dv.Quantity;

        //                        // Update product quantity using Dapper
        //                        string updateQuery = "UPDATE Product SET Quantity = @Quantity WHERE Id = @Id";
        //                        await dbConnection.ExecuteAsync(updateQuery, product_found, transaction);
        //                    }
        //                    else
        //                    {
        //                        // Handle the case where product is not found
        //                        throw new Exception("Product not found");
        //                    }
        //                }

        //                    // Update CorrelativeNumber using Dapper
        //                    string correlativeQuery = "SELECT * FROM CorrelativeNumber WHERE Management = 'Sale'";
        //                    var correlative = await dbConnection.QueryFirstOrDefaultAsync<CorrelativeNumber>(correlativeQuery, transaction);

        //                    if (correlative != null)
        //                    {
        //                        correlative.LastNumber++;
        //                        correlative.DateUpdate = DateTime.Now;

        //                        string updateCorrelativeQuery = "UPDATE CorrelativeNumber SET LastNumber = @LastNumber, DateUpdate = @DateUpdate WHERE Id = @Id";
        //                        await dbConnection.ExecuteAsync(updateCorrelativeQuery, correlative, transaction);

        //                        // Generate sale number
        //                        string ceros = new string('0', (int)correlative.QuantityDigits);
        //                        string saleNumber = ceros + correlative.LastNumber.ToString();
        //                        saleNumber = saleNumber.Substring((int)(saleNumber.Length - correlative.QuantityDigits));

        //                        entity.SaleNumber = saleNumber;

        //                        // Insert sale using Dapper
        //                        string insertQuery = "INSERT INTO Sales (saleNumber, idTypeDocumentSale, idUsers, customerDocument, clientName, Subtotal, totalTaxes, total, registrationDate) VALUES (@SaleNumber, @IdTypeDocumentSale, @IdUsers, @CustomerDocument, @ClientName, @Subtotal, @TotalTaxes, @Total, @RegistrationDate); SELECT CAST(SCOPE_IDENTITY() as int)";
        //                        int saleId = await dbConnection.QueryFirstOrDefaultAsync<int>(insertQuery, entity, transaction);

        //                        // Commit transaction
        //                        transaction.Commit();

        //                        return entity;
        //                    }
        //                    else
        //                    {
        //                        // Handle the case where CorrelativeNumber is not found
        //                        throw new Exception("CorrelativeNumber not found");
        //                    }

        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw;
        //            }

        //    }
        //}

        public async Task<Sale> Register(Sale entity)
        {
            using (var cn = new SqlConnection(_connection.DbConnection))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        foreach (DetailSale dv in entity.DetailSales)
                        {
                            // Fetch product information using SqlCommand
                            string query = "SELECT * FROM Product WHERE Id = @ProductId";
                            using (var cmd = new SqlCommand(query, cn, tx))
                            {
                                cmd.Parameters.AddWithValue("@ProductId", dv.ProductId);
                                using (var reader = await cmd.ExecuteReaderAsync())
                                {
                                    if (reader.Read())
                                    {
                                        // Update product quantity
                                        int quantity = (int)reader["Quantity"];
                                        quantity -= dv.Quantity;

                                        // Update product quantity using SqlCommand
                                        string updateQuery = "UPDATE Product SET Quantity = @Quantity WHERE Id = @Id";
                                        using (var updateCmd = new SqlCommand(updateQuery, cn, tx))
                                        {
                                            updateCmd.Parameters.AddWithValue("@Quantity", quantity);
                                            updateCmd.Parameters.AddWithValue("@Id", dv.ProductId);
                                            await updateCmd.ExecuteNonQueryAsync();
                                        }
                                    }
                                    else
                                    {
                                        // Handle the case where product is not found
                                        throw new Exception("Product not found");
                                    }
                                }
                            }
                        }

                        // Update CorrelativeNumber using SqlCommand
                        string correlativeQuery = "SELECT * FROM CorrelativeNumber WHERE Management = 'Sale'";
                        using (var correlativeCmd = new SqlCommand(correlativeQuery, cn, tx))
                        {
                            using (var reader = await correlativeCmd.ExecuteReaderAsync())
                            {
                                if (reader.Read())
                                {
                                    int lastNumber = (int)reader["LastNumber"];
                                    lastNumber++;
                                    DateTime dateUpdate = DateTime.Now;

                                    string updateCorrelativeQuery = "UPDATE CorrelativeNumber SET LastNumber = @LastNumber, DateUpdate = @DateUpdate WHERE Id = @Id";
                                    using (var updateCorrelativeCmd = new SqlCommand(updateCorrelativeQuery, cn, tx))
                                    {
                                        updateCorrelativeCmd.Parameters.AddWithValue("@LastNumber", lastNumber);
                                        updateCorrelativeCmd.Parameters.AddWithValue("@DateUpdate", dateUpdate);
                                        updateCorrelativeCmd.Parameters.AddWithValue("@Id", (int)reader["Id"]);
                                        await updateCorrelativeCmd.ExecuteNonQueryAsync();
                                    }

                                    // Generate sale number
                                    string ceros = new string('0', (int)reader["QuantityDigits"]);
                                    string saleNumber = ceros + lastNumber.ToString();
                                    saleNumber = saleNumber.Substring((int)(saleNumber.Length - (int)reader["QuantityDigits"]));

                                    entity.SaleNumber = saleNumber;

                                    // Insert sale using SqlCommand
                                    string insertQuery = "INSERT INTO Sales (saleNumber,UserId, customerDocument, clientName, Subtotal, totalTaxes, total, registrationDate) VALUES (@SaleNumber, @UserId, @CustomerDocument, @ClientName, @Subtotal, @TotalTaxes, @Total, @RegistrationDate); SELECT CAST(SCOPE_IDENTITY() as int)";
                                    using (var insertCmd = new SqlCommand(insertQuery, cn, tx))
                                    {
                                        insertCmd.Parameters.AddWithValue("@SaleNumber", entity.SaleNumber);
                                        insertCmd.Parameters.AddWithValue("@userId", entity.UserId);
                                        insertCmd.Parameters.AddWithValue("@CustomerDocument", entity.CustomerDocument);
                                        insertCmd.Parameters.AddWithValue("@ClientName", entity.ClientName);
                                        insertCmd.Parameters.AddWithValue("@Subtotal", entity.Subtotal);
                                        insertCmd.Parameters.AddWithValue("@TotalTaxes", entity.TotalTaxes);
                                        insertCmd.Parameters.AddWithValue("@Total", entity.Total);
                                        insertCmd.Parameters.AddWithValue("@RegistrationDate", entity.RegistrationDate);
                                        int saleId = (int)await insertCmd.ExecuteScalarAsync();
                                    }
                                }
                                else
                                {
                                    // Handle the case where CorrelativeNumber is not found
                                    throw new Exception("CorrelativeNumber not found");
                                }
                            }
                        }

                        // Commit transaction
                        tx.Commit();

                        return entity;
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction on exception
                        tx.Rollback();
                        throw;
                    }
                }
            }

        }




    }
}
