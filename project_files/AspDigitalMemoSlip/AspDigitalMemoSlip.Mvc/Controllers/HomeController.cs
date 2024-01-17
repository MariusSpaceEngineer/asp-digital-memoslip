using AspDigitalMemoSlip.Mvc.Models;
using DTOClassLibrary.DTO.Notification;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;

namespace AspDigitalMemoSlip.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory.CreateClient("MvcClient");
        }

        public async Task<IActionResult> Index()
        {
            List<NotificationDTO> notifications;
            if (HttpContext.Request.Cookies.ContainsKey("authToken"))
            {
                notifications = await GetNofications();
            }
            else
            {
                 notifications = new List<NotificationDTO>();

            }
            return View(notifications);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

      
        public async Task<List<NotificationDTO>> GetNofications()
        {
            string apiUrl = "/notifications";
            

            List<NotificationDTO> notifications = await _clientFactory.GetFromJsonAsync<List<NotificationDTO>>(apiUrl);

            return notifications;
        }
    }
}