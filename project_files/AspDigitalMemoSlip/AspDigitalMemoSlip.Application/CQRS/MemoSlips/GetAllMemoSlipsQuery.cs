using AspDigitalMemoSlip.Application.CQRS.CQRSInvoice;
using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using DTOClassLibrary.DTO.Consigner;
using DTOClassLibrary.DTO.Memo;
using DTOClassLibrary.DTO.Product;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.MemoSlips
{
    public class GetAllMemoQuery : IRequest<IEnumerable<MemoDTO>>
    {
        public int PageNr { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllMemoQueryHandler : IRequestHandler<GetAllMemoQuery, IEnumerable<MemoDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetAllMemoQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MemoDTO>> Handle(GetAllMemoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var memos = mapper.Map<IEnumerable<MemoDTO>>(await uow.MemoRepository.GetAll());

                foreach (var mem in memos)
                {
                    Console.WriteLine("Looking for products associated with MemoId: " + mem.Id);
                    var products = await uow.ProductRepository.GetProductsByMemoId(mem.Id);
                    mem.Products = (List<ProductDTO>)mapper.Map<IEnumerable<ProductDTO>>(products);

                    Console.WriteLine("Looking for consignee associated with MemoId: " + mem.Id);
                    var consignee = await uow.ConsigneeRepository.GetConsigneeByMemoId(mem.Id);
                    mem.Consignee = mapper.Map<DTOClassLibrary.DTO.Consignee.ConsigneeDTO>(consignee);

                    var consigner = await uow.ConsignerRepository.GetConsingerByMemoId(mem.Id);
                    mem.Consigner = mapper.Map<ConsignerDTO>(consigner);


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
