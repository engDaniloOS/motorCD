
using Microsoft.AspNetCore.Mvc;

namespace ConsultaCentroDistribuicaoMock
{
    public class Program
    {
        public static int TempoDelay { get; set; }
        public static int PercentualError { get; set; }
        public static Dictionary<int, List<string>> ItensECdsRelacionados { get; set; }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            SetUpVariaveis();

            app.MapGet("/distribuitioncenters", async ([FromQuery(Name = "itemId")] int itemId) =>
            {
                try
                {
                    TryLancarErro();
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }

                var cds = new List<string>();
                var existsItem = ItensECdsRelacionados.TryGetValue(itemId, out cds);

                await AddDelay();

                if (existsItem)
                    return Results.Ok(new { distribuitionCenters = cds });

                return Results.NotFound(new { message = "Item fora de estoque" });
            });

            app.Run();
        }

        private static void SetUpVariaveis()
        {
            PercentualError = int.Parse(Environment.GetEnvironmentVariable("ERROR")!);
            TempoDelay = int.Parse(Environment.GetEnvironmentVariable("DELAY")!);

            var isTodosCds =
                bool.Parse(Environment.GetEnvironmentVariable("TODOS_CDS")!);

            ItensECdsRelacionados =
                isTodosCds ? Itens.GetAll() : Itens.GetListaReduzida();
        }

        private static void TryLancarErro()
        {
            if (PercentualError == 0) return;

            var random = new Random();
            var numeroAleatorio = random.Next(1, 101);

            if (numeroAleatorio <= PercentualError)
                throw new Exception();
        }

        private async static Task AddDelay()
        {
            if (TempoDelay == 0)
                return;

            await Task.Delay(TempoDelay);
        }
    }
}
