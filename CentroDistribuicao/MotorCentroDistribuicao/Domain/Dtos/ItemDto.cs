using System.Text.Json.Serialization;

namespace MotorCentroDistribuicao.Domain.Dtos
{
    public record ItemDto
    {
        [JsonPropertyName("item")]
        public long Id { get; set; }

        [JsonPropertyName("erro")]
        public string? ErrorMessage { get; set; } = null;

        [JsonPropertyName("distribuitionCenters")]
        public List<string> CentrosDistribuicao { get; set; }
    }
}
