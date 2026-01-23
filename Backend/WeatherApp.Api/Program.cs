
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register WeatherService with HttpClient and weather-data path
var weatherDataDir = Path.GetFullPath(Path.Combine("..", "..", "Shared", "weather-data"));
builder.Services.AddSingleton(sp =>
    new WeatherApp.Api.Services.WeatherService(
        new HttpClient(),
        weatherDataDir,
        sp.GetRequiredService<IConfiguration>()
    )
);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
