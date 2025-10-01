using LIT.Smabu.UseCases;
using LIT.Smabu.Domain;
using LIT.Smabu.Infrastructure;
using Microsoft.OpenApi.Models;
using LIT.Smabu.API.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LIT.Smabu.API.Middlewares;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

QuestPDF.Settings.License = LicenseType.Community;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

var azureClientId = builder.Configuration["AzureAD:ClientId"]!;
var azureClientSecret = builder.Configuration["AzureAD:ClientSecret"]!;
var azureTenantId = builder.Configuration["AzureAD:TenantId"]!;
var azureIssuer = builder.Configuration["AzureAD:Issuer"]!;
var azureAudience = azureClientId;

const string DevCorsPolicy = "DevCors";
const string ProdCorsPolicy = "ProdCors";

builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy(DevCorsPolicy, policy =>
            policy
                .SetIsOriginAllowed(origin =>
                {
                    if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri)) return false;
                    if (!string.Equals(uri.Host, "localhost", StringComparison.OrdinalIgnoreCase)) return false;
                    return uri.Scheme is "http" or "https";
                })
                .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS")
                .WithHeaders("Authorization", "Content-Type", "Accept")
                // Nur aktivieren falls du Cookies / SignalR / Authorization Header mit Credentials brauchst:
                //.AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromMinutes(30))
        );
    }
    else
    {
        // Produktion: Origins aus Konfiguration (appsettings / Secrets)
        // Example in appsettings:
        // "Cors": { "AllowedOrigins": [ "https://app.example.com", "https://portal.example.com" ] }
        string[] allowed = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        options.AddPolicy(ProdCorsPolicy, policy =>
        {
            if (allowed.Length == 0)
            {
                // Failsafe: nichts freigeben, frühzeitig im Log warnen
                policy.WithOrigins("http://invalid-origin.local");
            }
            else
            {
                policy.WithOrigins(allowed)
                      .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS")
                      .WithHeaders("Authorization", "Content-Type", "Accept");
            }
            // Kein AllowCredentials standardmäßig in Prod ohne Notwendigkeit
            policy.SetPreflightMaxAge(TimeSpan.FromHours(1));
        });
    }
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.IncludeErrorDetails = true;
        options.Audience = azureAudience;
        options.Authority = azureIssuer;
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidIssuer = azureIssuer;
        options.TokenValidationParameters.ValidAudience = azureAudience;
        options.TokenValidationParameters.ValidateLifetime = true;

        options.TokenHandlers.Clear();
        options.TokenHandlers.Add(new CustomJwtSecurityTokenHandler());
        options.UseSecurityTokenValidators = false;
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(azureClientSecret));
        options.Validate();
    });

builder.Services.AddMemoryCache();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
AddSwagger(builder, azureClientId);
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddDomainServices();
builder.Services.AddUseCasesServices();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.{json|yaml}";
    });
    app.UseSwaggerUI(options =>
    {
        options.OAuthAppName("Swagger Client");
        options.OAuthClientId(azureClientId);
        options.OAuthClientSecret(azureClientSecret);
        options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    });
}

app.SeedDatabaseAsync().GetAwaiter();

// CORS muss vor Auth/Endpoints bleiben
app.UseCors(builder.Environment.IsDevelopment() ? DevCorsPolicy : ProdCorsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.RegisterCommonEndpoints();
app.RegisterDashboardEndpoints();
app.RegisterCatalogsEndpoints();
app.RegisterCustomersEndpoints();
app.RegisterInvoicesEndpoints();
app.RegisterOrdersEndpoints();
app.RegisterOffersEndpoints();
app.RegisterPaymentsEndpoints();
app.RegisterFinancialEndpoints();

app.Run();

static void AddSwagger(WebApplicationBuilder builder, string azureClientId)
{
    builder.Services.AddSwaggerGen(c =>
    {
        Dictionary<string, string> scopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ')?.ToDictionary(x => x) ?? [];
        scopes.Add($"api://{azureClientId}/access_as_user", "Access application on user behalf");
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
}