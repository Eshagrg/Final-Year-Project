using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System.Collections.Generic;
using System.Security.Claims;

namespace MilijuliFurniture.Controllers
{
    [Authorize(Policy = "MustBelongToAdminStaff")]
    public class SalesController : Controller
    {
        private readonly ISales _salesService;
        private readonly INotyfService _toastNotificationHero;

        public SalesController(ISales salesService, INotyfService toastNotificationHero, IFurnitureItems furnitureItems)
        {

            _salesService = salesService;
            _toastNotificationHero = toastNotificationHero;
            
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

        [HttpGet]
        public IActionResult CheckQuantity(int id, int quantity)
        {
            bool available = _salesService.IsQuantityAvailable(id, quantity);
            return Json(new { available });
        }

        //[HttpPost]
        //public async Task<IActionResult> RegisterSale([FromBody] VMSale model)
        //{
        //    try
        //    {
        //        string user = User.Identity.Name;
        //        string userId = User.FindFirst("UserId").Value;


        //        // Set the userId in the model
        //        model.User = (user);
        //        model.UserId = int.Parse(userId);

        //        int result = await _salesService.RegisterSale(model);

        //        if (result > 0)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, new { Success = true });
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = "Failed to register sale." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = ex.Message });
        //    }
        //}

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
                    DetailSales = detailSales

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


        //[HttpGet]
        //public async Task<IActionResult> GetProducts(string search)
        //{
        //    List<Product> obj = await _salesService.GetProducts(search);
        //    return StatusCode(StatusCodes.Status200OK, obj);
        //}
    }
}
