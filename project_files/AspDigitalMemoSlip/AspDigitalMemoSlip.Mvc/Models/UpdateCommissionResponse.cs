namespace AspDigitalMemoSlip.Mvc.Models
{
    public class UpdateCommissionResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Commission { get; set; }
        public int CommissionStatus { get; set; }
    }

}
