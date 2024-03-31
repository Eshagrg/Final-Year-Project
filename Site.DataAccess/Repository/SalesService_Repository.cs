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

        public async Task<Sale> Register(Sale entity)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connection.DbConnection))
            {
                dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (DetailSale dv in entity.DetailSales)
                        {
                            // Fetch product information using Dapper
                            string query = "SELECT * FROM Product WHERE Id = @ProductId";
                            var product_found = await dbConnection.QueryFirstOrDefaultAsync<Product>(query, new { Productid = dv.ProductId }, transaction);

                            // Update product quantity
                            product_found.Quantity -= dv.Quantity;

                            // Update product quantity using Dapper
                            string updateQuery = "UPDATE Product SET Quantity = @Quantity WHERE Id = @ProductId";
                            await dbConnection.ExecuteAsync(updateQuery, product_found, transaction);
                        }

                        // Update CorrelativeNumber using Dapper
                        string correlativeQuery = "SELECT * FROM CorrelativeNumbers WHERE Management = 'Sale'";
                        var correlative = await dbConnection.QueryFirstOrDefaultAsync<CorrelativeNumber>(correlativeQuery, transaction);
                        correlative.LastNumber++;
                        correlative.DateUpdate = DateTime.Now;

                        string updateCorrelativeQuery = "UPDATE CorrelativeNumbers SET LastNumber = @LastNumber, DateUpdate = @DateUpdate WHERE Id = @Id";
                        await dbConnection.ExecuteAsync(updateCorrelativeQuery, correlative, transaction);

                        // Generate sale number
                        string ceros = new string('0', (int)correlative.QuantityDigits);
                        string saleNumber = ceros + correlative.LastNumber.ToString();
                        saleNumber = saleNumber.Substring((int)(saleNumber.Length - correlative.QuantityDigits));

                        entity.SaleNumber = saleNumber;

                        // Insert sale using Dapper
                        string insertQuery = "INSERT INTO Sales (saleNumber, idTypeDocumentSale, idUsers, customerDocument, clientName, Subtotal, totalTaxes, total, registrationDate) VALUES (@SaleNumber, @IdTypeDocumentSale, @IdUsers, @CustomerDocument, @ClientName, @Subtotal, @TotalTaxes, @Total, @RegistrationDate); SELECT CAST(SCOPE_IDENTITY() as int)";
                        int saleId = await dbConnection.QueryFirstOrDefaultAsync<int>(insertQuery, entity, transaction);

                        // Commit transaction
                        transaction.Commit();

                        return entity;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

    }
}
