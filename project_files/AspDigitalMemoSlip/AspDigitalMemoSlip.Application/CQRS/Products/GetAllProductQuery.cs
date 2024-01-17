using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using DTOClassLibrary.DTO.Product;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.Products
{
    public class GetAllProductQuery : IRequest<IEnumerable<ProductDTO>>
    {
        public int PageNr { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IEnumerable<ProductDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetAllProductQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            return mapper.Map<IEnumerable<ProductDTO>>(await uow.ProductRepository.GetAll(request.PageNr, request.PageSize));
        }
    }
}
