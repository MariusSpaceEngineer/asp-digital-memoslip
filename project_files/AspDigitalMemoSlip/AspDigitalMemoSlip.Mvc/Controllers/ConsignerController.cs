using DTOClassLibrary.DTO.Consignee;
using Microsoft.AspNetCore.Mvc;

namespace AspDigitalMemoSlip.Mvc.Controllers
{
    public class ConsignerController : Controller
    {

        private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient _client;


        public ConsignerController(IHttpClientFactory clientFactory)
        {
            this.httpClientFactory = clientFactory;
            _client = httpClientFactory.CreateClient("MvcClient");

        }
        public IActionResult Account()
        {
            return View("Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> GetConsigners()
        {
            string apiUrl = "Consignee/GetAllConsignees";
            HttpClient httpClient = httpClientFactory.CreateClient();

            List<ConsigneeDTO> consignees = await httpClient.GetFromJsonAsync<List<ConsigneeDTO>>(apiUrl);

            ViewBag.Consignees = consignees;

            return View("Dashboard", consignees);
        }

        [HttpGet]
        [Route("Consigner/PendingConsignees")]
        public async Task<IActionResult> GetPendingConsigners()
        {
            string endpoint = "Consigner/GetAllPendingConsignees";
            List<ConsigneeDTO> consignees = await _client.GetFromJsonAsync<List<ConsigneeDTO>>(endpoint);

            ViewBag.Consignees = consignees;
            return View("PendingConsignees", consignees);
        }
    }
}
