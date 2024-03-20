using Site.DataAccess.Domain;
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
        string DeleteCategoryDetail(int id);

        IEnumerable<Product> GetProductlist();

        bool AddProducty(string categoryName, string createdBy);

        string UpdateProductDetail(string obj, int id);
        string DeleteProductDetail(int id);
    }
}
