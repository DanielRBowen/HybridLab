using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace HybridLab.Core.Clients
{
    public class MainHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAppKeys _appKeys;
        private readonly IConfiguration _configuration;

        public MainHttpClient(HttpClient httpClient, IAppKeys appKeys, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _appKeys = appKeys;
            _configuration = configuration;
        }

        public HttpRequestHeaders DefaultRequestHeaders
        {
            get
            {
                return _httpClient.DefaultRequestHeaders;
            }
        }

        public Uri? BaseAddress
        {
            get
            {
                return _httpClient.BaseAddress;
            }

            set
            {
                _httpClient.BaseAddress = value;
            }
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<TValue>([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            await SetUpHeaders();
            return await _httpClient.PostAsJsonAsync(requestUri, value, options, cancellationToken);
        }

        public async Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri)
        {
            await SetUpHeaders();
            return await _httpClient.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PutAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, object? objectToPut)
        {
            await SetUpHeaders();
            return await _httpClient.PutAsJsonAsync(requestUri, objectToPut);
        }

        public async Task<HttpResponseMessage> PostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content)
        {
            await SetUpHeaders();
            return await _httpClient.PostAsync(requestUri, content);
        }

        public async Task<TValue?> GetFromJsonAsync<TValue>([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            await SetUpHeaders();
            return await _httpClient.GetFromJsonAsync<TValue>(requestUri, options, cancellationToken);
        }

        public async Task SetUpHeaders()
        {
            var serviceUrl = _configuration.GetValue<string>("ServiceUrl", "");

            if (string.IsNullOrEmpty(serviceUrl))
            {
                serviceUrl = await _appKeys.GetAsync("serviceurl");
            }

            if (!string.IsNullOrEmpty(serviceUrl) && _httpClient.BaseAddress == null && _httpClient.BaseAddress != new Uri(serviceUrl))
            {
                _httpClient.BaseAddress = new Uri(serviceUrl);
            }

            var sessionId = await _appKeys.GetAsync("SessionId");

            if (sessionId == null && _httpClient.DefaultRequestHeaders.Contains("SessionId"))
            {
                _httpClient.DefaultRequestHeaders.Remove("SessionId");
            }

            if (_httpClient.DefaultRequestHeaders.Contains("SessionId") == false && sessionId != null)
            {
                _httpClient.DefaultRequestHeaders.Add("SessionId", sessionId);
            }
        }
    }
}
