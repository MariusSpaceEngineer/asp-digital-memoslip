using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using DTOClassLibrary.DTO.Consignee;
using DTOClassLibrary.DTO.Consigner;
using DTOClassLibrary.DTO.Memo;
using DTOClassLibrary.DTO.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.CQRS.MemoSlips
{
    public class GetMemoWithProductsByConsigneeQuery : IRequest<IEnumerable<MemoDTO>>
    {
        public string ConsigneeId { get; set; }
    }

    public class GetMemoWithProductsByConsgineeQueryHandler : IRequestHandler<GetMemoWithProductsByConsigneeQuery, IEnumerable<MemoDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetMemoWithProductsByConsgineeQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MemoDTO>> Handle(GetMemoWithProductsByConsigneeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var listMemos = await this.uow.MemoRepository.GetMemosWithProductsByConsginee(request.ConsigneeId);

                return mapper.Map<IEnumerable<MemoDTO>>(listMemos);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
