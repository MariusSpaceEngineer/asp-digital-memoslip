using AspDigitalMemoSlip.Application.Exceptions.Authentication;
using AspDigitalMemoSlip.Application.Interfaces.Commands;
using AspDigitalMemoSlip.Application.Services.Authentication;
using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.DTO.Authentication;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AspDigitalMemoSlip.Application.CQRS.Authentication
{
    public class LoginCommand : IRequest<AuthResult>
    {
        public LoginDTO Dto { get; set; }
        public string Url { get; set; }

        public LoginCommand(LoginDTO model, string url)
        {
            Dto = model;
            Url = url;
        }
    }

    public class LoginCommandHandler : LoginService, IRequestHandler<LoginCommand, AuthResult>, IAuthCommand
    {
        private readonly TokenHelper _tokenHelper;
        private readonly IConfiguration _configuration;


        public LoginCommandHandler(UserManager<User> userManager, IUserTwoFactorTokenProvider<User> tokenProvider, TokenHelper tokenHelper, IConfiguration configuration) : base(userManager, tokenProvider)
        {
            _tokenHelper = tokenHelper;
            _configuration = configuration;
        }

        public async Task<AuthResult> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            //Check if user exists
            var user = await ValidateUser(command.Dto);
            if (user == null)
                throw new UserValidationFailedException("Your username or password is incorrect. Please check your credentials and try again.");

            // Check the role and the URL of which the request is made
            if (!await CheckRoleAndUrl(user, command.Url))
            {
                throw new RoleUrlCheckFailedException("Unauthorized");
            }

            //Generates claims to be send to the client, if null the consignee hasn't yet been approved by his consigner
            var claims = await GenerateClaimsAsync(user);
            if (claims == null)
            {
                throw new ClaimsGenerationFailedException("Unauthorized: Account has not yet been accepted by consigner.");
            }

            //Generates JWT-Token
            string token = _tokenHelper.GenerateToken(claims);

            return new AuthResult(StatusCodes.Status200OK, "Login successful", token);
        }


        public async Task<bool> CheckRoleAndUrl(User user, string url)
        {
            // Get the user's role
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault(); // Assumes each user has one role

            // Get the URLs from the configuration
            var consignerUrls = new List<string> { _configuration["ClientUrls:MVC"], _configuration["ClientUrls:API"] };
            var consigneeUrls = new List<string> { _configuration["ClientUrls:Blazor"], _configuration["ClientUrls:API"] };
            var adminUrls = new List<string> { _configuration["ClientUrls:Blazor"], _configuration["ClientUrls:API"], _configuration["ClientUrls:MVC"] };

            // Check the role and the URL
            if (userRole == "Consigner" && !consignerUrls.Contains(url))
            {
                return false;
            }
            else if (userRole == "Consignee" && !consigneeUrls.Contains(url))
            {
                return false;
            }
            else if (userRole == "Admin" && !adminUrls.Contains(url))
            {
                return false;
            }

            return true;
        }

        public async Task<List<Claim>> GenerateClaimsAsync(User user)
        {
            try
            {
                List<Claim> authClaims;
                var userRoles = await _userManager.GetRolesAsync(user);

                if (user is Consignee consignee)
                {

                    if (consignee.AcceptedByConsigner)
                    {
                        authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, consignee.Id),
                            new Claim(ClaimTypes.Name, consignee.UserName),
                            new Claim("name", consignee.Name),
                            new Claim("phone_number", consignee.PhoneNumber),
                            new Claim("vat_number", consignee.VATNumber),
                            new Claim("insurance_number", consignee.InsuranceNumber),
                            new Claim("insurance_coverage", consignee.InsuranceCoverage.ToString()),
                            new Claim("id_number", consignee.NationalRegistryNumber),
                            new Claim("id_expire_date", consignee.NationalRegistryExpirationDate?.ToString("o")),
                            new Claim("id_will_expire", _tokenHelper.IsIDAboutToExpire(consignee).ToString()),
                            new Claim("mfa_enabled", consignee.TwoFactorEnabled.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };
                    }
                    else
                    {
                        return null;
                    }

                }
                else if (user is Domain.Consigner consigner)
                {

                    authClaims = new List<Claim>
                    {
                    new Claim(ClaimTypes.NameIdentifier, consigner.Id),
                    new Claim(ClaimTypes.Name, consigner.UserName),
                    new Claim("name", consigner.Name),
                    new Claim("phone_number", consigner.PhoneNumber),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                }
                else if (user is User) //If the user is something else other than a consignee or consigner (admin?)
                {
                    authClaims = new List<Claim>
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("name", user.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                }
                else
                {
                    throw new UserTypeNotRecognizedException("User type not recognized.");
                }

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                return authClaims;
            }
            catch (Exception e)
            {

                throw new ClaimsGenerationFailedException("An error occured when generating user's claims.", e);
            }
        }
    }
}
