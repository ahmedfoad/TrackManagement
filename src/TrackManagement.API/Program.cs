using TrackManagement.API;
using TrackManagement.Application;
using TrackManagement.Infrastructure;
using TrackManagement.Infrastructure.Extensions;
using TrackManagement.Infrastructure.Seeders;
using TrackManagement.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.


builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationServices()
    .AddPersistenceServices(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Seeding Data
await app.Services.InitializeDatabaseAsync();
await app.Services.UserDataSeederAsync();

app.UseSwagger()
   .UseSwaggerUI()
   .UseRouting();

//app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
