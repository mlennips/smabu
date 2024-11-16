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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder =>
        {
            builder.WithOrigins("http://localhost", "http://localhost:5173", "http://localhost:*")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
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

app.UseCors("AllowLocalhost");
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