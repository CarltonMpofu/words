
using Blazored.LocalStorage;

namespace Words.Client
{
    public class CustormHttpHandler: DelegatingHandler
    {
        private  ILocalStorageService _localStorage { get; }

        public CustormHttpHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            InnerHandler = new HttpClientHandler();
        }

        // Executes before sending to the server
        // Bypass logic before sending to the server
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(request.RequestUri.AbsolutePath.ToLower().Contains("login") ||
               request.RequestUri.AbsolutePath.ToLower().Contains("register"))
            { // Dont do anything
                return await base.SendAsync(request, cancellationToken);
            }

            // Bypass header and add the authorization header
            string token = await _localStorage.GetItemAsStringAsync("token");

            Console.WriteLine(token);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("Authorization", $"Bearer {token.Replace("\"", "")}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
