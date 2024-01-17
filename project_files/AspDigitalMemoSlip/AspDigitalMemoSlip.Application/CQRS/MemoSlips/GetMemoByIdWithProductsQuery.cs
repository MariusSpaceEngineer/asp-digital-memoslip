using AspDigitalMemoSlip.Application.CQRS.CQRSInvoice;
using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOClassLibrary.DTO;
using DTOClassLibrary.DTO.Memo;
using DTOClassLibrary.DTO.Consignee;
using DTOClassLibrary.DTO.Consigner;
using DTOClassLibrary.DTO.Product;

namespace AspDigitalMemoSlip.Application.CQRS.MemoSlips
{
    public class GetMemoByIdWithProductsQuery : IRequest<MemoDTO>
    {
        public int MemoId { get; set; }
    }

    public class GetMemoByIdWithProductsQueryHandler : IRequestHandler<GetMemoByIdWithProductsQuery, MemoDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetMemoByIdWithProductsQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<MemoDTO> Handle(GetMemoByIdWithProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var memo = await uow.MemoRepository.GetById(request.MemoId);
                var products = await uow.ProductRepository.GetProductsByMemoId(request.MemoId);
                var consignee = await uow.ConsigneeRepository.GetById(memo.ConsigneeId);
                var consigner = await uow.ConsignerRepository.GetById(memo.ConsignerId);

                var consigneeDto = mapper.Map<ConsigneeDTO>(consignee);
                var consignerDto = mapper.Map<ConsignerDTO>(consigner);
                var memoDto = mapper.Map<MemoDTO>(memo);

                var productDtos = mapper.Map<List<ProductDTO>>(products);

                memoDto.Products = productDtos;
                memoDto.Consignee = consigneeDto;
                memoDto.Consigner = consignerDto;
                return memoDto;
            }
            catch (Exception ex)
            {
               
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
