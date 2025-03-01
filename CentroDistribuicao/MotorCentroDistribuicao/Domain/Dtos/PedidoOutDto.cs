using System.Text.Json.Serialization;

namespace MotorCentroDistribuicao.Domain.Dtos
{
    public record PedidoOutDto
    {
        [JsonPropertyName("pedidoId")]
        public Guid Id { get; set; }

        [JsonPropertyName("itens")]
        public List<ItemDto> Itens { get; set; }

        [JsonPropertyName("validade")]
        public DateTime Validade { get; set; }

        [JsonPropertyName("erro")]
        public string? ErrorMessage { get; set; }

        [JsonIgnore]
        public bool HasError { get; set; } = false;

        [JsonIgnore]
        public bool NotFound { get; internal set; }
    }
}