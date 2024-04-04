using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Site.DataAccess.DBConn;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DataAccess.Repository
{
    public class SalesService_Repository : ISales
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



        public async Task<Sale> Register(Sale entity)
        {
            using (var cn = new SqlConnection(_connection.DbConnection))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        // Update CorrelativeNumber using SqlCommand
                        string correlativeQuery = "SELECT * FROM CorrelativeNumber WHERE Management = 'Sale'";
                        using (var correlativeCmd = new SqlCommand(correlativeQuery, cn, tx))
                        {
                            using (var reader = await correlativeCmd.ExecuteReaderAsync())
                            {
                                if (reader.Read())
                                {
                                    int lastNumber = (int)reader["LastNumber"];
                                    int id = (int)reader["CorrelativeNumberId"];
                                    lastNumber++;
                                    DateTime dateUpdate = DateTime.Now;
                                    string ceros = new string('0', (int)reader["QuantityDigits"]);
                                    string saleNumber = ceros + lastNumber.ToString();
                                    saleNumber = saleNumber.Substring((int)(saleNumber.Length - (int)reader["QuantityDigits"]));
                                    // Close the reader before executing another query
                                    reader.Close();
                                    // Generate sale number


                                    entity.SaleNumber = saleNumber;
                                    string updateCorrelativeQuery = "UPDATE CorrelativeNumber SET LastNumber = @LastNumber, DateUpdate = @DateUpdate WHERE CorrelativenumberId = @Id";
                                    using (var updateCorrelativeCmd = new SqlCommand(updateCorrelativeQuery, cn, tx))
                                    {
                                        updateCorrelativeCmd.Parameters.AddWithValue("@LastNumber", lastNumber);
                                        updateCorrelativeCmd.Parameters.AddWithValue("@DateUpdate", dateUpdate);
                                        updateCorrelativeCmd.Parameters.AddWithValue("@Id", id);

                                        await updateCorrelativeCmd.ExecuteNonQueryAsync();


                                    }




                                }
                                else
                                {
                                    reader.Close(); // Close the reader before throwing exception

                                    // Handle the case where CorrelativeNumber is not found
                                    throw new Exception("CorrelativeNumber not found");
                                }
                            }

                            // Insert sale using SqlCommand
                            string insertQuery = "INSERT INTO Sale (saleNumber,UserId, clientName, Subtotal, totalTaxes, total, registrationDate) VALUES (@SaleNumber, @UserId, @ClientName, @Subtotal, @TotalTaxes, @Total, @RegistrationDate); SELECT CAST(SCOPE_IDENTITY() as int)";
                            int saleId;

                            using (var insertCmd = new SqlCommand(insertQuery, cn, tx))
                            {
                                insertCmd.Parameters.AddWithValue("@SaleNumber", entity.SaleNumber);
                                insertCmd.Parameters.AddWithValue("@userId", entity.UserId);

                                insertCmd.Parameters.AddWithValue("@ClientName", entity.ClientName);
                                insertCmd.Parameters.AddWithValue("@Subtotal", entity.Subtotal);
                                insertCmd.Parameters.AddWithValue("@TotalTaxes", entity.TotalTaxes);
                                insertCmd.Parameters.AddWithValue("@Total", entity.Total);
                                insertCmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                                saleId = (int)await insertCmd.ExecuteScalarAsync();
                            }
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
                                            // Close the reader before executing another query
                                            reader.Close();
                                            // Update product quantity using SqlCommand
                                            string updateQuery = "UPDATE Product SET Quantity = @Quantity WHERE Id = @Id";
                                            using (var updateCmd = new SqlCommand(updateQuery, cn, tx))
                                            {
                                                updateCmd.Parameters.AddWithValue("@Quantity", quantity);
                                                updateCmd.Parameters.AddWithValue("@Id", dv.ProductId);
                                                await updateCmd.ExecuteNonQueryAsync();
                                            }
                                            reader.Close();
                                            // Insert detail sale into DetailSale table
                                            string insertDetailSaleQuery = "INSERT INTO DetailSale (ProductId, Quantity, SaleId,brandProduct,categoryProduct,price,total) VALUES (@ProductId, @Quantity, @SaleId,@BrandProduct,@CategoryProduct,@Price,@Total)";
                                            using (var insertDetailSaleCmd = new SqlCommand(insertDetailSaleQuery, cn, tx))
                                            {
                                                insertDetailSaleCmd.Parameters.AddWithValue("@ProductId", dv.ProductId);
                                                insertDetailSaleCmd.Parameters.AddWithValue("@Quantity", dv.Quantity);
                                                insertDetailSaleCmd.Parameters.AddWithValue("@SaleId", saleId);
                                                insertDetailSaleCmd.Parameters.AddWithValue("@BrandProduct", dv.BrandProduct);
                                                insertDetailSaleCmd.Parameters.AddWithValue("@CategoryProduct", dv.CategoryProducty);
                                                insertDetailSaleCmd.Parameters.AddWithValue("@Price", dv.Price);
                                                insertDetailSaleCmd.Parameters.AddWithValue("@Total", dv.Total);
                                                // Assuming saleId is the ID of the current sale
                                                await insertDetailSaleCmd.ExecuteNonQueryAsync();
                                            }
                                        }
                                        else
                                        {
                                            reader.Close(); // Close the reader before throwing exception
                                            // Handle the case where product is not found
                                            throw new Exception("Product not found");
                                        }
                                    }
                                }
                            }


                        }

                        // Commit transaction
                        tx.CommitAsync();

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

        //public async Task<List<Sale>> SaleHistory(string saleNumber, string startDate, string endDate)
        //{
        //    using IDbConnection db = new SqlConnection(_connection.DbConnection);
        //    string query = "";
        //    DateTime start_date = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-PE"));
        //    DateTime end_date = DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-PE"));
        //    if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
        //    {
        //        query = @"SELECT * FROM Sale 
        //              WHERE RegistrationDate BETWEEN @StartDate AND @EndDate";
        //    }
        //    else
        //    {
        //        query = @"SELECT * FROM Sale 
        //              WHERE SaleNumber = @SaleNumber";
        //    }

        //    var parameters = new {StartDate = start_date, EndDate = end_date };
        //    var sales = await db.QueryAsync<Sale>(query, parameters);
        //    return sales.AsList();
        //}

        public async Task<List<Sale>> SaleHistory(string salesNumber, string startDate, string endDate)
        {
            using IDbConnection db = new SqlConnection(_connection.DbConnection);
            string query = "";
            DateTime start_date = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-PE"));
            DateTime end_date = DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-PE"));

            query = @"SELECT s.*, ds.*, p.*,u.FullName AS UserFullName
              FROM Sale s
              JOIN DetailSale ds ON s.SaleID = ds.SaleID
              JOIN Product p ON ds.ProductID = p.Id
              JOIN Portal_Users u ON s.UserID = u.Id
              WHERE s.RegistrationDate BETWEEN @StartDate AND @EndDate";

            var parameters = new { StartDate = start_date, EndDate = end_date };
            var salesDictionary = new Dictionary<int, Sale>(); // Dictionary to store unique Sales by SaleID

            await db.QueryAsync<Sale, DetailSale, Product, string, Sale>(
                query,
                (sale, detailSale, product, userFullName) =>
                {
                    if (!salesDictionary.TryGetValue(sale.SaleId, out Sale saleEntry))
                    {
                        saleEntry = sale;
                        saleEntry.DetailSales = new List<DetailSale>();
                        salesDictionary.Add(saleEntry.SaleId, saleEntry);
                    }

                    detailSale.ProductId = product.Id; // Attach the product to the detail sale
                    detailSale.DescriptionProduct = product.Name;
                    detailSale.Quantity = (int)product.Quantity;
                    saleEntry.DetailSales.Add(detailSale);
                    saleEntry.CustomerDocument = userFullName;
                    return null; // We don't need to return anything here
                },
                parameters,
                splitOn: "SaleID, ProductID,UserFullName" // Assuming "SaleID" and "ProductID" are the columns separating Sale and Product
            );

            return salesDictionary.Values.ToList();
        }

        public async Task<Sale> Detail(string salesNumber)
        {
            using IDbConnection db = new SqlConnection(_connection.DbConnection);
            string query = "";
           

            query = @"SELECT s.*, ds.*, p.*,u.FullName AS UserFullName
              FROM Sale s
              JOIN DetailSale ds ON s.SaleID = ds.SaleID
              JOIN Product p ON ds.ProductID = p.Id
              JOIN Portal_Users u ON s.UserID = u.Id
              WHERE s.SaleNumber = @SaleNumber";

            var parameters = new { SaleNumber = salesNumber };
            var salesDictionary = new Dictionary<int, Sale>(); // Dictionary to store unique Sales by SaleID

            await db.QueryAsync<Sale, DetailSale, Product, string, Sale>(
                query,
                (sale, detailSale, product, userFullName) =>
                {
                    if (!salesDictionary.TryGetValue(sale.SaleId, out Sale saleEntry))
                    {
                        saleEntry = sale;
                        saleEntry.DetailSales = new List<DetailSale>();
                        salesDictionary.Add(saleEntry.SaleId, saleEntry);
                    }

                    detailSale.ProductId = product.Id; // Attach the product to the detail sale
                    detailSale.DescriptionProduct = product.Name;
                    detailSale.Quantity = (int)product.Quantity;
                    saleEntry.DetailSales.Add(detailSale);
                    saleEntry.CustomerDocument = userFullName;
                    return null; // We don't need to return anything here
                },
                parameters,
                splitOn: "SaleID, ProductID,UserFullName" // Assuming "SaleID" and "ProductID" are the columns separating Sale and Product
            );

            return salesDictionary.Values.FirstOrDefault();
        }










    }
}
