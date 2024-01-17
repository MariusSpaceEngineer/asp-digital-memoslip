
using AspDigitalMemoSlip.Application.CQRS.Authentication;
using AspDigitalMemoSlip.Application.CQRS.Consignees;
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
	public class ConsigneeController : ControllerBase
	{

        private readonly MemoSlipContext _context;
        private readonly IMediator mediator;

        public ConsigneeController(MemoSlipContext context, IMediator mediator)
        {
            _context = context;

            this.mediator = mediator;
        }

        [Authorize(Roles = "Consigner")]
        [HttpGet("GetAllConsignees")]
		public async Task<IActionResult> GetAllConsignees()
		{
			var people = await _context.Consignees.ToListAsync();

            return Ok(people);
        }

        [Authorize(Roles = "Consigner")]
        [HttpGet("GetConsigneeById/{id:int}")]
        public async Task<IActionResult> GetConsigneeById(int id)
        {
            var consignee = await _context.Consignees.FindAsync(id);

            return Ok(consignee);
        }

        [HttpGet("GetConsigneeByUserId/{id}")]
        public async Task<IActionResult> GetConsigneeByUserId(String id)
        {
            var consignee = await mediator.Send(new GetConsigneeByUserIdQuery(id));

            return Ok(consignee);
        }

        [Authorize (Roles ="Consigner")]
        [HttpPost("AcceptConsignee/{consigneeId}")]
        public async Task<IActionResult> AcceptConsignee(String consigneeId)
        {
                var query = new UpdateConsigneeStatusCommand(consigneeId);
                await mediator.Send(query);

                return Ok();
        }

        [HttpPost("DeclineConsignee/{id}")]
        public async Task<IActionResult> DeclineConsignee(String id)
        {
            var result = await mediator.Send(new DeleteConsigneeCommand(id));

            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }



    }
}
