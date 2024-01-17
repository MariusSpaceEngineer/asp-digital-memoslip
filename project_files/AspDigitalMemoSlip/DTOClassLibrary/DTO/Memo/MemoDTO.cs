using DTOClassLibrary.DTO.Consignee;
using DTOClassLibrary.DTO.Consigner;
using DTOClassLibrary.DTO.Product;

namespace DTOClassLibrary.DTO.Memo
{
    public class MemoDTO
    {
        public int Id { get; set; }
        public List<ProductDTO> Products { get; set; }
        public ConsignerDTO? Consigner { get; set; }
        public ConsigneeDTO? Consignee { get; set; }
        public int? TermsAndConditionsId { get; set; }
        public bool TermsAccepted { get; set; }
        public bool AcceptedByConsignee { get; set; }
        public DateTime CreateDate { get; set; }

        public string ConsignerId { get; set; }
        public string ConsigneeId { get; set; }
        public string? Password { get; set; }

        public MemoDTO()
        {
            Products = new List<ProductDTO>();
            Consigner = new ConsignerDTO();
            Consignee = new ConsigneeDTO();
        }

        public double TotalPrice()
        {
            return Products.Sum(p => p.Price);
        }
    }
}