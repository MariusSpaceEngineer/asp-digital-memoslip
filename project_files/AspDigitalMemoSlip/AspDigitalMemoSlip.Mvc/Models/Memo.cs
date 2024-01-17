namespace AspDigitalMemoSlip.Mvc.Models
{
    public class Memo
    {
        public List<Product> Products { get; set; }
        public Consigner? Consigner { get; set; }
        public Consignee? Consignee { get; set; }
        public int? TermsAndConditionsId { get; set; }
        public bool TermsAccepted { get; set; }

        public string ConsignerId { get; set; }
        public string ConsigneeId { get; set; }
        public string? Password { get; set; }

        public DateTime? CreateDate { get; set; }

        public Memo()
        {
            Products = new List<Product>();
        }


    }
}
