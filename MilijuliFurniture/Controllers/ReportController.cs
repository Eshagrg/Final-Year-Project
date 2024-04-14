using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;

namespace MilijuliFurniture.Controllers
{
    [Authorize(Policy = "MustBelongToAdminStaff")]
    public class ReportController : Controller
    {
 
        private readonly IReport _reportService;
        private readonly INotyfService _toastNotificationHero;

        public ReportController(IReport reportService,INotyfService toastHeroNotification)
        {
           
            _reportService = reportService;
            _toastNotificationHero = toastHeroNotification;
        }

        public IActionResult SalesTypeHistory()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SaleTypeHistoryData(string saleNumber, string startDate, string endDate)
        {
            if(!string.IsNullOrEmpty(saleNumber))
            {
                var sales = await _reportService.SaleTypeHistoryData(saleNumber, startDate, endDate);
                List<VMSale> vmHistorySale = sales.Select(s => new VMSale
                {
                    // Map properties manually here
                    SaleNumber = s.SaleNumber,
                    RegistrationDate = s.RegistrationDate.ToString(),
                    ClientName = s.ClientName,
                    Total = s.Total.ToString(),
                    CustomerDocument = s.CustomerDocument,
                    TotalTaxes = s.TotalTaxes.ToString(),

                    DetailSales = s.DetailSales.Select(ds => new VMDetailSale
                    {
                        // Map properties of VMDetailSale here
                        DescriptionProduct = ds.DescriptionProduct,
                        Quantity = ds.Quantity,
                        Price = ds.Price.ToString(),
                        Total = s.Total.ToString(),
                        BrandProduct = ds.BrandProduct,


                    }).ToList()

                    // Map other properties as needed
                }).ToList();
                return StatusCode(StatusCodes.Status200OK, vmHistorySale);
            }
            else if(!string.IsNullOrEmpty(startDate))
            {
                var sales = await _reportService.SaleTypeHistoryData(saleNumber, startDate, endDate);
                List<VMSale> vmHistorySale = sales.Select(s => new VMSale
                {
                    // Map properties manually here
                    SaleNumber = s.SaleNumber,
                    RegistrationDate = s.RegistrationDate.ToString(),
                    ClientName = s.ClientName,
                    Total = s.Total.ToString(),
                    CustomerDocument = s.CustomerDocument,
                    TotalTaxes = s.TotalTaxes.ToString(),

                    DetailSales = s.DetailSales.Select(ds => new VMDetailSale
                    {
                        // Map properties of VMDetailSale here
                        DescriptionProduct = ds.DescriptionProduct,
                        Quantity = ds.Quantity,
                        Price = ds.Price.ToString(),
                        Total = s.Total.ToString(),
                        BrandProduct = ds.BrandProduct,


                    }).ToList()

                    // Map other properties as needed
                }).ToList();
                return StatusCode(StatusCodes.Status200OK, vmHistorySale);
            }
            else
            {
                // If saleNumber is null, return an empty list with 200 OK status
                _toastNotificationHero.Error("Select one Sales Type");
                return BadRequest("Please select one Sales Type");
            }
            
        }
    }
}
