using DTOClassLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Domain
{
    public class ProductSale
    {
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int SalesConfirmationId { get; set; }
        public SalesConfirmation SalesConfirmation { get; set; }
        public int LotNumber { get; set; }
        public int CaratsSold { get; set; }
        public double SalePrice { get; set; }
        public DateTime SaleDate { get; set; }
        public bool AgreedPrice { get; set; }
        public AgreementState AgreementStates { get; set; }
  
    }
}
