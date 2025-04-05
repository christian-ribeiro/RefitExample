using Refit;
using RefitExampe.ApiClient.Interface.Service.Microservice.Authentication;
using RefitExampe.ApiClient.Refit.Microservice.Endpoint.Authentication;
using RefitExampe.ApiClient.Refit.Microservice.Endpoint.Customer;
using RefitExampe.ApiClient.Refit.Microservice.Endpoint.User;
using RefitExampe.ApiClient.Refit.Microservice.Handler;
using RefitExampe.ApiClient.Service.Microservice.Authentication;
using RefitExample.Arguments.Enum.Microservice;
using RefitExample.Domain.Interface.Service.User;
using RefitExample.Domain.Service.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<MicroserviceHandler>();
builder.Services.AddTransient<MicroserviceAuthenticationHandler>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddRefitClient<IMicroserviceAuthenticationRefit>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://reqres.in"))
    .AddHttpMessageHandler<MicroserviceAuthenticationHandler>();

builder.Services.AddRefitClient<IMicroserviceUserRefit>()
    .ConfigureHttpClient((client) =>
    {
        client.BaseAddress = new Uri("https://reqres.in");
        client.DefaultRequestHeaders.Add("X-Refit-Client", nameof(EnumMicroservice.DrugTrafficking));
    })
    .AddHttpMessageHandler<MicroserviceHandler>();

builder.Services.AddRefitClient<IMicroserviceCustomerRefit>()
    .ConfigureHttpClient((client) =>
    {
        client.BaseAddress = new Uri("https://reqres.in");
        client.DefaultRequestHeaders.Add("X-Refit-Client", nameof(EnumMicroservice.Pimp));
    })
    .AddHttpMessageHandler<MicroserviceHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
