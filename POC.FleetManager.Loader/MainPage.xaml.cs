using POC.FleetManager.Common;

namespace POC.FleetManager.Loader;

public partial class MainPage : ContentPage
{
    private readonly WebServiceSettings webServiceSettings;
    private readonly HttpClient httpClient;
    private readonly AuthService authService;
    private readonly ExtensionService extensionService;


    public MainPage(WebServiceSettings webServiceSettings, HttpClient httpClient, AuthService authService, ExtensionService extensionService)
    {
        InitializeComponent();
        this.webServiceSettings = webServiceSettings;
        this.httpClient = httpClient;
        this.authService = authService;
        this.extensionService = extensionService;
        var menu1 = new MenuBarItem { Text = "Menu 1" };
        var menu1Sub1 = new MenuFlyoutItem { Text = "Sub 1", CommandParameter = "Menu Item 1 Sub Item 1" };
        menu1Sub1.Clicked += OnMenuItemClicked;
        menu1.Add(menu1Sub1);

        this.MenuBarItems.Add(menu1);

        GetUserInfo();
    }
    //private async Task LoadExtensionsAsync()
    //{
    //    var host = await extensionService.LoadExtensionsAsync("demo-user");
    //    var extensions = host.GetExports<IExtension>();
    //    foreach (var ext in extensions)
    //    {
    //        await ext.Initialize(_configuration, _logger);
    //        await ext.Run();

    //        // Add the extension's view as a tab
    //        var tab = new TabViewItem { Text = ext.Name };
    //        var view = ext.GetView();
    //        if (view != null)
    //        {
    //            view.BindingContext = _eventsService; // Pass IEventsService to the view
    //            tab.Content = view;
    //            ExtensionTabs.Items.Add(tab);
    //        }
    //    }
    //    StatusLabel.Text = $"Environment: {_configService.GetAppConfig().Environment}, Extensions: {extensions.Count()}";
    //}

    private async void GetUserInfo()
    {
        var accessToken = await authService.GetAccessTokenAsync();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.GetStringAsync($"{webServiceSettings.BaseUrl}/hello");

        btnHello.Text = response ?? "Hello Today!";
    }

    private void OnMenuItemClicked(object? sender, EventArgs e)
    {
        var menuItem = sender as MenuFlyoutItem;
        lblCommand.Text = $"{menuItem!.CommandParameter} Clicked";
    }
}
