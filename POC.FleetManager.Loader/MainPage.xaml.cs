using POC.FleetManager.Common;
using POC.FleetManager.Common.Events;

namespace POC.FleetManager.Loader;

public partial class MainPage : ContentPage
{
    private readonly WebServiceSettings webServiceSettings;
    private readonly HttpClient httpClient;
    private readonly AuthService authService;
    private readonly WebServiceCatalog webServiceCatalog;
    private readonly ExtensionHostService extensionHostService;

    public MainPage(WebServiceSettings webServiceSettings, HttpClient httpClient, AuthService authService, WebServiceCatalog webServiceCatalog, EventAggregator eventAggregator, ExtensionHostService extensionHostService)
    {
        InitializeComponent();
        this.webServiceSettings = webServiceSettings;
        this.httpClient = httpClient;
        this.authService = authService;
        this.webServiceCatalog = webServiceCatalog;
        this.extensionHostService = extensionHostService;
        var menu1 = new MenuBarItem { Text = "Menu 1" };

        this.MenuBarItems.Add(menu1);

        eventAggregator.Subscribe(nameof(ExtensionEvent), OnExtensionEvent);

        GetUserInfo();

        Task.Delay(1000);
        LoadMenu();
    }
    
    private async Task OnExtensionEvent(EventData data)
    {
        if (data.Payload.FindTupleByKey("Message")!.Value.ToString() != "Extensions loaded!") return;
        LoadMenu();
        await Task.CompletedTask;
    }

    private void LoadMenu()
    {
        this.MenuBarItems[0].Clear();

        foreach (var extension in extensionHostService.GetExtensions())
        {
            var subMenu = new MenuFlyoutItem { Text = extension.Name, CommandParameter = extension };
            subMenu.Clicked += OnMenuItemClicked;
            this.MenuBarItems[0].Add(subMenu);
        }
    }

    private async void GetUserInfo()
    {
        var accessToken = await authService.GetAccessTokenAsync();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.GetStringAsync($"{webServiceSettings.BaseUrl}/hello");

        btnHello.Text = response ?? "Hello Today!";
    }

    private async void OnMenuItemClicked(object? sender, EventArgs e)
    {
        var menuItem = sender as MenuFlyoutItem;
        var extension = menuItem!.CommandParameter as IExtension;

        lblCommand.Text = $"{extension!.Name} menu clicked";

        await Task.CompletedTask;
    }
}
