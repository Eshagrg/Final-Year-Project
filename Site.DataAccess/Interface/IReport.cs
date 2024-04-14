using Site.DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DataAccess.Interface
{
    public interface IReport
    {
        Task<List<Sale>> SaleTypeHistoryData(string saleNumber, string startDate, string endDate);
    }
}
