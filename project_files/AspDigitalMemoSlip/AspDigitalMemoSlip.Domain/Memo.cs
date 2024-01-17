namespace AspDigitalMemoSlip.Domain
{
    public class Memo
    {
        public int Id { get; set; } 
        public int TermsAndConditionsId { get; set; }
        public bool TermsAccepted { get; set; }
        public string ConsignerId { get; set; }
        public string ConsigneeId { get; set; }
        public DateTime CreateDate { get; set; }
        public Consigner Consigner { get; set; }
        public Consignee Consignee { get; set; }
        public List<Product> Products { get; set; }

        public bool AcceptedByConsignee { get; set; }

        

        public Memo()
        {
            Products = new List<Product>();
            CreateDate = DateTime.Now;
        }
    }
}
