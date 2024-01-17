using DTOClassLibrary.DTO.Consignee;
using DTOClassLibrary.DTO.Consigner;
using DTOClassLibrary.Enums;

namespace DTOClassLibrary.DTO.Invoice
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public int MemoId { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public double Commision { get; set; }
        public Status CommisionStatus { get; set; }
        public string ConsigneeId { get; set; }
        public ConsigneeDTO Consignee { get; set; }
        public string ConsignerId { get; set; }
        public ConsignerDTO Consigner { get; set; }
        public ICollection<SalesConfirmationDTO> SaleConfirmation { get; set; }
    }

}
