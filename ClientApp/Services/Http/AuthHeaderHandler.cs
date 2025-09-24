using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp.Services.Http
{
    // This handler automatically adds the JWT Authorization header to outgoing requests.
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IAuthService _authService;

        // Initializes the handler with the authentication service.
        public AuthHeaderHandler(IAuthService authService)
        {
            _authService = authService;
        }

        // Intercepts outgoing HTTP requests to add the Authorization header.
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _authService.GetCurrentToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}