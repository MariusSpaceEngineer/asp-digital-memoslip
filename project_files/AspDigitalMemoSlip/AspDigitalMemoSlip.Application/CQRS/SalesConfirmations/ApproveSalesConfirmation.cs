using AspDigitalMemoSlip.Application.Commands;
using AspDigitalMemoSlip.Application.CQRS.Notifications;
using AspDigitalMemoSlip.Application.Exceptions;
using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AutoMapper;
using DTOClassLibrary.DTO;
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
    public class ApproveSalesConfirmationCommand : IRequest<SalesConfirmationDTO>
    {
        public int SalesConfirmationId { get; set; }
        public string RoleInitiator { get; set; }

    }
    public class CreateSalesConfirmationCommandHandler : IRequestHandler<ApproveSalesConfirmationCommand, SalesConfirmationDTO>
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

        public async Task<SalesConfirmationDTO> Handle(ApproveSalesConfirmationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                SalesConfirmation salesConfirmation = await uow.SalesConfirmationRepository.GetSalesConfirmationById(request.SalesConfirmationId);
                
                if (salesConfirmation != null) 
                {
                    if (request.RoleInitiator == "Consigner")
                    {
                            salesConfirmation.SalesConfirmationState = SalesConfirmationState.APPROVED;

                    }
                    else if (request.RoleInitiator == "Consignee")
                    {
                        if (salesConfirmation.SalesConfirmationState == SalesConfirmationState.CONFIRMCONSIGNER
                            || salesConfirmation.SalesConfirmationState == SalesConfirmationState.EDITEDBYCONSIGNER)
                        {
                            salesConfirmation.SalesConfirmationState = SalesConfirmationState.APPROVED;


                            //create notification and send
                            var notificationCommand = new CreateNotificationCommand(
                            eventType: GenericNotiType.PRODUCTSTATECHANGED,
                            message: "Sales confirmation approved successfully.",
                            receiverId: salesConfirmation.ConsignerId,
                            initiatorId: salesConfirmation.ConsigneeId
                        );

                            await mediator.Send(notificationCommand);
                        }

                    }

                    await uow.SalesConfirmationRepository.Update(salesConfirmation);
                    await uow.Commit();
                }

                return mapper.Map<SalesConfirmationDTO>(salesConfirmation);
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
    }

}
