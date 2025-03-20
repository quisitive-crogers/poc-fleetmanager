namespace POC.FleetManager.Loader;

public partial class MainPage : ContentPage
{
    private readonly WebServiceSettings webServiceSettings;
    private readonly HttpClient httpClient;
    private readonly AuthService authService;
    int counter = 0;

    public MainPage(WebServiceSettings webServiceSettings, HttpClient httpClient, AuthService authService)
    {
        InitializeComponent();
        this.webServiceSettings = webServiceSettings;
        this.httpClient = httpClient;
        this.authService = authService;
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        counter++;

        if (counter == 1)
            CounterBtn.Text = $"Clicked {counter} time";
        else
            CounterBtn.Text = $"Clicked {counter} times";

        SemanticScreenReader.Announce(CounterBtn.Text);

        var accessToken = await authService.GetAccessTokenAsync();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.GetStringAsync($"{webServiceSettings.BaseUrl}/hello");

        btnHello.Text = response ?? "Hello Today!";

    }
}
