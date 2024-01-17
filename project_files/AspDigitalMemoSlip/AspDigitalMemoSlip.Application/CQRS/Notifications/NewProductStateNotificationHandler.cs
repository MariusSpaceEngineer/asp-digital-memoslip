using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.DTO.Memo;
using DTOClassLibrary.DTO.Product;
using DTOClassLibrary.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.CQRS.Notifications
{

    public class NewProductStateNotificationHandler : INotificationHandler<ProductStateNotification>
    {
        private INotificationRepository _notificationRepository;
        private IUnitOfWork _unitOfWork;

        public NewProductStateNotificationHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork) { 
            this._notificationRepository = notificationRepository;
            this._unitOfWork = unitOfWork;
        }
        public async Task Handle(ProductStateNotification notification, CancellationToken cancellationToken)
        {
            Notification notif = new Notification();
            notif.NotificationType = NotificationType.ProductState;
            notif.ConsignerId = notification.Notification.ConsginerId;
            notif.ConsgineeId = notification.Notification.ConsgineeId;
            

            await _notificationRepository.Create(notif);
        }
    }
}
