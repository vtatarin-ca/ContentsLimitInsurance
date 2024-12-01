using Data;
using NLog.Web;
using NLog;
using Api.Common;

var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

try
{
  var builder = WebApplication.CreateBuilder(args);

  // Add services to the container.

  builder.Services.AddControllers();
  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();

  builder.Logging.ClearProviders();
  builder.Host.UseNLog();

  builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
  var settings = builder.Configuration.GetSection("WebUISettings").Get<WebUISettings>();
  if (string.IsNullOrWhiteSpace(settings?.OriginUrl))
    throw new ApplicationException("WebUISettings.OriginUrl is null or empty, please make sure you have correct URL there (e.g. appsettings*.json");

  // Add custom services
  Api.IoC.MapServices.Map(builder.Services);

  builder.Services.AddCors(options =>
  {
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins(settings.OriginUrl) // Replace with your frontend's URL
                        .AllowAnyHeader()
                        .AllowAnyMethod());
  });

  var app = builder.Build();

  // Use CORS policy
  app.UseCors("AllowSpecificOrigin");

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }

  app.UseHttpsRedirection();

  app.UseAuthorization();

  app.MapControllers();

  app.Run();
}
catch (Exception ex)
{
  logger.Error(ex);
  throw;
}
finally
{
  NLog.LogManager.Shutdown();
}