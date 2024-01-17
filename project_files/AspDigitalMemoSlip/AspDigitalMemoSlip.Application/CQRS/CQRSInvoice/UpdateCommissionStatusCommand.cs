using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.CQRSInvoice
{
    public class UpdateCommissionStatusCommand : IRequest<Invoice>
    {
        public int InvoiceId { get; set; }
        public Domain.Status Status { get; set; } // Use the domain's Status enum here
    }
    public class UpdateCommissionStatusCommandHandler : IRequestHandler<UpdateCommissionStatusCommand, Invoice>
    {
        private readonly IUnitOfWork uow;

        public UpdateCommissionStatusCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Invoice> Handle(UpdateCommissionStatusCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The provided command object is null.");
            }

            // Retrieve the existing invoice from the database
            var existingInvoice = await uow.InvoiceRepository.GetById(request.InvoiceId);

            if (existingInvoice == null)
            {
                throw new ArgumentException($"Invoice with ID {request.InvoiceId} not found.", nameof(request.InvoiceId));
            }

            // Update the commission status
            existingInvoice.CommisionStatus = request.Status;


            uow.InvoiceRepository.Update(existingInvoice);
            await uow.Commit();

            return existingInvoice;
        }
    }
}
