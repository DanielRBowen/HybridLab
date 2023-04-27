using HybridLab.Core.Clients;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace HybridLab.Core
{
    public class MainAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly MainHttpClient _mainHttpClient;
        private readonly IConfiguration _configuration;
        private readonly IAppKeys _appKeys;

        public MainAuthenticationStateProvider(
            MainHttpClient mainHttpClient,
            IAppKeys appKeys,
            IConfiguration configuration)
        {
            _mainHttpClient = mainHttpClient;
            _appKeys = appKeys;
            _configuration = configuration;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string token = await _appKeys.GetAsync("token");
                var identity = new ClaimsIdentity();

                // Since httpclient is scoped, the client will be different if one refreshes the page or restart.
                _mainHttpClient.DefaultRequestHeaders.Authorization = null;

                if (!string.IsNullOrEmpty(token))
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                    _mainHttpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
                }

                var serviceUrl = _configuration.GetValue<string>("ServiceUrl", "");

                if (string.IsNullOrEmpty(serviceUrl))
                {
                    serviceUrl = await _appKeys.GetAsync("serviceurl");
                }

                if (!string.IsNullOrEmpty(serviceUrl) && _mainHttpClient.BaseAddress == null && _mainHttpClient.BaseAddress != new Uri(serviceUrl))
                {
                    _mainHttpClient.BaseAddress = new Uri(serviceUrl);
                }

                if (_mainHttpClient.DefaultRequestHeaders.Contains("IsEowClient") == false)
                {   
                    _mainHttpClient.DefaultRequestHeaders.Add("IsEowClient", "true");
                }

                var sessionId = await _appKeys.GetAsync("SessionId");

                if (sessionId == null && _mainHttpClient.DefaultRequestHeaders.Contains("SessionId"))
                {
                    _mainHttpClient.DefaultRequestHeaders.Remove("SessionId");
                }

                if (_mainHttpClient.DefaultRequestHeaders.Contains("SessionId") == false && sessionId != null)
                {
                    _mainHttpClient.DefaultRequestHeaders.Add("SessionId", sessionId);
                }

                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user);
                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return state;
            }
            catch (Exception)
            {
                await _appKeys.RemoveAllAsync();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

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
