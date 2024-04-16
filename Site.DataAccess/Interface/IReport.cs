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
        Task<List<Sale>> SaleHistory(string startDate, string endDate);
        Task<List<Sale>> SaleDeleteHistory(string startDate, string endDate);
        Task<List<Sale>> SaleTypeHistoryData(string saleNumber, string startDate, string endDate);
    }
}
