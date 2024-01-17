using DTOClassLibrary.DTO.Product;

namespace AspDigitalMemoSlip.Domain
{
    public class Product
    {
        public int Id { get; set; }

        public int MemoId { get; set; }
        public Memo Memo { get; set; }
        public int? SalesConfirmationId { get; set; }
        public SalesConfirmation? SalesConfirmation { get; set; }


        public string ConsignerId { get; set; }
        public Consigner? Consigner { get; set; }

        public string ConsigneeId { get; set; }
        public Consignee? Consignee { get; set; }

        public string LotNumber { get; set; }
        public string Description { get; set; }
        public double CommisionPrice { get; set; }
        public int Carat { get; set; }
        public int Price { get; set; }
        public ProductState State { get; set; }
        public string? Remarks { get; set; }
        // public int CommissionPaidBy { get; set; }
        public int SuggestedPrice
        {
            get;
            set;
        }
        public SoldStatus ProductSoldStatus { get; set; }
        public enum SoldStatus
        {
            NotSold,
            SuggestedPrice,
            RefusedPrice,
            SoldPrice,
            CounterPrice
        }
        public Product()
        {
            Id = 0;
            LotNumber = "";
            Description = "";
            Carat = 0;
            Price = 0;
            Remarks = "";
            ProductSoldStatus = SoldStatus.NotSold;

        }
        public Product(int id, string lotNumber, string description, int carat, int price, string remarks)
        {
            Id = id;
            LotNumber = lotNumber;
            Description = description;
            Carat = carat;
            Price = price;
            Remarks = remarks;
            ProductSoldStatus = SoldStatus.NotSold;



        }
    }
}
