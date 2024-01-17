using AspDigitalMemoSlip.Application.Commands;
using AspDigitalMemoSlip.Application.CQRS.SalesConfirmations;
using AspDigitalMemoSlip.Application.Exceptions;
using Azure.Core;
using DTOClassLibrary.DTO;
using DTOClassLibrary.DTO.SalesConfirmation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AspDigitalMemoSlip.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesConfirmationController : ControllerBase
    {
        private readonly IMediator mediator;

        public SalesConfirmationController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateSalesConfirmation([FromBody] SalesConfirmationDTO request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    var userId = userIdClaim.Value;
                    var command = new CreateSalesConfirmationCommand() { ConsigneeId = userId, SalesConfirmation = request };
                    var result = await mediator.Send(command);



                    return Created("", result);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (RelationNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSalesConfirmations()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    var userId = userIdClaim.Value;
                    return Ok(await mediator.Send(new GetAllSalesConfirmationsQuery() { UserId = userId }));

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> UpdateStateProductSale([FromBody] SalesConfirmationDTO salesConfirmationDTO)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirst(ClaimTypes.Role);
                if (userIdClaim != null && userRole != null)
                {
                    var userId = userIdClaim.Value;
                    return Ok(await mediator.Send(new UpdateSalesConfirmation() {RoleInitiator = userRole.Value, SalesConfirmationDTO = salesConfirmationDTO }));

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("approve/{id}")]
        [Authorize]
        public async Task<IActionResult> ApproveSalesConfirmation(int id)
        {

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirst(ClaimTypes.Role);
                if (userIdClaim != null)
                {
                    var userId = userIdClaim.Value;
                    var userRoleValue = userRole.Value;
                    return Ok(await mediator.Send(new ApproveSalesConfirmationCommand() { SalesConfirmationId = id, RoleInitiator = userRoleValue }));

                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
