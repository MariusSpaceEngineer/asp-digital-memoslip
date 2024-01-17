using AspDigitalMemoSlip.Application.CQRS.Authentication;
using AspDigitalMemoSlip.Application.Utils.Authentication;
using DTOClassLibrary.DTO.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspDigitalMemoSlip.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IMediator mediator, ILogger<AuthenticationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            string originUrl = $"{Request.Headers["Origin"]}";

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var command = new LoginCommand(model, originUrl);
            var result = await _mediator.Send(command);

            // Add the JWT token to the Authorization header
            Response.Headers.Add("Authorization", $"Bearer {result.Token}");
            return Ok();
        }

        [HttpPost]
        [Route("{consignerUserName}/register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromRoute] string consignerUserName, [FromForm] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var command = new RegisterCommand(consignerUserName, model, new List<string> { UserRole.Consignee });
            var result = await _mediator.Send(command);
          
            return CreatedAtAction(nameof(Register), model);
        }

        [HttpGet]
        [Route("consignee/images/{id}")]
        [Authorize(Roles = "Consignee, Consigner")]
        public async Task<IActionResult> GetUserImages(string id)
        {
                var query = new GetAllUserImagesQuery(id);
                var result = await _mediator.Send(query);

                return Ok(result);
        }
          

        [HttpPut]
        [Route("consignee/mfa")]
        [Authorize(Roles ="Consignee")]
        public async Task<IActionResult> UpdateMFA()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var userId = userIdClaim.Value;

                var command = new UpdateMFACommand(userId);
                var result = await _mediator.Send(command);

                return Ok(result.Message);

            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("consigner/qrcode")]
        [Authorize(Roles = "Consigner")]
        public async Task<IActionResult> GenerateQRCode()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var userId = userIdClaim.Value;

                var command = new GenerateConsignerQRCodeCommand(userId);
                var result = await _mediator.Send(command);

                return Ok(result);

            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
