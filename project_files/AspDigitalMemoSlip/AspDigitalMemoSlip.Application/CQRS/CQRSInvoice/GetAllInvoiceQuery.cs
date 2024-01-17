using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using DTOClassLibrary.DTO.Invoice;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.CQRSInvoice
{
    public class GetAllInvoiceQuery : IRequest<IEnumerable<InvoiceDTO>>
    {
        public int PageNr { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllInvoiceQueryHandler : IRequestHandler<GetAllInvoiceQuery, IEnumerable<InvoiceDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetAllInvoiceQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<InvoiceDTO>> Handle(GetAllInvoiceQuery request, CancellationToken cancellationToken)
        {

            var invoices = await uow.InvoiceRepository.GetAll(request.PageNr, request.PageSize);

            // Fetch consignee and consigner details for each invoice
            foreach (var invoice in invoices)
            {
                invoice.Consignee = await uow.ConsigneeRepository.GetById(invoice.ConsigneeId);
                invoice.Consigner = await uow.ConsignerRepository.GetById(invoice.ConsignerId);
            }

            return mapper.Map<IEnumerable<InvoiceDTO>>(invoices);

            // return mapper.Map<IEnumerable<InvoiceDTO>>(await uow.InvoiceRepository.GetAll(request.PageNr, request.PageSize));
        }
    }
}
