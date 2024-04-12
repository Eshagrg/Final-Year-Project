using AspNetCoreHero.ToastNotification.Abstractions;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Security.Claims;

namespace MilijuliFurniture.Controllers
{
    [Authorize(Policy = "MustBelongToAdminStaff")]
    public class SalesController : Controller
    {
        private readonly ISales _salesService;
        private readonly INotyfService _toastNotificationHero;
        private readonly IConverter _converter;
   

        public SalesController(ISales salesService, INotyfService toastNotificationHero, IFurnitureItems furnitureItems, IConverter converter)
        {

            _salesService = salesService;
            _toastNotificationHero = toastNotificationHero;
            _converter = converter;
          
        }
        public IActionResult SalesIndex()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetProducts(string search)
        {
            IEnumerable<Product> obj = _salesService.GetProductList(search);
            return StatusCode(StatusCodes.Status200OK,obj);
        }

        public IActionResult ListTypeDocumentSale()
        {
            IEnumerable<VMTypeDocumentSale> vmListTypeDocumentSale = _salesService.GetTypeDocument();
            return StatusCode(StatusCodes.Status200OK, vmListTypeDocumentSale);
        }

        [HttpGet]
        public IActionResult CheckQuantity(int id, int quantity)
        {
            bool available = _salesService.IsQuantityAvailable(id, quantity);
            return Json(new { available });
        }


        [HttpPost]
        public async Task<IActionResult> RegisterSale([FromBody] VMSale model)
        {
            try
            {
                // Get the user id from claims
                string userId = User.FindFirst("UserId").Value;
                string user = User.Identity.Name;

                List<DetailSale> detailSales = model.DetailSales.Select(vmDetailSale => new DetailSale
                {
                    // Map properties from VMDetailSale to DetailSale
                    ProductId = vmDetailSale.ProductId,
                    Price = decimal.Parse(vmDetailSale.Price),
                    Quantity = (int)vmDetailSale.Quantity,
                    BrandProduct = vmDetailSale.BrandProduct,
                    CategoryProducty = vmDetailSale.CategoryProducty,
                    Total = decimal.Parse(vmDetailSale.Total)


                    // Map other properties as needed...
                }).ToList();
                Sale sale = new Sale
                {
                    UserId = int.Parse(userId),
                    ClientName = model.ClientName,
                    Subtotal = decimal.Parse(model.Subtotal),
                    TotalTaxes = decimal.Parse(model.TotalTaxes),
                    Total = decimal.Parse(model.Total),
                    DetailSales = detailSales,
                    TypeDocumentSaleId = model.TypeDocumentSaleId
                    
                    

                };
                // Set the user id in the model
              

                // Call the service method to register the sale
                Sale createdSale = await _salesService.Register(sale);

                // Check if sale is created successfully
                if (createdSale != null)
                {
                    // Return success response
                    return StatusCode(StatusCodes.Status200OK, new { Success = true, SaleNumber = createdSale.SaleNumber });
                }
                else
                {
                    // Return error response if sale creation fails
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = "Failed to register sale." });
                }
            }
            catch (Exception ex)
            {
                // Return error response in case of exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = ex.Message });
            }
        }


        public IActionResult SalesHistory()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> History(string saleNumber, string startDate, string endDate)
        {
            var sales = await _salesService.SaleHistory(saleNumber, startDate, endDate);
            List<VMSale> vmHistorySale = sales.Select(s => new VMSale
            {
                // Map properties manually here
                SaleNumber = s.SaleNumber,
                RegistrationDate = s.RegistrationDate.ToString(),
                ClientName = s.ClientName,
                Total =s.Total.ToString(),
                CustomerDocument = s.CustomerDocument,
                DetailSales = s.DetailSales.Select(ds => new VMDetailSale
                {
                    // Map properties of VMDetailSale here
                    DescriptionProduct = ds.DescriptionProduct,
                    Quantity = ds.Quantity,
                    Price = ds.Price.ToString(),
                    Total = s.Total.ToString(),
                    
                }).ToList()

                // Map other properties as needed
            }).ToList();
            return StatusCode(StatusCodes.Status200OK, vmHistorySale);
        }


        public IActionResult ShowPDFSale(string saleNumber)
        {
            string urlTemplateView = $"{this.Request.Scheme}://{this.Request.Host}/Template/PDFSale?saleNumber={saleNumber}";

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings(){
                        Page = urlTemplateView
                     
                    }
                }
            };
            var archivoPDF = _converter.Convert(pdf);
            return File(archivoPDF, "application/pdf");
        }
        //[HttpGet]
        //public async Task<IActionResult> GetProducts(string search)
        //{
        //    List<Product> obj = await _salesService.GetProducts(search);
        //    return StatusCode(StatusCodes.Status200OK, obj);
        //}
    }
}
