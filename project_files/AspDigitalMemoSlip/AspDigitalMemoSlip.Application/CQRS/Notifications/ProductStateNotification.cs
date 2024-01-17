using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.DTO.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.CQRS.Notifications
{
    public class ProductStateNotification : INotification
    {
        public NotificationChange Notification { get; }

        public ProductStateNotification(string idInitiator,ProductDTO changedProductDTO)
        {
            Notification = new NotificationChange();
            Notification.ConsginerId = changedProductDTO.ConsignerId;
            Notification.ConsgineeId = idInitiator;
        }
    }
}
