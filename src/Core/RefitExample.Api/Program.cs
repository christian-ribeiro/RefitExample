using Refit;
using RefitExample.ApiClient.Interface.Service.Microservice.Authentication;
using RefitExample.ApiClient.Refit.Extensions;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Authentication;
using RefitExample.ApiClient.Refit.Microservice.Handler;
using RefitExample.ApiClient.Refit.Microservice.Interface;
using RefitExample.ApiClient.Service.Microservice.Authentication;
using RefitExample.Domain.Interface.Service.User;
using RefitExample.Domain.Service.User;

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

builder.Services.AppendRefitInterfaces<IMicroserviceRefitInterface>(builder => builder.AddHttpMessageHandler<MicroserviceHandler>());
#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
