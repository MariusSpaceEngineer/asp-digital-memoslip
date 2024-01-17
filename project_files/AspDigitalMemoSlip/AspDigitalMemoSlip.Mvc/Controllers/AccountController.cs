using DTOClassLibrary.DTO.Consigner;
using DTOClassLibrary.DTO.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DTOClassLibrary.DTO.Authentication;

namespace AspDigitalMemoSlip.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public AccountController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _client = httpClientFactory.CreateClient("MvcClient");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            GetUserInfo();
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Remove the authToken cookie
            Response.Cookies.Delete("authToken");

            // Redirect the user to the home page
            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        public async Task<IActionResult> GetConsignerQRCode()
        {
            var response = await _client.GetAsync("Authentication/consigner/qrcode");

            if (response.IsSuccessStatusCode)
            {
                var content = JsonConvert.DeserializeObject<QRCodeResult>(await response.Content.ReadAsStringAsync());

                GetUserInfo();

                // Store the QR code and url in the ViewBag
                ViewBag.QRCode = "data:image/png;base64," + content.QRcode;
                ViewBag.Url = content.Url;

                // Return the Profile view
                return View("Profile");
            }
            else
            {
                // If the response is not successful, handle the error
                ModelState.AddModelError("", "Failed to generate QR code.");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginDTO model)
        {
            // Check if the username or password is provided
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                TempData["ErrorMessages"] = new string[] { "Username and password are required." };
                return RedirectToAction("Login", "Account");
            }


            // Create the content for the POST request
            var json = JsonConvert.SerializeObject(new { Username = model.Username, Password = model.Password });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("Authentication/login", content);

            if (result.IsSuccessStatusCode)
            {
                var token = result.Headers.GetValues("Authorization").First().Replace("Bearer ", string.Empty);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // The cookie is not accessible via JavaScript
                    Secure = true, // Transmit the cookie over HTTPS only
                    SameSite = SameSiteMode.Strict, // Prevents the cookie from being sent in cross-site requests
                };
                // If the response is successful, store the token in a cookie
                Response.Cookies.Append("authToken", token, cookieOptions);

                return RedirectToAction("Profile", "Account");
            }
            else
            {
                var errorResponse = await HandleRegistrationError(result);
                TempData["ErrorMessages"] = errorResponse;
                return RedirectToAction("Login", "Account");
            }
        }

        //Get the user information from the cookie recieved from a successful login
        private void GetUserInfo()
        {
            var token = Request.Cookies["authToken"];
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims;
                ViewBag.Name = claims.FirstOrDefault(c => c.Type == "name")?.Value;
                ViewBag.PhoneNumber = claims.FirstOrDefault(c => c.Type == "phone_number")?.Value;
            }
        }

        private async Task<List<string>> HandleRegistrationError(HttpResponseMessage response)
        {
            var errorMessages = new List<string>();
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = System.Text.Json.JsonSerializer.Deserialize<ErrorResponse>(responseContent);

                // Check if the message contains "--"
                if (responseObject.Message.Contains("--"))
                {
                    // Extract only the part after the property name
                    var messages = responseObject.Message.Split("--");
                    var formattedMessages = messages.Select(msg =>
                    {
                        var parts = msg.Split(":");
                        return parts.Length > 1 ? parts[1].Split("Severity")[0].Trim() : "";
                    });

                    errorMessages.AddRange(formattedMessages.Where(msg => !string.IsNullOrEmpty(msg)));
                }
                else
                {
                    // If the message does not contain "--", add the entire message to the list
                    errorMessages.Add(responseObject.Message);
                }
            }
            catch (Exception ex)
            {
                errorMessages.Add($"An error occurred while processing the response: {ex.Message}");
            }

            return errorMessages;
        }
    }
}
