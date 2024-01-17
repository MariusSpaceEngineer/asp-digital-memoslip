using AspDigitalMemoSlip.Application.CQRS.CQRSInvoice;
using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using DTOClassLibrary.DTO.Invoice;
using DTOClassLibrary.DTO.Memo;
using DTOClassLibrary.DTO.Notification;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.Notifications
{
    public class GetAllNotificationsForConsignerQuery : IRequest<IEnumerable<NotificationDTO>>
    {
        public string ConsginerId { get; set; }
    }

    public class GetAllNotificationsForConsignerQueryHandler : IRequestHandler<GetAllNotificationsForConsignerQuery, IEnumerable<NotificationDTO> >
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;


        public GetAllNotificationsForConsignerQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<NotificationDTO>> Handle(GetAllNotificationsForConsignerQuery request, CancellationToken cancellationToken)
        {
            return mapper.Map<IEnumerable<NotificationDTO>>(await this.uow.GenericNotificationRepository.GetAllNotificationUser(request.ConsginerId));
        }
    }
}

