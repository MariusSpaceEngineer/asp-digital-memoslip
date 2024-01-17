using DTOClassLibrary.Enums;

namespace AspDigitalMemoSlip.Mvc.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int ConsignerId { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
