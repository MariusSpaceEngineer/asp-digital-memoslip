using AspDigitalMemoSlip.Application.Commands;
using AspDigitalMemoSlip.Application.Exceptions;
using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AutoMapper;
using DTOClassLibrary.DTO;
using DTOClassLibrary.DTO.Product;
using DTOClassLibrary.DTO.SalesConfirmation;
using DTOClassLibrary.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.CQRS.SalesConfirmations
{
    public class UpdateSalesConfirmation :IRequest<SalesConfirmationDTO>
    {
        public SalesConfirmationDTO SalesConfirmationDTO { get; set; }
        public string RoleInitiator { get; set; }
    }

    public class UpdateSalesConfirmationCommandHandler : IRequestHandler<UpdateSalesConfirmation, SalesConfirmationDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;


        public UpdateSalesConfirmationCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<SalesConfirmationDTO> Handle(UpdateSalesConfirmation request, CancellationToken cancellationToken)
        {
            var salesConfirmationDTO = request.SalesConfirmationDTO;



            foreach (var productDto in salesConfirmationDTO.SoldProducts)
            {
                var existingProduct = await this.uow.ProductSaleRepository.GetById(productDto.Id);
                if (existingProduct != null)
                {
                    
                    existingProduct.SalesConfirmationId = salesConfirmationDTO.Id;
                    
                    if (existingProduct.SalePrice != productDto.SalePrice)
                    {
                        existingProduct.SalePrice = productDto.SalePrice;
                        existingProduct.AgreementStates = AgreementState.CounterPrice;
                    }
                    else
                    {
                        existingProduct.AgreementStates = AgreementState.Agreed;
                    }


                    
                    this.uow.ProductSaleRepository.Update(existingProduct);
                    await this.uow.Commit();
                }
                else
                {
                    // throw exception
                }
            }

            var salesConfiration = await this.uow.SalesConfirmationRepository.GetSalesConfirmationById(salesConfirmationDTO.Id);

            salesConfiration.SuggestedCommision = request.SalesConfirmationDTO.SuggestedCommision;

            if (request.RoleInitiator == "Consignee")
            {
                salesConfiration.SalesConfirmationState = SalesConfirmationState.EDITEDBYCONSIGNEE;

            }
            if(request.RoleInitiator == "Consigner")
            {
                salesConfiration.SalesConfirmationState = SalesConfirmationState.EDITEDBYCONSIGNER;

            }
            await this.uow.SalesConfirmationRepository.Update(salesConfiration);
            return salesConfirmationDTO;
        }
    }
}
