using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace MotorCentroDistribuicao.Configurations
{
    public static class LogServiceConfig
    {

        private const string OUTPUT_TEMPLATE = "[{Timestamp:HH:mm:ss} {Level:u3} {AppName}] [CorrelationId: {correlation_id}] {Message:lj}{NewLine}{Exception}";

        public static void ConfigureLog(this WebApplicationBuilder builder)
        {

            var logFormater = new JsonFormatter();

            Log.Logger = new LoggerConfiguration()
                                .Enrich
                                    .FromLogContext()
                                .Enrich
                                    .WithProperty("AppName", "Motor CD - Test")
                                .MinimumLevel
                                    .Override("Microsoft", LogEventLevel.Information)
                                .MinimumLevel
                                    .Override("System", LogEventLevel.Information)
                                .MinimumLevel
                                    .Information()
                                .WriteTo
                                    .Console(outputTemplate: OUTPUT_TEMPLATE)
                                .WriteTo
                                    .File(logFormater, "logs/log.txt", rollingInterval: RollingInterval.Day)
                                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}