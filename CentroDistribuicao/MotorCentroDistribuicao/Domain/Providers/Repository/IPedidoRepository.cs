using MotorCentroDistribuicao.Domain.Models;

namespace MotorCentroDistribuicao.Domain.Providers.Repository
{
    public interface IPedidoRepository
    {
        void Salvar(Pedido pedido);
        Pedido Get(Guid pedidoID);
    }
}
