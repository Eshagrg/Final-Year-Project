using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;

namespace MilijuliFurniture.Controllers
{
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

        //[HttpGet]
        //public async Task<IActionResult> GetProducts(string search)
        //{
        //    List<Product> obj = await _salesService.GetProducts(search);
        //    return StatusCode(StatusCodes.Status200OK, obj);
        //}
    }
}
