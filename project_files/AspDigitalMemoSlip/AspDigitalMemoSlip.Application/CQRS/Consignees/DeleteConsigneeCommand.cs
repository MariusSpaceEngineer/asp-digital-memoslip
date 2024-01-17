using AspDigitalMemoSlip.Application.Exceptions;
using AspDigitalMemoSlip.Application.Exceptions.Consignee;
using AspDigitalMemoSlip.Application.Interfaces;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.Consignees
{
    public class DeleteConsigneeCommand : IRequest<bool>
    {
        public string UserId { get; set; }

        public DeleteConsigneeCommand(string userId)
        {
            UserId = userId;
        }

        public class DeleteConsigneeCommandHandler : IRequestHandler<DeleteConsigneeCommand, bool>
        {
            private readonly IUnitOfWork uow;

            public DeleteConsigneeCommandHandler(IUnitOfWork uow)
            {
                this.uow = uow;
            }

            public async Task<bool> Handle(DeleteConsigneeCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await uow.ConsigneeRepository.DeleteConsignee(request.UserId);

                    if (!result)
                    {
                        throw new UpdateFailedException("Consignee could not be deleted.");
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw new ConsigneeDeletionFailedException("An error occurred while deleting the consignee.", ex);
                }
            }
        }
    }
}
