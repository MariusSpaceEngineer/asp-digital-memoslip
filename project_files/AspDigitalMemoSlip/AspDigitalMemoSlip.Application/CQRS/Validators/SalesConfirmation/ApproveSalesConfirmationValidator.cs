using AspDigitalMemoSlip.Application.Commands;
using AspDigitalMemoSlip.Application.CQRS.SalesConfirmations;
using AspDigitalMemoSlip.Application.Interfaces;
using DTOClassLibrary.DTO.SalesConfirmation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.CQRS.Validators.SalesConfirmation
{
    public class ApproveSalesConfirmationCommandValidator : AbstractValidator<ApproveSalesConfirmationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApproveSalesConfirmationCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(s => s.SalesConfirmationId)
                .MustAsync(async (id, cancellation) => await CheckIfSalesConfirmationExists(id))
                .WithMessage("No salesconfirmation with this id exists");

            RuleFor(s => s.RoleInitiator)
                .NotEmpty().WithMessage("Role Initiator is required.")
                .Must(role => role == "Consigner" || role == "Consignee")
                .WithMessage("Not the correct role for approval");

            RuleFor(s => s)
                .MustAsync(async (command, cancellation) => await CheckIfPossibleForApproval(command.SalesConfirmationId, command.RoleInitiator))
                .WithMessage("Salesconfirmation is not in the correct state");
        }

        private async Task<bool> CheckIfSalesConfirmationExists(int id)
        {
            var salesConfirmation = await _unitOfWork.SalesConfirmationRepository.GetSalesConfirmationById(id);
            return salesConfirmation != null;
        }

        private async Task<bool> CheckIfPossibleForApproval(int id, string roleInitiator)
        {
            var salesConfirmation = await _unitOfWork.SalesConfirmationRepository.GetSalesConfirmationById(id);
            if (salesConfirmation == null) return false;

            if (roleInitiator == "Consigner")
            {
                return salesConfirmation.SalesConfirmationState != SalesConfirmationState.APPROVED;
            }

            if (roleInitiator == "Consignee")
            {
                return salesConfirmation.SalesConfirmationState == SalesConfirmationState.CONFIRMCONSIGNER
                    || salesConfirmation.SalesConfirmationState == SalesConfirmationState.EDITEDBYCONSIGNER;
            }

            return false;
        }
    }

}

