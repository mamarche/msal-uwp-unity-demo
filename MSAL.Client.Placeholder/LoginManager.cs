using System;
using System.Threading.Tasks;

namespace MSAL.Client
{
    public class LoginManager : ILoginManager
    {
        public LoginManager(string clientId, string[] scopes)
        {
            this.Init(clientId, scopes);
        }

        public async Task<LoginResult> AcquireToken()
        {
            return new LoginResult() { Status = LoginStatus.Success, 
                AccessToken = "FakeAccessToken", 
                Error = null };
        }

        public void Init(string clientId, string[] scopes)
        {
            
        }

        public Task<string> SignOutAsync()
        {
            return null;
        }
    }
}
