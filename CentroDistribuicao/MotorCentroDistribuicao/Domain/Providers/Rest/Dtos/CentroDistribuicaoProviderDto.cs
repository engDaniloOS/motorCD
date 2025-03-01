using System.Text.Json.Serialization;

namespace MotorCentroDistribuicao.Domain.Providers.Rest.Dtos
{
    public class CentroDistribuicaoProviderDto
    {
        [JsonPropertyName("distribuitionCenters")]
        public List<string>? CentrosDistribuicao { get; set; }
    }
}
