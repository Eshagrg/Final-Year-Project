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
        public int Quantity { get; set; }

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
        public int Quantity { get; set; }

    }
}
