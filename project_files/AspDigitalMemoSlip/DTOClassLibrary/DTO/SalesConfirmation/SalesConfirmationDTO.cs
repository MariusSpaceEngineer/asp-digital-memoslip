using DTOClassLibrary.DTO.Product;
using DTOClassLibrary.DTO.ProductSale;
using DTOClassLibrary.DTO.SalesConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOClassLibrary.DTO
{
    public class SalesConfirmationDTO
    {
        public int Id { get; set; }
        public string? ConsigneeId { get; set; }
        public string? ConsigneeName { get; set; }
        public ICollection<ProductSaleDTO>? SoldProducts { get; set; } = new List<ProductSaleDTO>();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public SalesConfirmationState SalesConfirmationState { get; set; }
        public double? SuggestedCommision { get; set; }
        public string? PaymentTermDays { get; set; }

        public double TotalSalesPrice
        {
            get
            {
                if (SoldProducts != null)
                {
                    return SoldProducts.Sum(product => product.SalePrice);
                }
                return 0; 
            }
        }

        public double CommissionAmount
        {
            get
            {
                if (SuggestedCommision.HasValue && SoldProducts != null)
                {
                    return (TotalSalesPrice * SuggestedCommision.Value) / 100;
                }
                return 0;
            }
        }
    }
}
