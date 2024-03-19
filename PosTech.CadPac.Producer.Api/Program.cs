using MassTransit;
using Microsoft.OpenApi.Models;
using PosTech.CadPac.Domain.Services;
using PosTech.CadPac.Producer.Api.Authentication;
using PosTech.CadPac.Producer.Api.Service;
using PosTech.CadPac.Producer.Api.Swagger;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddAzureAppConfiguration(Environment.GetEnvironmentVariable("postechazappconfiguration"))
    .Build();

var builder = WebApplication.CreateBuilder(args);

var clientId = configuration["postechcadpac:clientId"] ?? string.Empty;
var clientSecret = configuration["postechcadpac:clientSecret"] ?? string.Empty;
var baseHash = configuration["postechcadpac:clientbasehash"] ?? string.Empty;

builder.Host.ConfigureServices(services =>
{
    var authenticationCredentials = new AuthenticationCredentials(clientId, clientSecret, baseHash);
    configuration.GetSection("TokenService").Bind(authenticationCredentials);

    services.AddSingleton(r => authenticationCredentials);

    services.AddSingleton<ITokenService, TokenService>();
});
     

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PosTech.CadPac.Producer.Api",
        Version = "v1"
    });

    options.OperationFilter<SwaggerFilterOptions>();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
            },
            new List<string>()
         }
    });
});

builder.Services.ConfigureOptions<JwtBearerOptionsConfiguration>()
    .AddAuthentication("Bearer")
    .AddJwtBearer();

var conexao = configuration["postechcadpac:masstransit:azurebus"] ?? string.Empty;

builder.Services.AddMassTransit((x => {
    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(conexao);
    });
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
