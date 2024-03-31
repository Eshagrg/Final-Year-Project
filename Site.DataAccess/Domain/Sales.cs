using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DataAccess.Domain
{
    public class VMSale
    {
        public int SaleId { get; set; }
        public string? SaleNumber { get; set; }
        public int? TypeDocumentSaleId { get; set; }
        public string? TypeDocumentSale { get; set; }
        public int? UserId { get; set; }
        public string? User { get; set; }
        public string? CustomerDocument { get; set; }
        public string? ClientName { get; set; }
        public string? Subtotal { get; set; }
        public string? TotalTaxes { get; set; }
        public string? Total { get; set; }
        public string? RegistrationDate { get; set; }
        public virtual ICollection<VMDetailSale> DetailSales { get; set; }
    }
    public partial class Sale
    {
        public Sale()
        {
            DetailSales = new HashSet<DetailSale>();
        }

        public int SaleId { get; set; }
        public string? SaleNumber { get; set; }
        public int? UserId { get; set; }
        public string? CustomerDocument { get; set; }
        public string? ClientName { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? TotalTaxes { get; set; }
        public decimal? Total { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public virtual Users? UsersNavigationId { get; set; }
        public virtual ICollection<DetailSale> DetailSales { get; set; }
    }
    public class VMDetailSale
    {
        public int? ProductId { get; set; }
        public string? BrandProduct { get; set; }
        public string? DescriptionProduct { get; set; }
        public string? CategoryProducty { get; set; }
        public int? Quantity { get; set; }
        public string? Price { get; set; }
        public string? Total { get; set; }
    }

    public partial class DetailSale
    {
        public int DetailSaleId { get; set; }
        public int? SaleId { get; set; }
        public int? ProductId { get; set; }
        public string? BrandProduct { get; set; }
        public string? DescriptionProduct { get; set; }
        public string? CategoryProducty { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }

        public virtual Sale? SaleNavigationId { get; set; }
    }

    public partial class CorrelativeNumber
    {
        public int IdCorrelativeNumber { get; set; }
        public int? LastNumber { get; set; }
        public int? QuantityDigits { get; set; }
        public string? Management { get; set; }
        public DateTime? DateUpdate { get; set; }
    }
}
