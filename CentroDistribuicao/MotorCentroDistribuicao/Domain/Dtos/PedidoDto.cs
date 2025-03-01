using System.Text.Json.Serialization;

namespace MotorCentroDistribuicao.Domain.Dtos
{
    public record PedidoDto
    {
        [JsonPropertyName("itens")]
        public List<long> Itens { get; set; }
    }
}
