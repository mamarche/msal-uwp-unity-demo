using System;
using System.Threading.Tasks;

namespace MSAL.Client.Placeholder
{
    public class LoginManager : ILoginManager
    {
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
