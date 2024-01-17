using System.Net.Http.Headers;

namespace AspDigitalMemoSlip.Mvc.Utils;
public class AuthenticationHttpHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationHttpHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Get the token from the cookie
        var token = _httpContextAccessor.HttpContext.Request.Cookies["authToken"];

        // Check if the token is not null or empty
        if (!string.IsNullOrEmpty(token))
        {
            // Set the Authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Set the Origin header to the request domain and protocol
        var requestDomainAndProtocol = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
        request.Headers.Add("Origin", requestDomainAndProtocol);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
