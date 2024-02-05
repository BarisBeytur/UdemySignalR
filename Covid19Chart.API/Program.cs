using Covid19Chart.API.Hubs;
using Covid19Chart.API.Models;
using Covid19Chart.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddSignalR();
builder.Services.AddScoped<CovidService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<CovidHub>("/CovidHub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
