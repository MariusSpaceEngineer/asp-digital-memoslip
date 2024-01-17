namespace AspDigitalMemoSlip.Mvc.Models
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public int MemoId { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public double Commision { get; set; }
        public Status CommisionStatus { get; set; } = Status.Unpaid;
        public int TotalPages { get; set; }
        public int PageNr { get; set; }
    }

    public enum Status
    {

        Paid,
        Unpaid
    }
}
