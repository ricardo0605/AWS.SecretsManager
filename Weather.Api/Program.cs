using Weather.Api.Models;
using Weather.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

builder.Configuration.AddSecretsManager(configurator: options =>
{
    options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}");
    options.KeyGenerator = (_, s) => s
        .Replace($"{env}_{appName}_", string.Empty)
        .Replace("__", ":");


    // This will be an interval that our service will poll for a secret update.
    // I'm gonna to leave this short for study purposes but ultimately you should
    // only be doing that in an interval that makes sense because we are paying per
    // request on secret manager.
    options.PollingInterval = TimeSpan.FromSeconds(10);
});



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<OpenWeatherApiSettings>(builder.Configuration.GetSection(OpenWeatherApiSettings.Key));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IWeatherService, WeatherService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
