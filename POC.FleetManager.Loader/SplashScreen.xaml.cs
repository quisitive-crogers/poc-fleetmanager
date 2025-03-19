using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace POC.FleetManager.Loader
{
    public partial class SplashScreen : ContentPage
    {
        private readonly AuthService _authService;

        public SplashScreen(AuthService authService)//, ConfigService configService, ExtensionService extensionService)
        {
            InitializeComponent();
            _authService = authService;
            LoadAsync();
        }

        private async void LoadAsync()
        {
            await Task.Delay(2000); // Simulate splash delay
            var isAuthenticated = await _authService.AuthenticateAsync();
            if (isAuthenticated)
            {
                //var appConfig = _configService.GetAppConfig();
                //await _extensionService.LoadExtensionsAsync("demo-user");
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await DisplayAlert("Error", "Authentication failed. Please check your Entra ID credentials.", "OK");
                Application.Current!.Quit();
            }
        }
    }
}