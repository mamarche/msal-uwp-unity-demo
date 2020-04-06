using MSAL.Client;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MSAL.Test.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        LoginManager client;
        string clientId = "[your clientId here]";
        string[] scopes = new string[] { "user.read" };

        public MainPage()
        {
            this.InitializeComponent();

            client = new LoginManager(clientId, scopes);
        }

        private async void signInButton_Click(object sender, RoutedEventArgs e)
        {
            var res = await client.AcquireToken();
            if (res.Status == LoginStatus.Success)
            {
                resultText.Text = $"Token: {res.AccessToken}";
            }
            else
            {
                resultText.Text = "ERROR: " + res.Error;
            }
        }

        private async void signOutButton_Click(object sender, RoutedEventArgs e)
        {
            var res = await client.SignOutAsync();
            if (res == null)
            {
                resultText.Text = "User Signed Out";
            }
            else
            {
                resultText.Text = $"ERROR: {res}";
            }
        }
    }
}
