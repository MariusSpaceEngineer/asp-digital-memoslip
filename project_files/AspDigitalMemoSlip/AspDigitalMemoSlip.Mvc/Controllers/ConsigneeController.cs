using AspDigitalMemoSlip.Application.CQRS.Consigner;
using AspDigitalMemoSlip.Application.Utils;
using AspDigitalMemoSlip.Mvc.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AspDigitalMemoSlip.Mvc.Controllers
{
    public class ConsigneeController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient _client;

        public ConsigneeController(IHttpClientFactory clientFactory, IMemoryCache memoryCache)
        {
            this.httpClientFactory = clientFactory;
            _client = httpClientFactory.CreateClient("MvcClient");

        }

        public async Task<IActionResult> Index()
        {
            string apiUrl = "ID";
            HttpClient httpClient = httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(apiUrl);


            if (response.IsSuccessStatusCode)
            {
                var profile = await response.Content.ReadAsAsync<Identity>();

                var imageData = profile.Picture;
                if (imageData != null && imageData.Length > 0)
                {
                    var base64String = Convert.ToBase64String(imageData);
                    ViewBag.Picture = base64String;
                }

                ViewBag.Profile = profile;
            }
            else
            {

                ViewBag.ProfileError = $"Error:{response.StatusCode}{response.ReasonPhrase}";
            }


            return View("ConsgineeIdentity");
        }

        [Route("Consignee/Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            string getConsigneeEndpoint = $"Consignee/GetConsigneeByUserId/{id}";
            string getConsigneeImagesEndpoint = $"Authentication/consignee/images/{id}";

            //Get the consignee information + images linked to his profile
            var consigneeInfo = await _client.GetAsync(getConsigneeEndpoint);
            var consigneeImages = await _client.GetAsync(getConsigneeImagesEndpoint);

            if (consigneeInfo.IsSuccessStatusCode)
            {
                var consignee = await consigneeInfo.Content.ReadAsAsync<Consignee>();
                ViewBag.Consignee = consignee;

                if (consigneeImages.IsSuccessStatusCode)
                {
                    var contentString = await consigneeImages.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    var images = JsonSerializer.Deserialize<ConsigneeImagesModel>(contentString, options);
                    ViewBag.Images = images; 
                }
                else
                {
                    ViewBag.Images = null; // No images available
                }

                return View("Details");
            }
            else
            {
                ViewBag.ConsigneeError = $"Error: Consignee with id {id} not found";
                return View("Error");
            }
        }


        [Route("Consignee/Accept/{id}")]
        public async Task<IActionResult> Accept(string id)
        {
            // Your logic for accepting the consignee goes here
            // This could involve updating the status of the consignee in your database
            string apiUrl = $"Consignee/AcceptConsignee/{id}";

            var response = await _client.PostAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("PendingConsignees", "Consigner");
            }
            else
            {
                return View("Error");
            }

        }


        [Route("Consignee/Decline/{id}")]
        public async Task<IActionResult> Decline(string id)
        {
            // Your logic for accepting the consignee goes here
            // This could involve updating the status of the consignee in your database
            string apiUrl = $"Consignee/DeclineConsignee/{id}";

            var response = await _client.PostAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("PendingConsignees", "Consigner");
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> ConsigneeOverview()
        {
            List<Consignee> consignees = new List<Consignee>();
            string apiUrl = "Consignee/GetAllConsignees";
            var response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var contentString = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                consignees = JsonSerializer.Deserialize<List<Consignee>>(contentString, options);
                if (consignees != null)
                {
                    foreach (var consignee in consignees)
                    {
                        string getConsigneeImagesEndpoint = $"Authentication/consignee/images/{consignee.Id}";

                        var consigneeImages = await _client.GetAsync(getConsigneeImagesEndpoint);

                        if (consigneeImages.IsSuccessStatusCode)
                        {


                            contentString = await consigneeImages.Content.ReadAsStringAsync();

                            options = new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            };

                            var images = JsonSerializer.Deserialize<ConsigneeImagesModel>(contentString, options);
                            consignee.Images = new ImagesModel();
                            consignee.Images.Selfie = images.Selfie.FileContents;
                        }

                        else
                        {
                            ViewBag.Images = null; // No images available
                        }
                    }

                }
            }
            else
            {
                ViewBag.Consignees = null;
            }

            

            ViewBag.Consignees = consignees;


            return View("ConsigneeOverview");
        }



    }
}

