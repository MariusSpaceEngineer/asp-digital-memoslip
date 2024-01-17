using AspDigitalMemoSlip.Application.Exceptions.Authentication;
using AspDigitalMemoSlip.Application.Exceptions.Email;
using AspDigitalMemoSlip.Application.Exceptions.Memo;
using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Application.Interfaces.Services;
using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.DTO.Memo;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AspDigitalMemoSlip.Application.CQRS.MemoSlips
{
    public class AcceptMemoCommand : IRequest
    {
        public string UserId { get; set; }
        public AcceptMemoDTO AcceptMemoDTO { get; set; }

        public AcceptMemoCommand(string id,AcceptMemoDTO model)
        {
           UserId = id;
           AcceptMemoDTO = model;
        }

        public class AcceptMemoCommandHandler : IRequestHandler<AcceptMemoCommand>
        {
            private readonly UserManager<User> _userManager;
            private readonly IUnitOfWork _uow;
            private readonly IUserTwoFactorTokenProvider<User> _tokenProvider;
            private readonly IEmailService _emailService;


            public AcceptMemoCommandHandler(UserManager<User> userManager, IUnitOfWork uow, IUserTwoFactorTokenProvider<User> tokenProvider, IEmailService emailService)
            {
                _userManager = userManager;
                _uow = uow;
                _tokenProvider = tokenProvider;
                _emailService = emailService;

            }

            public async Task Handle(AcceptMemoCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user != null)
                {
                    var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.AcceptMemoDTO.Password);
                    if (isPasswordValid)
                    {
                        var memo = await GetMemo(request.AcceptMemoDTO.Id);
                        memo.AcceptedByConsignee = true;
                        memo.TermsAccepted = true;
                        await NotifyConsigner(memo, user);
                        await UpdateMemo(memo);
                    }
                    else
                    {
                        throw new InvalidPasswordException("Password incorrect.");
                    }
                }
                else
                {
                    throw new UserNotFoundException("User has not been found.");
                }
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

            private async Task NotifyConsigner(Memo memo, User user)
            {
                var consigner = await _userManager.FindByIdAsync(memo.ConsignerId);
                if (consigner != null)
                {
                    NotifyAcceptedMemo(consigner.Email, consigner.Name, user.Name, memo.Id);
                }
            }

            private async Task UpdateMemo(Memo memo)
            {
                _uow.MemoRepository.Update(memo);
                await _uow.Commit();
            }

            private async Task NotifyAcceptedMemo(string to, string recipientName, string consigneeName, int memoId)
            {
                try
                {
                    string subject = $"Good News! Your Memo #{memoId} Has Been Approved!";
                    string body = $"Dear {recipientName},\n\nWe are pleased to inform you that your memo with ID: {memoId} " +
                        $"has been accepted by {consigneeName}.";

                    await _emailService.SendEmailAsync(to, recipientName, subject, body);
                }
                catch (Exception ex)
                {
                    throw new EmailSendingException("An error occurred while sending the email.", ex);
                }
            }
        }
    }
       
}
