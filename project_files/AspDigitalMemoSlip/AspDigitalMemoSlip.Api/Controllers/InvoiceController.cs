using AspDigitalMemoSlip.Application.CQRS.CQRSInvoice;
using AspDigitalMemoSlip.Application.Exceptions;
using Mailjet.Client.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace AP.MyGameStore.WebAPI.Controllers
{
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public InvoiceController(IMediator mediator, HttpClient httpClient, IConfiguration configuration)
        {
            this.mediator = mediator;
            _httpClient = httpClient;
            this._configuration = configuration;
        }

        [HttpGet("GetAllInvoice")]
        public async Task<IActionResult> GetAllInvoice([FromQuery] int pageNr = 1, [FromQuery] int pageSize = 10)
        {

            return Ok(await mediator.Send(new GetAllInvoiceQuery() { PageNr = pageNr, PageSize = pageSize }));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetInvoiceById(int id)
        {

            var invoice = await mediator.Send(new GetByIdQuery() { Id = id });
            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }


        //move this to secret config file
        private const string API_KEY = "3ca4e6d8f61acbeb70a4d9b54bc99a8b";
        [HttpGet]
        [Route("GetCurrentConversionRate")]
        public async Task<IActionResult> GetCurrentConversionRate()
        {
            var url = $"http://api.exchangeratesapi.io/v1/latest?access_key={_configuration["ConversionRate:ApiKey"]}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error calling the exchangeratesapi.io API");
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(content);

            if (jsonResponse["success"].Value<bool>() == false)
            {
                return BadRequest("Failed to retrieve conversion rate");
            }

            var rate = jsonResponse["rates"]["USD"].Value<decimal>();

            return Ok(rate);

        }


        [HttpPost]
        [Route("CreateInvoice")]
        public async Task<IActionResult> CreateInvoice(AddCommand command)
        {
            try
            {
                // Check if the 'command' object is null
                if (command == null)
                {
                    return BadRequest("Invalid data. The 'command' object is null.");
                }

                // Send the command to the mediator
                var result = await mediator.Send(command);

                // Check if the result is null or handle specific conditions
                if (result == null)
                {
                    return BadRequest("Invalid result. The operation did not succeed.");
                }

                // Return a 201 Created response with the result
                return CreatedAtAction(nameof(GetInvoiceById), new { id = result.Id }, result);
            }
            catch (RelationNotFoundException ex)
            {
                // Return a 400 Bad Request response with the exception message
                return BadRequest(ex.Message);
            }
        }





        [HttpPatch]
        [Route("UpdateCommissionStatus/{id}")]
        public async Task<IActionResult> UpdateCommissionStatus(int id, [FromBody] UpdateCommissionStatusCommand updateDTO)
        {
            try
            {
                // Check if the 'updateDTO' object is null
                if (updateDTO == null)
                {
                    return BadRequest("Invalid data. The 'updateDTO' object is null.");
                }



                // Send the command to the mediator
                var result = await mediator.Send(new UpdateCommissionStatusCommand() { InvoiceId = id, Status = updateDTO.Status });

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


    }

}
