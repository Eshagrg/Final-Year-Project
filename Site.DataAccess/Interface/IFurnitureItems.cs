﻿using Site.DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DataAccess.Interface
{
	public interface IFurnitureItems
	{
        IEnumerable<Category> GetCategorylist();
        //IEnumerable<Product_VM> GetCategorylistDropDown();
        bool AddCategory(string categoryName,string createdBy);

        string UpdateCategoryDetail(string obj,int id);
        string DeleteCategoryDetail(int id,string deletedBy);

        IEnumerable<Product> GetProductlist();

        string SaveProductData(Product obj,string createdBy);

        Product_VM GetProductDetailsById(int id);
        string UpdateProductDetail(Product obj, int id);
        string DeleteProductDetail(int id,string deletedBy);


        //DashBoard details
        Task<int> TotalSalesLastWeek();
        //Task<string> TotalIncomeLastWeek();
        Task<int> TotalProducts();
        Task<int> TotalUsers();
        Task<int> TotalCategories();
        Task<int> TotalIncomes();
        Task<IEnumerable<KeyValuePair<string, int>>> SalesLastWeek();
        Task<IEnumerable<KeyValuePair<string, int>>> ProductsTopLastWeek();
        //Task<Dictionary<string, int>> SalesLastWeek();
        //Task<Dictionary<string, int>> ProductsTopLastWeek();
    }
}
