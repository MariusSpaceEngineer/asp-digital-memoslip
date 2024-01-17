using DTOClassLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Domain
{
    public class GenericNotification 
    {
        public int Id { get; set; }
        public GenericNotiType EventType { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string ReceiverId { get; set; }
        public string? InitiatorId { get; set; }
        public string? InitiatorName { get; set; }


        public GenericNotification(GenericNotiType eventType, string message, string receiverId, string initiatorId)
        {
            EventType = eventType;
            Message = message;
            ReceiverId = receiverId;
            Timestamp = DateTime.Now;
            InitiatorId = initiatorId;
        }
        public GenericNotification(GenericNotiType eventType, string message, string receiverId)
        {
            EventType = eventType;
            Message = message;
            ReceiverId = receiverId;
            Timestamp = DateTime.Now;
        }
    }
}
