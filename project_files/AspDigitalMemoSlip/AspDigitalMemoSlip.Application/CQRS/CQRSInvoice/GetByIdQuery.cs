using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using DTOClassLibrary.DTO.Invoice;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.CQRSInvoice
{
    public class GetByIdQuery : IRequest<InvoiceDTO>
    {
        public int Id { get; set; }
    }

    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, InvoiceDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public async Task<InvoiceDTO> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            return mapper.Map<InvoiceDTO>(await uow.InvoiceRepository.GetById(request.Id));
        }
    }
}
