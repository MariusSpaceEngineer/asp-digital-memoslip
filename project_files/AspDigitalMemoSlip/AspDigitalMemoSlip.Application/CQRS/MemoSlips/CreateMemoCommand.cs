using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.DTO.Consignee;
using DTOClassLibrary.DTO.Consigner;
using DTOClassLibrary.DTO.Memo;
using DTOClassLibrary.DTO.Product;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.MemoSlips
{
    public class CreateMemoCommand : IRequest<MemoDTO>
    {
        public MemoDTO Memo { get; set; }

        public CreateMemoCommand(MemoDTO memo)
        {
            Memo = memo;
        }
    }

    public class CreateMemoCommandHandler : IRequestHandler<CreateMemoCommand, MemoDTO>
    {
        private readonly IUnitOfWork uow;

        public CreateMemoCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<MemoDTO> Handle(CreateMemoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var memo = new Memo
                {
                    Products = request.Memo.Products.Select(p => new Product
                    {
                        Id = p.ID,
                        Price = p.Price,
                        ConsignerId = request.Memo.ConsignerId,
                        ConsigneeId = request.Memo.ConsigneeId,
                        Description = p.Description,
                        LotNumber = p.LotNumber,
                        Carat = p.Carat,
                        Remarks = p.Remarks
                    }).ToList(),
                    Consigner = uow.ConsignerRepository.GetById(request.Memo.ConsignerId).Result,
                    Consignee = uow.ConsigneeRepository.GetById(request.Memo.ConsigneeId).Result,
                    TermsAccepted = request.Memo.TermsAccepted,
                    AcceptedByConsignee = false,
                    ConsigneeId = request.Memo.ConsigneeId,
                    ConsignerId = request.Memo.ConsignerId
                };

                

                await uow.MemoRepository.Create(memo);
                await uow.Commit();

                var memoDto = new MemoDTO
                {
                    
                };

                return memoDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
