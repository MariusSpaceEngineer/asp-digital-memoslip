using AspDigitalMemoSlip.Mvc.Models;
using DTOClassLibrary.DTO.Consignee;
using DTOClassLibrary.DTO.Consigner;
using DTOClassLibrary.DTO.Memo;
using DTOClassLibrary.DTO.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace AspDigitalMemoSlip.Mvc.Controllers
{
    public class MemoController : Controller
    {

        private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient _client;

        public MemoController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            _client = httpClientFactory.CreateClient("MvcClient");

        }

        // GET: Memo/MemosPage
        public async Task<IActionResult> MemosPage()
        {
            Console.WriteLine("memo controller: getting memos page");
            List<MemoDTO> memos = new List<MemoDTO>();
            Console.WriteLine("memo controller: getting memos from api");
            var response = await _client.GetAsync("memo");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                memos = JsonSerializer.Deserialize<List<MemoDTO>>(content);
            }
            else
            {
                Console.WriteLine("memo controller: error while fetching memos");
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine(response.ToString());
            }

            return View(memos);
        }

        // GET: Memo/MemoDetails/id
        public async Task<IActionResult> MemoDetails(int id)
        {
            Console.WriteLine("memo controller: getting memo details for memo Id " + id);
            var response = await _client.GetAsync("memo/" + id);
            MemoDTO memo = null;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("memo controller: content of response from api: " +content);
                memo = JsonSerializer.Deserialize<MemoDTO>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Match JSON property names in a case-insensitive manner
                });
            }
            else
            {
                Console.WriteLine("memo controller: error while fetching memo");
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine(response.ToString());
            }


            Console.WriteLine("memo sent to view: ");
            PrintProperties(memo);

            return View(memo);
        }



        bool modelStateValid = true;

        string userId = null;

        private void GetUserInfo()
        {
            var token = Request.Cookies["authToken"];
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims;
                var userIdClaim = claims.FirstOrDefault(c => c.Type == "nameid");

                if (userIdClaim != null)
                {
                    userId = userIdClaim.Value;
                }
            }
        }


        // GET: Memo/Create
        public async Task<ActionResult> Create()
        {
            Console.WriteLine("memo controller: getting create page");
            Console.WriteLine("memo controller: getting user info");



            var viewModel = new MemoViewModel
            {
                Memo = new Memo
                {
                    Products = new List<Product>(),
                    Consigner = new Consigner(),
                    Consignee = new Consignee()
                },
                Consignees = new List<SelectListItem>()
            };







            var consigneesResponse = await _client.GetAsync("consignee/GetAllConsignees");
            if (consigneesResponse.IsSuccessStatusCode)
            {
                var consigneesData = await consigneesResponse.Content.ReadAsAsync<IEnumerable<Consignee>>();
                if (consigneesData != null)
                {
                    viewModel.Consignees = consigneesData
                        .Where(c => c.AcceptedByConsigner)
                        .Select(c => new SelectListItem
                        {
                            Value = c.Id,
                            Text = c.Name
                        }).ToList();
                }
            }
            else
            {
                Console.WriteLine("MemoController: Error while fetching consignees:");
                Console.WriteLine(consigneesResponse.Content.ReadAsStringAsync().Result);
                Console.WriteLine(consigneesResponse.ToString());
                Console.WriteLine("MemoController: End of error while fetching consignees");
            }

            return View(viewModel);
        }



        [HttpPost]
        public async Task<ActionResult> Create([Bind("Memo, Memo.ConsignerId, Memo.ConsigneeId, Memo.Products, Memo.TermsAccepted")] MemoViewModel viewModel)
        {
            Console.WriteLine("memo controller: received post");

            //sets userId variable to currently logged in Consigner
            GetUserInfo();

            if (userId is not null)
            {
                Console.WriteLine("memo controller: user id: " + userId);

                viewModel.Memo.ConsignerId = userId;
                if(viewModel.Memo.Consigner is null)
                {
                    viewModel.Memo.Consigner = new Consigner();
                }
                viewModel.Memo.Consigner.Id = userId;
            }

            Console.WriteLine("memo controller: consigner id : " + viewModel.Memo.ConsignerId);
            Console.WriteLine("memo controller: consignee id : " + viewModel.Memo.ConsigneeId);

            foreach (var prod in viewModel.Memo.Products)
            {
                prod.ConsignerId = viewModel.Memo.ConsignerId;
                prod.ConsigneeId = viewModel.Memo.ConsigneeId;
            }

            if (string.IsNullOrEmpty(viewModel.Memo.ConsignerId))
            {
                ModelState.AddModelError("Memo.ConsignerId", "Consigner ID cannot be empty.");
                modelStateValid = false;
            }

            if (string.IsNullOrEmpty(viewModel.Memo.ConsigneeId))
            {
                ModelState.AddModelError("Memo.ConsigneeId", "Consignee ID cannot be empty.");
                modelStateValid = false;
            }

            for (int i = 0; i < viewModel.Memo.Products.Count; i++)
            {
                var product = viewModel.Memo.Products[i];

                if (product.Carat < 0)
                {
                    ModelState.AddModelError($"Memo.Products[{i}].Carat", "Carat cannot be negative.");
                    modelStateValid = false;
                }
                if (product.Price < 0)
                {
                    ModelState.AddModelError($"Memo.Products[{i}].Price", "Price cannot be negative.");
                    modelStateValid = false;
                }
            }






            if (modelStateValid)
            {
                // Convert to DTO
                Console.WriteLine("memo controller: model state valid:");
                PrintProperties(viewModel.Memo);
                MemoDTO memoDTO = ConvertMemoToDTO(viewModel.Memo);
                Console.WriteLine("memo controller: converted to DTO:");
                PrintProperties(memoDTO);
                // Post                 
                var response = _client.PostAsJsonAsync("Memo", memoDTO).Result;
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("memo controller: received success status code");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Console.WriteLine("memo controller: received error status code");
                    Console.WriteLine("memo controller: " + response.Content.ReadAsStringAsync().Result);
                    ModelState.AddModelError("", "Error while saving the memo");
                }



                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine("memo controller: model state not valid");
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            // Repopulate the Consignees dropdown data
            var consigneesResponse = await _client.GetAsync("consignee/GetAllConsignees");
            if (consigneesResponse.IsSuccessStatusCode)
            {
                var consigneesData = await consigneesResponse.Content.ReadAsAsync<IEnumerable<Consignee>>();
                if (consigneesData != null)
                {
                    viewModel.Consignees = consigneesData
                        .Where(c => c.AcceptedByConsigner)
                        .Select(c => new SelectListItem
                        {
                            Value = c.Id,
                            Text = c.Name
                        }).ToList();
                }
            }
            return View(viewModel);
        }








        private MemoDTO ConvertMemoToDTO(Memo memo)
        {
            Console.WriteLine("converting memo to DTO");

            var consignerDTO = new ConsignerDTO { Id = memo.ConsignerId };
            var consigneeDTO = new ConsigneeDTO { Id = memo.ConsigneeId };



            var memoDTO = new MemoDTO
            {
                Products = memo.Products?.Where(p => p != null)
                                         .Select(p => new ProductDTO
                                         {
                                             LotNumber = p.LotNumber,
                                             Description = p.Description,
                                             Carat = p.Carat,
                                             Price = p.Price,
                                             Remarks = p.Remarks,
                                             ConsignerId = memo.ConsignerId,
                                             ConsigneeId = memo.ConsigneeId,
                                         }).ToList() ?? new List<ProductDTO>(),

                Consigner = consignerDTO,
                Consignee = consigneeDTO,
                ConsignerId = memo.ConsignerId,
                ConsigneeId = memo.ConsigneeId,
                Password = memo.Password,


                TermsAccepted = memo.TermsAccepted
            };

            Console.WriteLine("memo converted to DTO");
            return memoDTO;
        }







        static void PrintProperties(object obj, string indent = "")
        {
            if (obj == null)
            {
                Console.WriteLine($"{indent}Object is null");
                return;
            }

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj, null);

                if (value is IEnumerable && !(value is string))
                {
                    Console.WriteLine($"{indent}{property.Name}:");
                    foreach (var item in (IEnumerable)value)
                    {
                        PrintProperties(item, indent + "    ");
                    }
                }
                else if (value != null && value.GetType().IsClass && !(value is string))
                {
                    Console.WriteLine($"{indent}{property.Name}:");
                    PrintProperties(value, indent + "    ");
                }
                else
                {
                    Console.WriteLine($"{indent}{property.Name}: {value}");
                }
            }
        }



    }

}
