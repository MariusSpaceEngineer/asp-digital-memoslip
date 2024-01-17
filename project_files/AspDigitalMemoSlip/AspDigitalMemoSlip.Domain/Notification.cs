using DTOClassLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Domain
{
    
    public class Notification
    {
        public int Id { get; set; }
        public string ConsignerId { get; set; }
        public string ConsgineeId { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
