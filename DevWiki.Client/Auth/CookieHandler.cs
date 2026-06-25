using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace DevWiki.Client.Auth
{
    public class CookieHandler : DelegatingHandler
    {
        private readonly Microsoft.JSInterop.IJSRuntime _jsRuntime;

        public CookieHandler(Microsoft.JSInterop.IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", new object[] { "authToken" });
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
