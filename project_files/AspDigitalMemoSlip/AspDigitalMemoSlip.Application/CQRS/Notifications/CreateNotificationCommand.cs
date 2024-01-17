using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.CQRS.Notifications
{
    public class CreateNotificationCommand : IRequest<GenericNotification>
    {
        public GenericNotiType EventType { get; }
        public string Message { get; }
        public string ReceiverId { get; }
        public string InitiatorId { get; set; }
        public string? InitiatorName { get; set; }

        public CreateNotificationCommand(GenericNotiType eventType, string message, string receiverId,string initiatorId)
        {
            EventType = eventType;
            Message = message;
            ReceiverId = receiverId;
            InitiatorId = initiatorId;
        }
    }
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, GenericNotification>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateNotificationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericNotification> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new GenericNotification(request.EventType, request.Message, request.ReceiverId);

            Consignee initiator = await _unitOfWork.ConsigneeRepository.GetConsigneeByUserId(request.InitiatorId);
            notification.InitiatorName = initiator.Name;

            return await _unitOfWork.GenericNotificationRepository.Create(notification);
        }
    }
}
