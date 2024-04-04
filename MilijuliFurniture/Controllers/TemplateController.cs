using Microsoft.AspNetCore.Mvc;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;

namespace MilijuliFurniture.Controllers
{

    public class TemplateController : Controller
    {
        private readonly ISales _saleService;
       
        public TemplateController(ISales saleService)
        {
            _saleService = saleService;
          
        }

        public async Task<IActionResult> PDFSale(string saleNumber)
        {
            var vmVenta = await _saleService.Detail(saleNumber);

            return View(vmVenta);
        }

    }
}
