using Microsoft.Extensions.Caching.Memory;
using MotorCentroDistribuicao.Configurations;
using MotorCentroDistribuicao.Domain.Providers.Rest;
using MotorCentroDistribuicao.Domain.Providers.Rest.Dtos;
using System.Net;
using System.Text.Json;

namespace MotorCentroDistribuicao.Providers.Rest
{
    public class CentroDistribuicaoProvider(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        IConfiguration configuration) : ICentroDistribuicaoProvider
    {

        private readonly HttpClient httpClient =
            httpClientFactory.CreateClient(HttpClientServiceConfig.HTTP_CLIENT_CD);

        public async Task<CentroDistribuicaoProviderDto> GetCentrosDistribuicaoPorItem(long item)
        {
            var cacheExpiresIn = configuration.GetRequiredSection("Http")["CachePeriodInMinutes"];
            var url = $"/distribuitioncenters?itemId={item}";

            var cachedResponse = cache.GetOrCreateAsync(url, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(int.Parse(cacheExpiresIn));

                var response = await httpClient.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return new CentroDistribuicaoProviderDto { CentrosDistribuicao = null };

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<CentroDistribuicaoProviderDto>(json);
            });

            return await cachedResponse;
        }
    }
}
