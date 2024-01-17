using AspDigitalMemoSlip.Application.CQRS.SalesConfirmations;
using AspDigitalMemoSlip.Application.Interfaces;
using DTOClassLibrary.DTO.ProductSale;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.CQRS.Validators.SalesConfirmation
{
    public class UpdateSalesConfirmationValidator : AbstractValidator<UpdateSalesConfirmation>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSalesConfirmationValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(c => c.SalesConfirmationDTO)
                .NotNull().WithMessage("SalesConfirmation can't be empty");

            RuleFor(c => c.SalesConfirmationDTO.Id)
                .NotEmpty().WithMessage("Sales Confirmation can't be null")
                .MustAsync(CheckIfSalesConfirmationExists)
                .WithMessage("Sales Confirmation must exist.");

            

            RuleFor(c => c.RoleInitiator)
                .NotEmpty().WithMessage("missing role")
                .Must(role => role == "Consignee" || role == "Consigner")
                .WithMessage("missing role initiator.");

        }


        private async Task<bool> CheckIfSalesConfirmationExists(int salesConfirmationId, CancellationToken cancellationToken)
        {
            var salesConfirmation = await _unitOfWork.SalesConfirmationRepository.GetSalesConfirmationById(salesConfirmationId);
            return salesConfirmation != null;
        }

        private async Task<bool> CheckIfAProductSaleExists(ProductSaleDTO productSaleDto, CancellationToken cancellationToken)
        {
            var productSale = await _unitOfWork.ProductSaleRepository.GetById(productSaleDto.Id);
            return productSale != null && productSale.SalesConfirmationId == productSaleDto.SalesConfirmationId;
        }
    }
}
