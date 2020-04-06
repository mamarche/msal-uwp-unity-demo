using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MSAL.Client
{
    public class LoginManager : ILoginManager
    {
        private string _graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";
        private string _redirectUrl = "https://login.microsoftonline.com/common/oauth2/nativeclient";
        private IPublicClientApplication _client;

        public string AccessToken { get; private set; } = null;
        public string[] Scopes { get; private set; } = null;

        public LoginManager(string clientId, string[] scopes)
        {
            this.Init(clientId, scopes);
        }

        public void Init(string clientId, string[] scopes)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentException();
            if (scopes == null) throw new ArgumentException();

            Scopes = scopes;
            
            _client = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority(AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount)
            .WithLogging((level, message, containsPii) =>
            {
                Debug.WriteLine($"MSAL: {level} {message} ");
            }, LogLevel.Warning, enablePiiLogging: false, enableDefaultPlatformLogging: true)
            .WithRedirectUri(_redirectUrl)
            .Build();
        }

        public async Task<LoginResult> AcquireToken()
        {
            AuthenticationResult authResult = null;

            // It's good practice to not do work on the UI thread, so use ConfigureAwait(false) whenever possible.            
            IEnumerable<IAccount> accounts = await _client.GetAccountsAsync().ConfigureAwait(false);
            IAccount firstAccount = accounts.FirstOrDefault();

            try
            {
                authResult = await _client.AcquireTokenSilent(Scopes, firstAccount).ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilent.
                // This indicates you need to call AcquireTokenInteractive to acquire a token
                Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await _client.AcquireTokenInteractive(Scopes)
                                                               .ExecuteAsync()
                                                               .ConfigureAwait(false);
                }
                catch (MsalException msalex)
                {
                    return new LoginResult()
                    {
                        Status = LoginStatus.Error,
                        AccessToken = null,
                        Error = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}"
                    };
                }
            }
            catch (Exception ex)
            {

                return new LoginResult()
                {
                    Status = LoginStatus.Error,
                    AccessToken = null,
                    Error = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}"
                };
            }

            if (authResult != null)
            {
                AccessToken = authResult.AccessToken;
                return new LoginResult()
                {
                    Status = LoginStatus.Success,
                    AccessToken = AccessToken,
                    Error = null
                };
            }

            return new LoginResult()
            {
                Status = LoginStatus.Error,
                AccessToken = null,
                Error = null
            }; ;
        }

        public async Task<string> SignOutAsync()
        {
            IEnumerable<IAccount> accounts = await _client.GetAccountsAsync().ConfigureAwait(false);
            IAccount firstAccount = accounts.FirstOrDefault();

            try
            {
                await _client.RemoveAsync(firstAccount).ConfigureAwait(false);
                AccessToken = null;
                return null;
            }
            catch (MsalException ex)
            {
                return $"Error signing out user: {ex.Message}";
            }
        }
    }
}
