using AspDigitalMemoSlip.Application.CQRS.CQRSInvoice;
using AspDigitalMemoSlip.Domain;
using AutoMapper;
using DTOClassLibrary.DTO.Invoice;

namespace AspDigitalMemoSlip.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Invoice, InvoiceDTO>();    
        }
    }
}