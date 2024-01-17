using AspDigitalMemoSlip.Application.CQRS.Products;
using AspDigitalMemoSlip.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using X.PagedList;

namespace AspDigitalMemoSlip.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

 
        [HttpPatch]
        [Route("UpdateProductStatus/{id}")]
        public async Task<IActionResult> UpdateProductStatus(int id, [FromBody] UpdateSoldStatusDev request, [FromQuery] int pageNr = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Check if the 'request' object is null
                if (request == null)
                {
                    // Log the request body for debugging purposes
                    var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                    Console.WriteLine($"Request body: {requestBody}");

                    return BadRequest("Invalid data. The 'updateDTO' object is null.");
                }

                // Additional validation logic if needed



                // Convert DTO to HttpContent
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                // Send the PATCH request to the backend API
                var response = await _httpClient.PatchAsync($"UpdateSoldStatus/{id}?pageNr={pageNr}&pageSize={pageSize}", content);
                response.EnsureSuccessStatusCode();

                // Optionally, read the response content as JObject
                var result = await response.Content.ReadAsAsync<JObject>();

                // Update the UI on success
                Console.WriteLine($"Product status updated for product {id}. New status: {request.Status}");

                // Include pagination information
                if (response.Headers.TryGetValues("X-Total-Pages", out var totalPagesHeader) &&
                    response.Headers.TryGetValues("X-Page", out var currentPageNrHeader))
                {
                    var totalPages = Convert.ToInt32(totalPagesHeader.First());
                    var currentPageNr = Convert.ToInt32(currentPageNrHeader.First());

                    result["totalPages"] = totalPages;
                    result["currentPageNr"] = currentPageNr;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine(ex);

                // Return a 500 Internal Server Error response
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPatch]
        [Route("UpdateProductStatusCounter/{id}")]
        public async Task<IActionResult> UpdateProductStatusCounter(int id, [FromBody] UpdateSoldStatusCounter request, [FromQuery] int pageNr = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Check if the 'request' object is null
                if (request == null)
                {
                    // Log the request body for debugging purposes
                    var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                    Console.WriteLine($"Request body: {requestBody}");

                    return BadRequest("Invalid data. The 'request' object is null.");
                }



                // Get the new suggested price from the request body
                var newSuggestedPrice = request.SuggestedPrice;

                // Convert DTO to HttpContent
                var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    productId = id,
                    status = request.Status,
                    suggestedPrice = request.SuggestedPrice,
                    commisionPrice = request.CommisionPrice,
                    commissionPaidBy = request.CommissionPaidBy



                }), Encoding.UTF8, "application/json");

                // Send the PATCH request to the backend API
                var response = await _httpClient.PatchAsync($"UpdateSoldStatusCounter/{id}?pageNr={pageNr}&pageSize={pageSize}", content);
                response.EnsureSuccessStatusCode();

                // Optionally, read the response content as JObject
                var result = await response.Content.ReadAsAsync<JObject>();

                // Update the UI on success
                Console.WriteLine($"Product status updated for product {id}. New status: {request.Status}");

                // Include pagination information
                if (response.Headers.TryGetValues("X-Total-Pages", out var totalPagesHeader) &&
                    response.Headers.TryGetValues("X-Page", out var currentPageNrHeader))
                {
                    var totalPages = Convert.ToInt32(totalPagesHeader.First());
                    var currentPageNr = Convert.ToInt32(currentPageNrHeader.First());

                    result["totalPages"] = totalPages;
                    result["currentPageNr"] = currentPageNr;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine(ex);

                // Return a 500 Internal Server Error response
                return StatusCode(500, "Internal Server Error");
            }
        }














    }
}
