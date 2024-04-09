using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DataAccess.Domain
{
	
	public class Category
	{
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string CreatedBy { get; set; }

        public bool Status { get; set; }

    }

    public class Product_VM
    {
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string Name { get; set; }

        public IFormFile UploadImage { get; set; }

        public int Price { get; set; }
        public string CreatedBy { get; set; }

        public bool Status { get; set; }

        public string Brand { get; set; }
        public int? Quantity { get; set; }

    }

    public class Product
    {
        public int Id { get; set; }
        [Required]
    
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
        [Required]
        public string Name { get; set; }

        public string UploadImage { get; set; }
        public int Price { get; set; }

        public string CreatedBy { get; set; }

        public bool Status { get; set; }

        public string Brand { get; set; }
        public int? Quantity { get; set; }

    }

    public class VMDashBoard
    {
        public int TotalSales { get; set; }
        public string? TotalIncome { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalUsers { get; set; }
        public List<VMSalesWeek> SalesLastWeek { get; set; }
        public List<VMProductsWeek> ProductsTopLastWeek { get; set; }
    }

    public class VMSalesWeek
    {
        public string? Date { get; set; }
        public int Total { get; set; }
    }
    public class VMProductsWeek
    {
        public string? Product { get; set; }
        public int Quantity { get; set; }
    }
}
