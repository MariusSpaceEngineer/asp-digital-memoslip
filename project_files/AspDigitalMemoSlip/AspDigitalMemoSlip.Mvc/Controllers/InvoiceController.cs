using AspDigitalMemoSlip.Mvc.Models;
using DTOClassLibrary.DTO.Invoice;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using X.PagedList;

namespace AspDigitalMemoSlip.Mvc.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly HttpClient _httpClient;

        public InvoiceController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        [HttpPatch]
        [Route("UpdateCommissionStatus/{id}")]
        public async Task<IActionResult> UpdateCommissionStatus(int id, [FromBody] UpdateCommissionStatusDTO updateDTO, [FromQuery] int pageNr = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Check if the 'updateDTO' object is null
                if (updateDTO == null)
                {
                    // Log the request body for debugging purposes
                    var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                    Console.WriteLine($"Request body: {requestBody}");

                    return BadRequest("Invalid data. The 'updateDTO' object is null.");
                }

                // Additional validation logic if needed

                // Convert DTO to HttpContent
                var content = new StringContent(JsonConvert.SerializeObject(updateDTO), Encoding.UTF8, "application/json");

                // Send the PATCH request to the backend API
                var response = await _httpClient.PatchAsync($"UpdateCommissionStatus/{id}?pageNr={pageNr}&pageSize={pageSize}", content);
                response.EnsureSuccessStatusCode();

                // Optionally, read the response content as JObject
                var result = await response.Content.ReadAsAsync<JObject>();

                // Update the UI on success
                Console.WriteLine($"Commission status updated for invoice {id}. New status: {(updateDTO.Status == 0 ? "Paid" : "Unpaid")}");

                // Modify the response body to change 1 to 0
                if (result != null && result.TryGetValue("status", out JToken statusToken) && statusToken.Value<int>() == 1)
                {
                    result["status"] = 0;
                }

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


