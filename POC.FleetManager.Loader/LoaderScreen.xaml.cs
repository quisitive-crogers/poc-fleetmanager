using POC.FleetManager.Common.Events;

namespace POC.FleetManager.Loader;

public partial class LoaderScreen : ContentPage
{
    private readonly AuthService authService;
    private readonly WebServiceCatalog webServiceCatalog;
    private readonly EventAggregator eventAggregator;

    public LoaderScreen(AuthService authService, WebServiceCatalog webServiceCatalog, EventAggregator eventAggregator)
    {
        InitializeComponent();
        this.authService = authService;
        this.webServiceCatalog = webServiceCatalog;
        this.eventAggregator = eventAggregator;
        eventAggregator.Subscribe(nameof(ExtensionEvent), OnExtensionsEvent);
        LoadAsync();
    }

    async Task OnExtensionsEvent(EventData e)
    {
        lblStatus.Text = e.Payload.First().Value.ToString();
        await Task.CompletedTask;
    }

    private async void LoadAsync()
    {
        lblStatus.Text = "Authenticating User...";
        var isAuthenticated = await authService.AuthenticateAsync();
        if (isAuthenticated)
        {
            await webServiceCatalog.LoadExtensionsAsync();
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await DisplayAlert("Error", "Authentication failed. Please check your Entra ID credentials.", "OK");
            Application.Current!.Quit();
        }
    }
}