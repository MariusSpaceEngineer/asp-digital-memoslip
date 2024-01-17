using DTOClassLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOClassLibrary.DTO.Notification
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public GenericNotiType EventType { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string? ReceiverId { get; set; }
        public string? InitiatorId { get; set; }
        public string? InitiatorName { get; set; }
    }
}
