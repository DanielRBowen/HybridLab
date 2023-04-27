using HybridLab.Core.Dtos;
using HybridLab.Core.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.Net.Http.Json;


namespace HybridLab.Core.Clients
{
    public class AccountClient : IAccountClient
    {
        private readonly MainHttpClient _mainHttpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;
        private readonly IAppKeys _appKeys;
        private readonly ILogger<AccountClient> _logger;
        private readonly IConfiguration _configuration;

        private string _userName;

        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_userName) == false)
                {
                    return _userName;
                }

                return string.Empty;
            }
        }

        private string _serviceUrl;

        public string ServiceUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_serviceUrl) == false)
                {
                    return _serviceUrl;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Uncapitalized Tenant name from the ServiceUrl
        /// </summary>
        public string TenantName
        {
            get
            {
                if (string.IsNullOrEmpty(_serviceUrl))
                {
                    return string.Empty;
                }

                return _serviceUrl.GetSubDomain();
            }
        }


        public AccountClient(MainHttpClient mainHttpClient,
            AuthenticationStateProvider authenticationStateProvider,
            NavigationManager navigationManager,
            IAppKeys appKeys,
            ILogger<AccountClient> logger,
            IConfiguration configuration)
        {
            _mainHttpClient = mainHttpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
            _appKeys = appKeys;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> Login(UserLoginDto user)
        {
            try
            {
                var serviceUrlString = _configuration.GetValue<string>("ServiceUrl", "");
                
                if (string.IsNullOrEmpty(serviceUrlString) && !string.IsNullOrEmpty(user.ServiceUrl))
                {
                    serviceUrlString = user.ServiceUrl;
                }

                if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(serviceUrlString))
                {
                    return false;
                }

                var serviceUrl = new Uri(serviceUrlString);
                await _appKeys.SetAsync("serviceurl", serviceUrl.AbsoluteUri);
                var response = await _mainHttpClient.PostAsJsonAsync("api/account/token", user);
                var token = await response.Content.ReadAsStringAsync();

                if (token.ToLowerInvariant().Contains("error"))
                {
                    _logger.LogError("The login token contains an error: {0}", token);
                    return false;
                }

                await _appKeys.SetAsync("token", token);
                await _appKeys.SetAsync("username", user.UserName);
                await _appKeys.SetAsync("password", user.Password);
                await _appKeys.SetAsync("serviceurl", serviceUrl.AbsoluteUri);
                _userName = user.UserName;
                _serviceUrl = user.ServiceUrl;
                await _authenticationStateProvider.GetAuthenticationStateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Couldn't log in. \r\n {0}", ex);
                return false;
            }

            return true;
        }

        public async Task<bool> SetAccount()
        {
            if (string.IsNullOrEmpty(_serviceUrl))
            {
                var serviceUrl = _configuration.GetValue<string>("ServiceUrl", "");

                if (string.IsNullOrEmpty(serviceUrl))
                {
                    serviceUrl = await _appKeys.GetAsync("serviceurl");
                }

                if (string.IsNullOrEmpty(serviceUrl) == false)
                {
                    _serviceUrl = serviceUrl;
                }
                else
                {
                    return false;
                }
            }

            if (string.IsNullOrEmpty(_userName))
            {
                var userName = await _appKeys.GetAsync("username");

                if (string.IsNullOrEmpty(userName) == false)
                {
                    _userName = userName;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public async Task GetAuthenticationState()
        {
            await _authenticationStateProvider.GetAuthenticationStateAsync();
        }

        private void ClearAccount()
        {
            _userName = string.Empty;
            _serviceUrl = string.Empty;
        }

        public async Task<bool> Logout()
        {
            try
            {
                await _appKeys.RemoveAllAsync();
                ClearAccount();
                await _authenticationStateProvider.GetAuthenticationStateAsync();
                Microsoft.AspNetCore.Components.WebAssembly.Authentication.NavigationManagerExtensions.NavigateToLogout(_navigationManager, "/");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
