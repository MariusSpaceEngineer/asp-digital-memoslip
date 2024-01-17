using AspDigitalMemoSlip.Application.CQRS.Notifications;
using AspDigitalMemoSlip.Application.CQRS.Products;
using AspDigitalMemoSlip.Application.Exceptions;
using DTOClassLibrary.DTO.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspDigitalMemoSlip.Api.Controllers
{
    public class ProductController : Controller
    {
        

        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {

            this.mediator = mediator;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct([FromQuery] int pageNr = 1, [FromQuery] int pageSize = 10)
        {

            return Ok(await mediator.Send(new GetAllProductQuery() { PageNr = pageNr, PageSize = pageSize }));
        }

        [HttpPatch]
        [Route("UpdateSoldStatus/{id}")]
        public async Task<IActionResult> UpdateSoldStatus(int id, [FromBody] UpdateSoldStatusDev updateDTO)
        {
            try
            {
                // Check if the 'updateDTO' object is null
                if (updateDTO == null)
                {
                    return BadRequest("Invalid data. The 'updateDTO' object is null.");
                }

                // Send the command to the mediator
                var command = new UpdateSoldStatusDev { ProductId = id, Status = updateDTO.Status };
                var result = await mediator.Send(command);

                // Check if the result is null or handle specific conditions
                if (result == null)
                {
                    return BadRequest("Invalid result. The operation did not succeed.");
                }

                // Return a 200 OK response with the result
                return Ok(result);
            }
            catch (RelationNotFoundException ex)
            {
                // Return a 400 Bad Request response with the exception message
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch]
        [Route("UpdateSoldStatusCounter/{id}")]
        public async Task<IActionResult> UpdateSoldStatusCounter(int id, [FromBody] UpdateSoldStatusCounter updateDTO)
        {
            try
            {
                // Check if the 'updateDTO' object is null
                if (updateDTO == null)
                {
                    return BadRequest("Invalid data. The 'updateDTO' object is null.");
                }

                // Send the command to the mediator
                var command = new UpdateSoldStatusCounter { ProductId = id, Status = updateDTO.Status, SuggestedPrice = updateDTO.SuggestedPrice, CommisionPrice = updateDTO.CommisionPrice, CommissionPaidBy = updateDTO.CommissionPaidBy };
                var result = await mediator.Send(command);

                // Check if the result is null or handle specific conditions
                if (result == null)
                {
                    return BadRequest("Invalid result. The operation did not succeed.");
                }

                // Return a 200 OK response with the result
                return Ok(result);
            }
            catch (RelationNotFoundException ex)
            {
                // Return a 400 Bad Request response with the exception message
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("UpdateSoldStatusBroker/{id}")]
        public async Task<IActionResult> UpdateSoldStatusBroker(int id, [FromBody] UpdateSoldStatusBroker updateDTO)
        {
            try
            {


                // Check if the 'updateDTO' object is null
                if (updateDTO == null)
                {
                    return BadRequest("Invalid data. The 'updateDTO' object is null.");
                }

                // Send the command to the mediator
                var command = new UpdateSoldStatusBroker { ProductId = id, Status = updateDTO.Status, CommissionPaidBy = updateDTO.CommissionPaidBy };
                var result = await mediator.Send(command);

                // Check if the result is null or handle specific conditions
                if (result == null)
                {
                    return BadRequest("Invalid result. The operation did not succeed.");
                }

                // Return a 200 OK response with the result
                return Ok(result);
            }
            catch (RelationNotFoundException ex)
            {
                // Return a 400 Bad Request response with the exception message
                return BadRequest(ex.Message);
            }
        }


        private async Task ProductChangedNotificationAsync(string userId, ProductDTO obj)
        {
            await mediator.Publish(new ProductStateNotification(userId, obj));
        }

        [HttpPut("products/state")]
        [Authorize]
        public async Task<IActionResult> UpdateProductState([FromBody] ProductStateChangeDTO productStateUpdate)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    return Unauthorized("Er is geen consigner met deze id");
                }
                var userId = userIdClaim.Value;

                var result = await mediator.Send(new UpdateProductCommand() { ProductStateChangeDTO = productStateUpdate });

                //if (result != null)
                //    //await ProductChangedNotificationAsync(userId, result);


                return Ok(result);

            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
