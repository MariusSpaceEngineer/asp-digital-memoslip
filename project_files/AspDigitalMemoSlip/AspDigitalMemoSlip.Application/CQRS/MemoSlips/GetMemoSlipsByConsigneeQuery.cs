using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
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
    public class GetMemoSlipsByConsigneeQuery : IRequest<IEnumerable<MemoDTO>>
    {
        public GetMemoSlipsByConsigneeQuery(string id)
        {
            ConsigneeId = id;
        }

        public int PageNr { get; set; }
        public int PageSize { get; set; }

        public string ConsigneeId { get; set; }
    }

    public class GetMemoSlipsByConsigneeQueryHandler : IRequestHandler<GetMemoSlipsByConsigneeQuery, IEnumerable<MemoDTO>>
    {

        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetMemoSlipsByConsigneeQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MemoDTO>> Handle(GetMemoSlipsByConsigneeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var memos = mapper.Map<IEnumerable<MemoDTO>>(await uow.MemoRepository.GetMemosByConsigneeId(request.ConsigneeId));

                foreach (var mem in memos)
                {
                    Console.WriteLine("Looking for products associated with MemoId: " + mem.Id);
                    var products = await uow.ProductRepository.GetProductsByMemoId(mem.Id);
                    mem.Products = (List<ProductDTO>)mapper.Map<IEnumerable<ProductDTO>>(products);

                    Console.WriteLine("Looking for consignee associated with MemoId: " + mem.Id);
                    var consignee = await uow.ConsigneeRepository.GetConsigneeByMemoId(mem.Id);
                    mem.Consignee = mapper.Map<DTOClassLibrary.DTO.Consignee.ConsigneeDTO>(consignee);


                }

                return memos;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

    }
}
