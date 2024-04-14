using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
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
    public class ReportService_Repository: IReport
    {
        private readonly ConnectionStrings _connection;

        public ReportService_Repository(IOptions<ConnectionStrings> connection)
        {
            _connection = connection.Value;
        }

        public async Task<List<Sale>> SaleTypeHistoryData(string salesNumber, string startDate, string endDate)
        {
            using IDbConnection db = new SqlConnection(_connection.DbConnection);
            string query = "";

            //DateTime start_date = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-PE"));
            //DateTime end_date = DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-PE"));
            DateTime start_date = startDate != null ? DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-PE")) : DateTime.MinValue;
            DateTime end_date = endDate != null ? DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-PE")) : DateTime.MinValue;

            if (!string.IsNullOrEmpty(salesNumber))
            {
                query = @"Select * 
						from Sale s
						JOIN TypeDocumentSale ts ON s.TypeDocumentSaleId = ts.idTypeDocumentSale
						WHERE ts.idTypeDocumentSale = @SalesNumber";
            }
            else
            {
                query = @"Select * 
						from Sale s
						JOIN TypeDocumentSale ts ON s.TypeDocumentSaleId = ts.idTypeDocumentSale
                        WHERE s.RegistrationDate BETWEEN @StartDate AND @EndDate";
            }



            var parameters = new { StartDate = start_date, EndDate = end_date, SalesNumber = salesNumber };
            var salesDictionary = new Dictionary<int, Sale>(); // Dictionary to store unique Sales by SaleID

            await db.QueryAsync<Sale, VMTypeDocumentSale, Sale>(
                query,
                (sale, typeDocumentSale) =>
                {
                    if (!salesDictionary.TryGetValue(sale.SaleId, out Sale saleEntry))
                    {
                        saleEntry = sale;
                        saleEntry.CustomerDocument = sale.CustomerDocument;
                        salesDictionary.Add(saleEntry.SaleId, saleEntry);
                    }

                    return null; // We don't need to return anything here
                },
                parameters,
                splitOn: "idTypeDocumentSale" // Assuming "SaleID" and "ProductID" are the columns separating Sale and Product
            ); ;

            return salesDictionary.Values.ToList();
        }
    }
}
