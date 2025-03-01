
using MotorCentroDistribuicao.Configurations;
using System.Text.Json.Serialization;

namespace MotorCentroDistribuicao
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureDependencyInjection();
            builder.Services.ConfigureHttpClient(builder.Configuration);
            builder.Services.ConfigureAutoMapper();
            builder.Services.ConfigureMemoryCache();
            builder.Services.ConfigureDatabase();

            builder.ConfigureLog();
            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
