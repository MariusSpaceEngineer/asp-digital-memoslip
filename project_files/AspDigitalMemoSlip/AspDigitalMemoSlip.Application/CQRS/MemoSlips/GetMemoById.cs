using AspDigitalMemoSlip.Application.Exceptions;
using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using DTOClassLibrary.DTO.Memo;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.MemoSlips
{
    public class GetMemoByIdQuery : IRequest<MemoDTO>
    {
        public int Id { get; set; }
    }

    public class GetMemoByIdQueryHandler : IRequestHandler<GetMemoByIdQuery, MemoDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetMemoByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<MemoDTO> Handle(GetMemoByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var memo = await uow.MemoRepository.GetById(request.Id);
                return mapper.Map<MemoDTO>(memo);
            }
            catch (EntityNotFoundException ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
    }
}
