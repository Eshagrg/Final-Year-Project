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

        public async Task<List<Sale>> SaleHistory(string startDate, string endDate)
        {
            using IDbConnection db = new SqlConnection(_connection.DbConnection);
            string query = "";

            //DateTime start_date = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-PE"));
            //DateTime end_date = DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-PE"));
            DateTime start_date = startDate != null ? DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-PE")) : DateTime.MinValue;
            DateTime end_date = endDate != null ? DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-PE")) : DateTime.MinValue;

            if (!string.IsNullOrEmpty(startDate))
            {
            query = @"SELECT s.*,ts.description AS CustomerDocument, ds.*,ds.quantity As Quantity,p.Name,p.Price,p.CategoryId,p.Brand, u.FullName AS UserFullName
            FROM Sale s
            JOIN TypeDocumentSale ts ON s.TypeDocumentSaleId = ts.idTypeDocumentSale
            JOIN DetailSale ds ON s.SaleID = ds.SaleID
            JOIN Product p ON ds.ProductID = p.Id
            JOIN Portal_Users u ON s.UserID = u.Id
            WHERE s.RegistrationDate BETWEEN @StartDate AND @EndDate";
            }



            var parameters = new { StartDate = start_date, EndDate = end_date};
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
                    detailSale.Price = product.Price;
                    detailSale.BrandProduct = userFullName;
                    detailSale.Quantity = (int)product.Quantity;
                    saleEntry.DetailSales.Add(detailSale);
                    saleEntry.CustomerDocument = saleEntry.CustomerDocument;



                    return null; // We don't need to return anything here
                },
                parameters,
                splitOn: "SaleID, ProductID,UserFullName" // Assuming "SaleID" and "ProductID" are the columns separating Sale and Product
            );

            return salesDictionary.Values.ToList();
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

        public async Task<List<Sale>> SaleDeleteHistory(string startDate, string endDate)
        {
            using IDbConnection db = new SqlConnection(_connection.DbConnection);
            string query = "";

            //DateTime start_date = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-PE"));
            //DateTime end_date = DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-PE"));
            DateTime start_date = startDate != null ? DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-PE")) : DateTime.MinValue;
            DateTime end_date = endDate != null ? DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-PE")) : DateTime.MinValue;

            if (!string.IsNullOrEmpty(startDate))
            {
                query = @"SELECT 
                        s.*,                                    
                        ts.description AS CustomerDocument,     
                        ds.*,                                   
                        ds.quantity AS Quantity,                
                        p.Name,                                 
                        p.Price,
                        p.DeletedAt,
                        p.CategoryId,                           
                        p.Brand,                                
                        u.FullName AS UserFullName              
                    FROM 
                        Sale s                                  
                    JOIN 
                        TypeDocumentSale ts ON s.TypeDocumentSaleId = ts.idTypeDocumentSale
                    JOIN 
                        DetailSale ds ON s.SaleID = ds.SaleID   
                    JOIN 
                        Product p ON ds.ProductID = p.Id        
                    JOIN 
                        Portal_Users u ON s.UserID = u.Id       
                     
                    WHERE p.DeletedAt BETWEEN @StartDate AND @EndDate
                    And p.isDeleted = 1";
;
            }



            var parameters = new { StartDate = start_date, EndDate = end_date };
            var salesDictionary = new Dictionary<int, Sale>(); // Dictionary to store unique Sales by SaleID

            await db.QueryAsync<Sale, DetailSale, Product, string, Sale>(
                query,
                (sale, detailSale, product, userFullName) =>
                {
                    if (!salesDictionary.TryGetValue(sale.SaleId, out Sale saleEntry))
                    {
                        saleEntry = sale;
                        saleEntry.RegistrationDate = product.DeletedAt;
                        saleEntry.DetailSales = new List<DetailSale>();
                        salesDictionary.Add(saleEntry.SaleId, saleEntry);
                    }

                    detailSale.ProductId = product.Id; // Attach the product to the detail sale
                    detailSale.DescriptionProduct = product.Name;
                    detailSale.Price = product.Price;
                    detailSale.BrandProduct = userFullName;
                    detailSale.Quantity = (int)product.Quantity;
                    saleEntry.DetailSales.Add(detailSale);
                    saleEntry.CustomerDocument = saleEntry.CustomerDocument;
                 



                    return null; // We don't need to return anything here
                },
                parameters,
                splitOn: "SaleID, ProductID,UserFullName" // Assuming "SaleID" and "ProductID" are the columns separating Sale and Product
            );

            return salesDictionary.Values.ToList();
        }
    }
}
