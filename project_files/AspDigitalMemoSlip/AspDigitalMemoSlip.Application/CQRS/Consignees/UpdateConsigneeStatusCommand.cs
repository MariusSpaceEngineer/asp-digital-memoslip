using AspDigitalMemoSlip.Application.Exceptions;
using AspDigitalMemoSlip.Application.Exceptions.Authentication;
using AspDigitalMemoSlip.Application.Exceptions.Email;
using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Application.Interfaces.Services;
using AspDigitalMemoSlip.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AspDigitalMemoSlip.Application.CQRS.Consignees
{
    public class UpdateConsigneeStatusCommand : IRequest<bool>
    {
        public string ConsigneeId { get; set; }

        public UpdateConsigneeStatusCommand(string consigneeId)
        {
            ConsigneeId = consigneeId;
        }
    }
    public class UpdateConsigneeStatusCommandHandler : IRequestHandler<UpdateConsigneeStatusCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _emailService;
        protected readonly UserManager<User> _userManager;

        public UpdateConsigneeStatusCommandHandler(IUnitOfWork uow, IEmailService emailService, UserManager<Domain.User> userManager)
        {
            _uow = uow;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<bool> Handle(UpdateConsigneeStatusCommand request, CancellationToken cancellationToken)
        {
            var consignee = await GetConsignee(request.ConsigneeId);
            var result = await UpdateConsigneeStatus(request.ConsigneeId);
            await SendConfirmationEmail(consignee.Email, consignee.Name);
            return result;
        }

        private async Task<User> GetConsignee(string consigneeId)
        {
            var consignee = await _userManager.FindByIdAsync(consigneeId);
            if (consignee == null)
            {
                throw new UserNotFoundException($"User with ID {consigneeId} not found.");
            }
            return consignee;
        }

        private async Task<bool> UpdateConsigneeStatus(string consigneeId)
        {
            try
            {
                var result = await _uow.ConsigneeRepository.UpdateConsigneeAcceptedByConsignerToTrue(consigneeId);
                if (!result)
                {
                    throw new UpdateFailedException("Failed to update consignee status.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the consignee status.", ex);
            }
        }

        private async Task SendConfirmationEmail(string to, string recipientName)
        {
            string subject = "Hooray, you are in!";
            string body = "Your account has been accepted, you can now log in and start trading!";
            try
            {
                await _emailService.SendEmailAsync(to, recipientName, subject, body);
            }
            catch (Exception ex)
            {
                throw new EmailSendingException("Failed to send confirmation email.", ex);
            }
        }
    }
}

