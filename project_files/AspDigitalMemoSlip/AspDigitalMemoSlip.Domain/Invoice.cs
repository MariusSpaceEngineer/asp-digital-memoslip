namespace AspDigitalMemoSlip.Domain
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public double Commision { get; set; }
        public Status CommisionStatus { get; set; }
        public string ConsigneeId { get; set; }
        public Consignee Consignee { get; set; }
        public string ConsignerId { get; set; }
        public Consigner Consigner { get; set; }
        public ICollection<SalesConfirmation> SaleConfirmation { get; set; }

    }

    public enum Status
    {
        Paid,
        Unpaid
    }
}
