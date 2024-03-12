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

        bool AddCategory(string categoryName,string createdBy);

        string UpdateCategoryDetail(Category obj);
        string DeleteCategoryDetail(int id);
    }
}
