namespace POC.FleetManager.Loader;

public partial class LoaderScreen : ContentPage
{
    private readonly AuthService authService;

    public LoaderScreen(AuthService authService)
    {
        InitializeComponent();
        this.authService = authService;
        LoadAsync();
    }

    private async void LoadAsync()
    {
        await Task.Delay(2000); // Simulate splash delay
        var isAuthenticated = await authService.AuthenticateAsync();
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