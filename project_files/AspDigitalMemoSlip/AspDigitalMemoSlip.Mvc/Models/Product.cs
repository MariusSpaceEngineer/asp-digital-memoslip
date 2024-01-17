namespace AspDigitalMemoSlip.Mvc.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string LotNumber { get; set; }
        public string Description { get; set; }

        public int Carat { get; set; }
        public int Price { get; set; }

        public string Remarks { get; set; }
        public bool CommissionPaidBy { get; set; }
        public double CommisionPrice { get; set; }

        public string ConsignerId { get; set; }
        public string ConsigneeId { get; set; }

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
            CounterPrice,

        }

        public Product()
        {
            Id = 0;
            LotNumber = "";
            Description = "";
            Carat = 0;
            Price = 0;
            Remarks = "";

        }
        public Product(int id, string lotNumber, string description, int carat, int price, string remarks)
        {
            Id = id;
            LotNumber = lotNumber;
            Description = description;
            Carat = carat;
            Price = price;
            Remarks = remarks;

        }
    }

}
