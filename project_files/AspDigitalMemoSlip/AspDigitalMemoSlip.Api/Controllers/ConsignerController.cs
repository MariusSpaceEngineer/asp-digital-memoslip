using AspDigitalMemoSlip.Application.CQRS.Consigner;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AspDigitalMemoSlip.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsignerController : ControllerBase
    {
        private readonly MemoSlipContext _context;
        private readonly IMediator mediator;

        public ConsignerController(MemoSlipContext context, IMediator mediator)
        {
            _context = context;

            this.mediator = mediator;
        }

        [Authorize]
        [HttpGet("GetAllConsigners")]
        public async Task<IActionResult> GetAllConsigners()
        {
            var people = await _context.Consigners.ToListAsync();

            return Ok(people);
        }

        [HttpGet("GetConsignerById/{id:int}")]
        public async Task<IActionResult> GetConsignerById(int id)
        {
            var consigner = await _context.Consigners.FindAsync(id);

            return Ok(consigner);
        }

        [Authorize (Roles = "Consigner")]
        [HttpGet("GetAllPendingConsignees")]
        public async Task<IActionResult> GetAllPendingConsignees()
        {
            // Get the current user's Id
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetAllPendingConsigneesByConsignerQuery(userId);

            // Send the query to MediatR
            var consignees = await mediator.Send(query);

            return Ok(consignees);
        }



    }
}
