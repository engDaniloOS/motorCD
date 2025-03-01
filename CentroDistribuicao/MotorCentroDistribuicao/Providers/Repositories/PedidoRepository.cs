using LiteDB;
using MotorCentroDistribuicao.Domain.Models;
using MotorCentroDistribuicao.Domain.Providers.Repository;

namespace MotorCentroDistribuicao.Providers.Repositories
{
    public class PedidoRepository(ILiteDatabase database) : IPedidoRepository
    {
        public Pedido Get(Guid pedidoID) => GetCollection().FindById(pedidoID);

        public void Salvar(Pedido pedido) => GetCollection().Insert(pedido);

        private ILiteCollection<Pedido> GetCollection() => database.GetCollection<Pedido>("pedidos");
    }
}
