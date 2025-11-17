using dotnet.Core;
using dotnet.Extensions;
using dotnet.Services.Testing;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseRequestLocalization();

app.Run();
