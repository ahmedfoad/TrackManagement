using TrackManagement.Application;
using TrackManagement.API;
using TrackManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.


builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationServices()
    .AddPersistenceServices(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

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
app.UseAuthorization();
app.MapControllers();

app.Run();
