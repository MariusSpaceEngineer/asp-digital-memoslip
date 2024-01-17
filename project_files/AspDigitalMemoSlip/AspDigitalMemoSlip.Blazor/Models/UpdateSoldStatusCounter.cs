using DTOClassLibrary.Enums;
namespace AspDigitalMemoSlip.Blazor.Models
{
    public class UpdateSoldStatusCounter
    {
        public int ProductId { get; set; }
        public SoldStatusDTO Status { get; set; }
        public int SuggestedPrice { get; set; }
        public int CommissionPaidBy { get; set; }
        public double CommisionPrice { get; set; }
    }
}
