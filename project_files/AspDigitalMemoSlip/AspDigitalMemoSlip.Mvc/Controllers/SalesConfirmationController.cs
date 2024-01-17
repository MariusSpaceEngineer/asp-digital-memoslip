using AspDigitalMemoSlip.Mvc.Models;
using DTOClassLibrary.DTO;
using DTOClassLibrary.DTO.ProductSale;
using DTOClassLibrary.DTO.SalesConfirmation;
using DTOClassLibrary.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Text;

namespace AspDigitalMemoSlip.Mvc.Controllers
{
    public class SalesConfirmationController : Controller
    {
        private readonly HttpClient _httpClient;

        public SalesConfirmationController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("MvcClient");
        }
        
        public async Task<IActionResult> SalesConfirmationAsync()
        {
            var response = await _httpClient.GetAsync($"SalesConfirmation");
            response.EnsureSuccessStatusCode();

            var salesConfirmationDTOs = await response.Content.ReadAsAsync<List<SalesConfirmationDTO>>();

            return View(salesConfirmationDTOs);
        }

        [HttpPost]
        [Route("salesconfirmation/update")]
        public async Task<IActionResult> UpdateSalesConfirmations([FromBody] SalesConfirmationUpdateModel SalesConfirmationUpdate)
        {
            

            SalesConfirmationDTO salesConfirmationDTO = new SalesConfirmationDTO()
            {
                Id = SalesConfirmationUpdate.SalesConfirmationId,
                SuggestedCommision = SalesConfirmationUpdate.Commision
            };

            foreach (var item in SalesConfirmationUpdate.UpdatedProductsSale)
            {
                var productSaleDTO = new ProductSaleDTO()
                {
                    Id = item.Key,
                    SalePrice = Convert.ToInt32(item.Value.Price),
                };
                if (Enum.TryParse(item.Value.Decision, out AgreementState parsedState))
                {
                    productSaleDTO.AgreementStates = parsedState;
                }
                else
                {
                    productSaleDTO.AgreementStates = AgreementState.SuggestedPrice;
                }
                salesConfirmationDTO.SoldProducts.Add(productSaleDTO);
            }

            var jsonContent = JsonConvert.SerializeObject(salesConfirmationDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("SalesConfirmation/update", content);
        
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("SalesConfirmationAsync"); 
            }
            else
            {
                return View("SalesConfirmation");
            }
        }

        [HttpPatch]
        [Route("salesconfirmation/approve/{salesconfirmationid}")]
        public async Task<IActionResult> ApproveSalesConfirmation(int salesConfirmationId)
        {
            HttpContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

        
            var response = await _httpClient.PatchAsync("salesconfirmation/approve/"+ salesConfirmationId, content);
            if (response.IsSuccessStatusCode)
            {
                RedirectToAction("SalesConfirmationAsync");
            }

            return null;
        
        }
    }
}
