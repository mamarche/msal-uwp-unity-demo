using System.Threading.Tasks;

namespace MSAL.Client
{
    public interface ILoginManager
    {
        void Init(string clientId, string[] scopes);
        Task<LoginResult> AcquireToken();
        Task<string> SignOutAsync();
    }
}
