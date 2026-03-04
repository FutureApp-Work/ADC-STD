using dotnet.Core;
using dotnet.Extensions;
using dotnet.Services.Testing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TestingDBContext>(options =>
{
  options.UseMySql(builder.Configuration.GetConnectionString("TestingDB"),
                   MariaDbServerVersion.Create(new Version(11, 8), ServerType.MariaDb));
});

// Add services to the container.
builder.Services
       .AddModelBindingProvider()
       .AddFilterAttribute()
       .AddJsonSerializerOption()
       .AddServiceRegistration()
       .AddSupportedCulture(builder.Configuration)
       .AddAPIVersioning()
       .AddMvc();

// Add API Explorer for Swagger
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT Bearer auth support
builder.Services.AddSwaggerGen(options =>
{
    // Add API version info
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ADC-STD API",
        Version = "v1",
        Description = "ADC-STD Web API with JWT Authentication",
        Contact = new OpenApiContact
        {
            Name = "ADC-STD Team"
        }
    });

    // Include XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Add JWT Bearer authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Add security requirement
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // JWT configuration would go here - for now, we just need the scheme registered
        // Real configuration should be loaded from appsettings.json
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable Swagger in all environments (Development AND Production)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ADC-STD API V1");
    options.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseRequestLocalization();

app.Run();

// Expose Program for integration testing
public partial class Program { }
