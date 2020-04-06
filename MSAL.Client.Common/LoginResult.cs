namespace MSAL.Client
{
    public enum LoginStatus
    {
        Success,
        Warning,
        Error
    }

    public class LoginResult
    {
        public LoginStatus Status { get; set; }
        public string AccessToken { get; set; }
        public string Error { get; set; }

    }
}
