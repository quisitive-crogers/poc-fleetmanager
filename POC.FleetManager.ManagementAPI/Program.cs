using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using POC.FleetManager.Common;
using POC.FleetManager.ManagementAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
.Build();

var azureAppConfigSettings = config.GetSection("AzureAppConfiguration").Get<AzureAppConfigSettings>()!;

// Add Azure App Configuration as a configuration source
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(azureAppConfigSettings!.ConnectionString)
           .ConfigureRefresh(refresh =>
           {
               refresh.RegisterAll();
               refresh.SetRefreshInterval(TimeSpan.FromMinutes(5));
           });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("API.Read", policy =>
    {
        policy.RequireScope(builder.Configuration.GetSection("AzureAd")["Scopes"]!);
    });

var app = builder.Build();

app.UseRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Console.WriteLine("navigate to /openapi/v1.json to get endpoints");

app.Run();
