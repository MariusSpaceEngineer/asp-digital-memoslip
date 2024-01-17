
using AspDigitalMemoSlip.Application.CQRS.Notifications;
using AspDigitalMemoSlip.Application.Exceptions;
using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AutoMapper;
using DTOClassLibrary.DTO;
using DTOClassLibrary.DTO.Invoice;
using DTOClassLibrary.DTO.Product;
using DTOClassLibrary.DTO.SalesConfirmation;
using DTOClassLibrary.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using static AspDigitalMemoSlip.Domain.ProductSale;

namespace AspDigitalMemoSlip.Application.Commands

{
    public class CreateSalesConfirmationCommand : IRequest<SalesConfirmationDTO>
    {
        public SalesConfirmationDTO SalesConfirmation { get; set; }
        public string ConsigneeId { get; set; }
        public string? ConsingerId { get; set; }

    }
    public class CreateSalesConfirmationCommandHandler : IRequestHandler<CreateSalesConfirmationCommand, SalesConfirmationDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IMediator mediator;


        public CreateSalesConfirmationCommandHandler(IUnitOfWork uow, IMapper mapper, IMediator mediator)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        public async Task<SalesConfirmationDTO> Handle(CreateSalesConfirmationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Consigner consigner = await uow.ConsignerRepository.GetConsginerByConsigneeId(request.ConsigneeId);
                request.SalesConfirmation.SalesConfirmationState = SalesConfirmationState.CREATED;
                var createdSalesConfirmation = await uow.SalesConfirmationRepository.Create(consigner.Id,request.ConsigneeId,request.SalesConfirmation);
                await uow.Commit();


                await UpdateProductsAndCreateSalesAsync(request, createdSalesConfirmation);

                var notificationCommand = new CreateNotificationCommand(
                eventType: GenericNotiType.SALESCONFCREATED,
                message: "Sales confirmation created successfully.",
                receiverId: consigner.Id,
                initiatorId: request.ConsigneeId
                );

                await mediator.Send(notificationCommand);

                return createdSalesConfirmation;
            }
            catch (RelationNotFoundException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        private async Task UpdateProductsAndCreateSalesAsync(CreateSalesConfirmationCommand request, SalesConfirmationDTO salesConfirmation)
        {
            var productsToUpdate = request.SalesConfirmation.SoldProducts;

            foreach (var productSale in productsToUpdate)
            {
                productSale.SalesConfirmationId = salesConfirmation.Id;

                var newProductSale = new ProductSale
                {
                    CaratsSold = productSale.CaratsSold,
                    ProductId = productSale.ProductId,
                    SalePrice = productSale.SalePrice,
                    SalesConfirmationId = salesConfirmation.Id,
                    AgreementStates = AgreementState.SuggestedPrice
                };

                await uow.ProductSaleRepository.Create(newProductSale);
                await uow.ProductRepository.UpdateCaratOfAProduct(newProductSale);
            }

            await uow.Commit();
        }
    }
}
