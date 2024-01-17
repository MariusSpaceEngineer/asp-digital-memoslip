using AspDigitalMemoSlip.Application.CQRS.Consignees;
using AspDigitalMemoSlip.Application.CQRS.Consigner;
using DTOClassLibrary.DTO.Consignee;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.CQRS.Validators.Consignee
{
    public class ConsigneeDTOValidator : AbstractValidator<ConsigneeDTO>
    {
        private readonly IMediator _mediator;

        public ConsigneeDTOValidator(IMediator mediator)
        {
            _mediator = mediator;

            RuleFor(consignee => consignee.Id)
                .NotEmpty().WithMessage("Consignee ID is required.")
                .MustAsync(async (consigneeId, cancellation) => await BeAValidConsignee(consigneeId, cancellation))
                .WithMessage("Consignee ID does not exist in the database.");

        }
        private async Task<bool> BeAValidConsignee(string consigneeId, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new ConsigneeExistsQuery(consigneeId), cancellationToken);
        }
    }
}
