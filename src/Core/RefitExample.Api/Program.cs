using Refit;
using RefitExample.ApiClient.Interface.Service.Microservice.Authentication;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Authentication;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.DrugTrafficking;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Pimp;
using RefitExample.ApiClient.Refit.Microservice.Handler;
using RefitExample.ApiClient.Service.Microservice.Authentication;
using RefitExample.Domain.Interface.Service.User;
using RefitExample.Domain.Service.User;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();

#region Refit
builder.Services.AddTransient<MicroserviceHandler>();
builder.Services.AddTransient<MicroserviceAuthenticationHandler>();

builder.Services.AddRefitClient<IMicroserviceAuthenticationRefit>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://reqres.in"))
    .AddHttpMessageHandler<MicroserviceAuthenticationHandler>();

string pattern = @"IMicroservice(.*?)Refit";

builder.Services.AddRefitClient<IMicroserviceDrugTraffickingRefit>()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("https://reqres.in");
        client.DefaultRequestHeaders.Add(MicroserviceHandler.RefitClientHeader, Regex.Match(nameof(IMicroserviceDrugTraffickingRefit), pattern).Groups[1].Value);
    })
    .AddHttpMessageHandler<MicroserviceHandler>();

builder.Services.AddRefitClient<IMicroservicePimpRefit>()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("https://reqres.in");
        client.DefaultRequestHeaders.Add(MicroserviceHandler.RefitClientHeader, Regex.Match(nameof(IMicroservicePimpRefit), pattern).Groups[1].Value);
    })
    .AddHttpMessageHandler<MicroserviceHandler>();
#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
