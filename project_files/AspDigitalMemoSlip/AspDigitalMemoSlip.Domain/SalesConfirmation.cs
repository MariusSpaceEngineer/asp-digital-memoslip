using DTOClassLibrary.DTO.SalesConfirmation;
using System;
using System.Collections.Generic;

namespace AspDigitalMemoSlip.Domain
{
    public class SalesConfirmation
    {
        public int Id { get; set; }
        public string? ConsigneeId { get; set; }
        public Consignee? Consignee { get; set; }

        public string? ConsignerId { get; set; }
        public Consigner? Consigner { get; set; }

        public ICollection<ProductSale>? ProductSales { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
        public SalesConfirmationState SalesConfirmationState { get; set; }
        public double? SuggestedCommision { get; set; }
        public string? PaymentTermDays { get; set; }

    }
}
