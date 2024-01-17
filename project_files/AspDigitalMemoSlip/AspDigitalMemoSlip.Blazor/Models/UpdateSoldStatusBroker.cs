using DTOClassLibrary.Enums;
namespace AspDigitalMemoSlip.Blazor.Models
{
    public class UpdateSoldStatusBroker
    {
        public int ProductId { get; set; }
        public SoldStatusDTO Status { get; set; }
        public int SuggestedPrice { get; set; }
        public bool CommissionPaidBy { get; set; }
        public double CommisionPrice { get; set; }
    }
}

