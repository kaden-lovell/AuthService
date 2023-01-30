// TODO: fill in these fields
using AuthService;
using Microsoft.AspNetCore;
using Serilog;
using Serilog.Events;
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json").AddJsonFile($"config.{environment}.json", true).Build();

var logging = new Logging();

configuration.GetSection("Logging").Bind(logging);

if (string.IsNullOrWhiteSpace(logging.OutputTemplate))
{
    Log.Logger =
      new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", environment == "development" ? LogEventLevel.Warning : LogEventLevel.Error)
        //.MinimumLevel.Override("Microsoft.AspNetCore.SignalR", LogEventLevel.Debug)
        //.MinimumLevel.Override("Microsoft.AspNetCore.Http.Connections", LogEventLevel.Debug)
        .Enrich.FromLogContext()
        .WriteTo.File(new MessageFormatter(), logging.PathFormat, logging.LogLevel, logging.FileSizeLimit, retainedFileCountLimit: logging.FileCountLimit, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
        .CreateLogger();
}
else
{
    // Example output template: {Timestamp:yyyy-MM-dd hh:mm:ss tt zzz} [{Level}] {Message}{NewLine}{Exception}
    Log.Logger =
      new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", environment == "development" ? LogEventLevel.Warning : LogEventLevel.Error)
        //.MinimumLevel.Override("Microsoft.AspNetCore.SignalR", LogEventLevel.Debug)
        //.MinimumLevel.Override("Microsoft.AspNetCore.Http.Connections", LogEventLevel.Debug)
        .Enrich.FromLogContext()
        .WriteTo.File(logging.PathFormat, logging.LogLevel, logging.OutputTemplate, null, logging.FileSizeLimit, retainedFileCountLimit: logging.FileCountLimit, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
        .CreateLogger();
}

try
{
    Log.Information("Starting web host");
    BuildWebHost(args).Run();
    
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally {
    Log.CloseAndFlush();
}

static IWebHost BuildWebHost(string[] args) =>
  WebHost.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .UseSerilog()
    .Build();

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
