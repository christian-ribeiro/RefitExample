using Refit;
using RefitExample.ApiClient.DataAnnotation;
using RefitExample.ApiClient.Interface.Service.Microservice.Authentication;
using RefitExample.ApiClient.Refit.Extensions;
using RefitExample.ApiClient.Refit.Microservice.Configuration;
using RefitExample.ApiClient.Refit.Microservice.Endpoint.Authentication;
using RefitExample.ApiClient.Refit.Microservice.Handler;
using RefitExample.ApiClient.Refit.Microservice.Interface;
using RefitExample.ApiClient.Service.Microservice.Authentication;
using RefitExample.Domain.Interface.Service.User;
using RefitExample.Domain.Service.User;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();

#region Refit
#region Validate
//Valida se alguma interface do Refit do tipo IMicroserviceRefitInterface não possui o DataAnnotation
var missing = typeof(IBaseRefitInterface).Assembly.GetTypes()
    .Where(t => typeof(IMicroserviceRefitInterface).IsAssignableFrom(t) && t.IsInterface && t != typeof(IMicroserviceRefitInterface))
    .Where(x => x.GetCustomAttribute<MicroserviceRefitAttribute>() == null).ToList();

if (missing.Count != 0)
    throw new InvalidOperationException($"As seguintes interfaces estão sem o atributo MicroserviceName: {string.Join(", ", missing.Select(m => m.Name))}");
#endregion

builder.Services.AddTransient<MicroserviceHandler>();
builder.Services.AddTransient<MicroserviceAuthenticationHandler>();

builder.Services.AddRefitClient<IMicroserviceAuthenticationRefit>()
    .ConfigureHttpClient(client => client.BaseAddress = MicroserviceEnvironmentVariable.BaseAddress)
    .AddHttpMessageHandler<MicroserviceAuthenticationHandler>();

builder.Services.AppendRefitInterfaces<IMicroserviceRefitInterface>(
    httpClientConfigurator: (client, type) =>
    {
        client.BaseAddress = MicroserviceEnvironmentVariable.BaseAddress;

        var microserviceRefitAttribute = type.GetCustomAttribute<MicroserviceRefitAttribute>() ?? type.GetInterfaces()
               .Select(i => i.GetCustomAttribute<MicroserviceRefitAttribute>())
               .FirstOrDefault(attr => attr != null);

        if (microserviceRefitAttribute != null)
        {
            client.DefaultRequestHeaders.Add(MicroserviceHandler.RefitClientHeader, microserviceRefitAttribute.Microservice.ToString());
        }
    },
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