using dotnet.Core;
using dotnet.Extensions;
using dotnet.Services.Testing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // JWT configuration would go here - for now, we just need the scheme registered
        // Real configuration should be loaded from appsettings.json
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseRequestLocalization();

app.Run();

// Expose Program for integration testing
public partial class Program { }
