using AspDigitalMemoSlip.Application.Exceptions.Authentication;
using AspDigitalMemoSlip.Application.Exceptions.Email;
using AspDigitalMemoSlip.Application.Exceptions.Memo;
using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Application.Interfaces.Services;
using AspDigitalMemoSlip.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AspDigitalMemoSlip.Application.CQRS.MemoSlips
{
    public class DeleteMemoCommand : IRequest
    {
        public int MemoId { get; }

        public DeleteMemoCommand(int memoId)
        {
            MemoId = memoId;
        }
    }
    public class DeleteMemoCommandHandler : IRequestHandler<DeleteMemoCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _emailService;

        public DeleteMemoCommandHandler(UserManager<User> userManager, IUnitOfWork uow, IEmailService emailService)
        {
            _userManager = userManager;
            _uow = uow;
            _emailService = emailService;
        }


        public async Task Handle(DeleteMemoCommand request, CancellationToken cancellationToken)
        {
            var memo = await GetMemo(request.MemoId);
            await DeleteAssociatedProducts(request.MemoId);
            var consigner = await GetUser(memo.ConsignerId);
            var consignee = await GetUser(memo.ConsigneeId);
            await NotifyDeclinedMemo(consigner.Email, consignee.Name, consignee.Name, memo.Id);
            await DeleteMemo(memo);
        }

        private async Task<Memo> GetMemo(int id)
        {
            var memo = await _uow.MemoRepository.GetById(id);
            if (memo == null)
            {
                throw new MemoNotFoundException("Memo not found.");
            }
            return memo;
        }

        private async Task<User> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }
            return user;
        }

        private async Task DeleteAssociatedProducts(int memoId)
        {
            var products = await _uow.ProductRepository.GetProductsByMemoId(memoId);
            foreach (var product in products)
            {
                _uow.ProductRepository.Delete(product);
            }
        }

        private async Task DeleteMemo(Memo memo)
        {
            _uow.MemoRepository.Delete(memo);
            await _uow.Commit();
        }

        private async Task NotifyDeclinedMemo(string to, string recipientName, string consigneeName, int memoId)
        {
            try
            {
                string subject = $"Memo #{memoId} Update: Memo Has Been Declined";
                string body = $"Dear {recipientName},\n\nWe regret to inform you that your memo with ID: {memoId} has been declined by {consigneeName} and removed from the system.";

                await _emailService.SendEmailAsync(to, recipientName, subject, body);
            }
            catch (Exception ex)
            {
                throw new EmailSendingException("An error occurred while sending the email.", ex);
            }
        }
    }
}
