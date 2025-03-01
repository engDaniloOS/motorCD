using MotorCentroDistribuicao.Domain.Dtos;

namespace MotorCentroDistribuicao.Domain.Validators
{
    public static class PedidoValidator
    {
        public static string IsValidOrGetErrorMessage(this PedidoDto dto)
            => dto.Itens.Count > 100 ? "Um pedido não pode ter mais que 100 itens" : string.Empty;
    }
}
