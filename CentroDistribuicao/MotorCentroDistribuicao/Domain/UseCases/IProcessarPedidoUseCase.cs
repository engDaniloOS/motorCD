using MotorCentroDistribuicao.Domain.Dtos;

namespace MotorCentroDistribuicao.Domain.UseCases
{
    public interface IProcessarPedidoUseCase
    {
        Task<PedidoOutDto> GetCentrosDistribuicao(PedidoDto pedido);
    }
}
