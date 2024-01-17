using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using DTOClassLibrary.DTO;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.SalesConfirmations
{
    public class GetAllSalesConfirmationsQuery : IRequest<IEnumerable<SalesConfirmationDTO>>
    {
        public string UserId { get; set; }
    }
    public class GetAllSalesConfirmationQueryHandler : IRequestHandler<GetAllSalesConfirmationsQuery, IEnumerable<SalesConfirmationDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetAllSalesConfirmationQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<SalesConfirmationDTO>> Handle(GetAllSalesConfirmationsQuery request, CancellationToken cancellationToken)
        {
            var salesConfirmations = await uow.SalesConfirmationRepository
            .GetAllSalesConfirmationsForUser(request.UserId);

            var salesConfirmationDTOs = mapper.Map<IEnumerable<SalesConfirmationDTO>>(salesConfirmations);

            foreach (var sales in salesConfirmationDTOs)
            {
                if (sales.ConsigneeId != null)
                {
                    var consigneeFromSalesConfirmation = await uow.ConsigneeRepository.GetById(sales.ConsigneeId);
                    sales.ConsigneeName = consigneeFromSalesConfirmation.Name;
                }
            }
            return salesConfirmationDTOs;
        }
    }
}
