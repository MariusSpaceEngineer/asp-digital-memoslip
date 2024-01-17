using AspDigitalMemoSlip.Application.CQRS.MemoSlips;
using AspDigitalMemoSlip.Domain;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using AspDigitalMemoSlip.Infrastructure.UoW;
using AutoMapper;
using Azure.Core;
using DTOClassLibrary.DTO.Memo;
using DTOClassLibrary.DTO.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;

namespace AspDigitalMemoSlip.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemoController : ControllerBase
    {
        private readonly MemoSlipContext _context;
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public MemoController(MemoSlipContext context, IMediator mediator, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("products/{id:int}")]
        public async Task<IActionResult> GetMemoWithProducts(int id)
        {
            try
            {
                return Ok(await _mediator.Send(new GetMemoByIdWithProductsQuery { MemoId = id }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("memo")]
        public async Task<IActionResult> GetMemoWithProductsByConsignee()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = userIdClaim.Value;
            var result = await _mediator.Send(new GetMemoWithProductsByConsigneeQuery { ConsigneeId = userId });
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllMemos()
        {
            var result = await _mediator.Send(new GetAllMemoQuery());
            return Ok(JsonSerializer.Serialize(result));
        }

        [HttpGet("byConsignee/{id}")]
        public async Task<IActionResult> GetMemosByConsignee(string id)
        {
            var result = await _mediator.Send(new GetMemoSlipsByConsigneeQuery(id));
            return Ok(JsonSerializer.Serialize(result));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemoById(int id)
        {
            return Ok(await _mediator.Send(new GetMemoByIdWithProductsQuery { MemoId = id }));
        }

        [Authorize(Roles = "Consigner")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MemoDTO memoDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = userIdClaim.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized();
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, memoDTO.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Invalid password.");
            }

            var memo = new MemoDTO
            {
                TermsAccepted = memoDTO.TermsAccepted,
                ConsignerId = memoDTO.ConsignerId,
                ConsigneeId = memoDTO.ConsigneeId,
                Products = memoDTO.Products.Select(p => new ProductDTO
                {
                    LotNumber = p.LotNumber,
                    Description = p.Description,
                    Carat = p.Carat,
                    Price = p.Price,
                    Remarks = p.Remarks,
                    ConsignerId = memoDTO.ConsignerId,
                    ConsigneeId = memoDTO.ConsigneeId
                }).ToList()
            };

            await _mediator.Send(new CreateMemoCommand(memo));
            return Ok();
        }

        [Authorize(Roles = "Consignee")]
        [HttpPost("accept")]
        public async Task<IActionResult> AcceptMemo([FromBody] AcceptMemoDTO model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = userIdClaim.Value;
            await _mediator.Send(new AcceptMemoCommand(userId, model));
            return Ok();
        }

        [Authorize(Roles = "Consignee")]
        [HttpPost("{id}/decline")]
        public async Task<IActionResult> DeclineMemo(int id)
        {
            await _mediator.Send(new DeleteMemoCommand(id));
            return Ok();
        }
    }
}
