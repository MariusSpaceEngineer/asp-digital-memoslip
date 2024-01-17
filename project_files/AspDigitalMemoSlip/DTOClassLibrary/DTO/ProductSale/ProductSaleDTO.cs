using DTOClassLibrary.DTO.Product;
using DTOClassLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOClassLibrary.DTO.ProductSale
{
    public class ProductSaleDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SalesConfirmationId { get; set; }
        public string? LotNumber { get; set; }
        public int SalePrice { get; set; }
        public int CaratsSold { get; set; }
        public bool AgreedPrice { get; set; }
        public int MemoId { get; set; }
        public AgreementState AgreementStates { get; set; }
        public bool isEditing { get; set; }
        public int NewPrice { get; set; }
    }
}
