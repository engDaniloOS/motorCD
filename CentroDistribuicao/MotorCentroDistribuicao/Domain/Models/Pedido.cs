namespace MotorCentroDistribuicao.Domain.Models
{
    public class Pedido
    {
        public Guid? Id { get; set; }

        public List<Item>? Itens { get; set; }

        public DateTime? Validade { get; set; }
    }
}
