using UdemySignalR.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Sonradan eklenenler
builder.Services.AddSignalR();
builder.Services.AddCors(o => 
    o.AddPolicy("CorsPolicy", builder => { builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .WithOrigins("http://localhost:44324", "http://localhost:21093", "http://localhost:44324", "http://localhost:5067", "https://localhost:7294");
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();


// http://localhost:4400/MyHub
app.MapHub<MyHub>("/MyHub");

app.Run();
