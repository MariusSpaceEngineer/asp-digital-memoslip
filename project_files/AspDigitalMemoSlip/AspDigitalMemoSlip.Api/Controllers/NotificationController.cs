using AspDigitalMemoSlip.Application.CQRS.MemoSlips;
using AspDigitalMemoSlip.Application.CQRS.Notifications;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using AspDigitalMemoSlip.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AspDigitalMemoSlip.Api.Controllers
{
    public class NotificationController : ControllerBase
    {
        private readonly IMediator mediator;
        public NotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("notifications")]
        [Authorize]
        public async Task<IActionResult> GetNotificationsForConsigner()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim.Value;
                var notifications = await this.mediator.Send(new GetAllNotificationsForConsignerQuery() { ConsginerId = userId });
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
