using AspDigitalMemoSlip.Application.CQRS.CQRSInvoice;
using AspDigitalMemoSlip.Application.CQRS.MemoSlips;
using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using DTOClassLibrary.DTO.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.CQRS.Products
{
    public class GetProductsByMemoIdQuery : IRequest<IEnumerable<ProductDTO>>
    {
        public int PageNr { get; set; }
        public int PageSize { get; set; }

        public int MemoId { get; set; }
    }

    public class GetAllMemoQueryHandler : IRequestHandler<GetProductsByMemoIdQuery, IEnumerable<ProductDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetAllMemoQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByMemoIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var products = mapper.Map<IEnumerable<ProductDTO>>(await uow.ProductRepository.GetProductsByMemoId(request.MemoId));



                return products;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;

        }
    }

}
