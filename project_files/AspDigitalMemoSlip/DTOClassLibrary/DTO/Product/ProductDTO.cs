using DTOClassLibrary.Enums;

namespace DTOClassLibrary.DTO.Product
{
    public class ProductDTO
    {
        public int ID { get; set; }
        public int SalesConfirmationId { get; set; }
        public string ConsignerId { get; set; }
        public string ConsigneeId { get; set; }
        public string LotNumber { get; set; }
        public string Description { get; set; }
        public int MemoId { get; set; }
        public int Carat { get; set; }
        public int Price { get; set; }
        public double CommisionPrice { get; set; }
        public string Remarks { get; set; }
        //  public int CommissionPaidBy { get; set; }

        public bool Selected { get; set; }

        public ProductState State { get; set; }
        public int SuggestedPrice
        {
            get;
            set;

        }
        public SoldStatusDTO ProductSoldStatus { get; set; }
    }
}
