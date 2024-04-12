﻿using Site.DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DataAccess.Interface
{
    public interface ISales
    {
        IEnumerable<Product> GetProductList(string search);
        bool IsQuantityAvailable(int id, int quantity);
        Task<Sale> Register(Sale entity);
        Task<Sale> Detail(string SaleNumber);
        Task<List<Sale>> SaleHistory(string saleNumber, string startDate, string endDate);
        IEnumerable<VMTypeDocumentSale> GetTypeDocument();

        //Task<List<Product>> GetProducts(string search);
    }
}
