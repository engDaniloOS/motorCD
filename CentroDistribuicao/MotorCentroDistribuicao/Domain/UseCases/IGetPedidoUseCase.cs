using MotorCentroDistribuicao.Domain.Dtos;

namespace MotorCentroDistribuicao.Domain.UseCases
{
    public interface IGetPedidoUseCase
    {
        PedidoOutDto GetPedidoProcessado(Guid pedidoId);
    }
}
