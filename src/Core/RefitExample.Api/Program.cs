using Refit;
using RefitExample.ApiClient.DataAnnotation;
using RefitExample.ApiClient.Refit.Extensions;
using RefitExample.ApiClient.Refit.Microservice.Configuration;
using RefitExample.ApiClient.Refit.Microservice.Handler;
using RefitExample.ApiClient.Refit.Microservice.Interface;
using RefitExample.ApiClient.Refit.MockEndpoint.Authentication;
using RefitExample.ApiClient.Refit.MockEndpoint.Credential;
using RefitExample.Arguments.Const;
using RefitExample.Arguments.Extension;
using RefitExample.Domain.Interface.Service.User;
using RefitExample.Domain.Service.User;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();

#region Refit
//Tratativa para garantir que nenhum Refit que herde de IMicroserviceRefitInterface esteja sem o DataAnnotation
var missing = typeof(IBaseRefitInterface).Assembly.GetTypes()
    .Where(t => typeof(IMicroserviceRefitInterface).IsAssignableFrom(t) && t.IsInterface && t != typeof(IMicroserviceRefitInterface))
    .Where(x => x.GetCustomAttributeFromHierarchy<MicroserviceRefitAttribute>() == null).ToList();

if (missing.Count != 0)
    throw new InvalidOperationException($"As seguintes interfaces estão sem o atributo MicroserviceName: {string.Join(", ", missing.Select(m => m.Name))}");

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

        var microserviceRefitAttribute = type.GetCustomAttributeFromHierarchy<MicroserviceRefitAttribute>();

        if (microserviceRefitAttribute != null)
            client.DefaultRequestHeaders.Add(ConfigurationConst.RefitClientHeader, microserviceRefitAttribute.Microservice.ToString());
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