using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using FluentValidation;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.CQRSInvoice
{
    public class AddCommand : IRequest<Invoice>
    {
        public Invoice Invoice { get; private set; }

        // Parameterless constructor for model binding
        public AddCommand()
        {
            // Initialize Invoice to a new instance when using the parameterless constructor
            Invoice = new Invoice();
        }

        public AddCommand(int total, double commision)
        {
            // Set the provided values to the Invoice
            Invoice = new Invoice
            {
                Total = total,
                Commision = commision

            };
        }
    }

    public class AddCommandValidator : AbstractValidator<AddCommand>
    {
        private readonly IUnitOfWork uow;

        public AddCommandValidator(IUnitOfWork uow)
        {
            this.uow = uow;

            RuleFor(command => command.Invoice).NotNull();

        }

        private async Task<bool> ConsigneeExists(string Id, CancellationToken cancellation)
        {
            var consignee = await uow.ConsigneeRepository.GetById(Id);
            return consignee != null;
        }
    }

    public class AddCommandHandler : IRequestHandler<AddCommand, Invoice>
    {
        private readonly IUnitOfWork uow;

        public AddCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Invoice> Handle(AddCommand request, CancellationToken cancellationToken)
        {
            if (request.Invoice == null)
            {
                // Log or throw an exception indicating that the Invoice is null
                throw new ArgumentNullException(nameof(request.Invoice), "The provided invoice object is null.");
            }

            // Make sure MemoId is set before saving

            await uow.InvoiceRepository.Create(request.Invoice);
            await uow.Commit();
            return request.Invoice;
        }
    }
}
