using Blazored.LocalStorage;
using System.Security.Claims;
using System.Text.Json;

namespace Words.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        public ILocalStorageService _localStorage { get; }

        // Inject local storage
        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await _localStorage.GetItemAsStringAsync("token");

            var identity = new ClaimsIdentity(); // means user is not authorised since it is empty

            if(!string.IsNullOrEmpty(token)) // try to authenticate user if token exists
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"); // means user is  authorised

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }


        // Not my hand writing
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
