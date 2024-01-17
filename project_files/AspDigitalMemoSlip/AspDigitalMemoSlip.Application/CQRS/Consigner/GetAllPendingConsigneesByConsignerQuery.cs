using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using DTOClassLibrary.DTO.Consignee;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.Consigner
{
    public class GetAllPendingConsigneesByConsignerQuery : IRequest<IEnumerable<ConsigneeDTO>>
    {
        public string ConsignerId { get; set; }

        public GetAllPendingConsigneesByConsignerQuery(string id)
        {
            ConsignerId = id;
        }
    }

    public class GetAllPendingConsigneesByConsignerQueryHandler : IRequestHandler<GetAllPendingConsigneesByConsignerQuery, IEnumerable<ConsigneeDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetAllPendingConsigneesByConsignerQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ConsigneeDTO>> Handle(GetAllPendingConsigneesByConsignerQuery request, CancellationToken cancellationToken)
        {
            var consignees = mapper.Map<IEnumerable<ConsigneeDTO>>(await uow.ConsignerRepository.GetPendingConsigneesByConsignerId(request.ConsignerId));
            return consignees;
        }
    }

}
