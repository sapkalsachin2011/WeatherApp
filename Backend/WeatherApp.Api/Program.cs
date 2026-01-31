using Polly;
using Polly.Extensions.Http;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var weatherDataDir = Path.GetFullPath(Path.Combine("..", "..", "Shared", "weather-data"));
builder.Services.AddSingleton<string>(weatherDataDir);
// Register WeatherService as a typed client with Polly retry policy for transient HTTP errors
builder.Services.AddHttpClient<WeatherApp.Api.Services.WeatherService>()
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5291")
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
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers().RequireCors("AllowAll");
app.Run();