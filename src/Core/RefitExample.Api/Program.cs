using Refit;
using RefitExample.ApiClient.DataAnnotation;
using RefitExample.ApiClient.Refit.Extensions;
using RefitExample.ApiClient.Refit.Microservice.Configuration;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Authentication;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Credential;
using RefitExample.ApiClient.Refit.Microservice.Handler;
using RefitExample.ApiClient.Refit.Microservice.Interface;
using RefitExample.Domain.Interface.Service.User;
using RefitExample.Domain.Service.User;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();

#region Refit
var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    })
};

builder.Services.AddTransient<MicroserviceHandler>();
builder.Services.AddTransient<MicroserviceAuthenticationHandler>();

builder.Services.AddRefitClient<IMicroserviceAuthenticationRefit>(refitSettings)
    .ConfigureHttpClient(client => client.BaseAddress = MicroserviceEnvironmentVariable.BaseAddress)
    .AddHttpMessageHandler<MicroserviceAuthenticationHandler>();

builder.Services.AddRefitClient<IMicroserviceCredentialRefit>(refitSettings)
    .ConfigureHttpClient(client => client.BaseAddress = MicroserviceEnvironmentVariable.BaseAddress)
    .AddHttpMessageHandler<MicroserviceAuthenticationHandler>();

builder.Services.AppendRefitInterfaces<IMicroserviceRefitInterface>(
    httpClientConfigurator: (client, type, _) =>
    {
        client.BaseAddress = MicroserviceEnvironmentVariable.BaseAddress;

        var microserviceRefitAttribute = type.GetCustomAttribute<MicroserviceRefitAttribute>();

        if (microserviceRefitAttribute != null)
            client.DefaultRequestHeaders.Add(MicroserviceHandler.RefitClientHeader, microserviceRefitAttribute.Microservice.ToString());
    },
    refitSettings: refitSettings,
    configureHttpClientBuilder: builder => builder.AddHttpMessageHandler<MicroserviceHandler>()
);
#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();