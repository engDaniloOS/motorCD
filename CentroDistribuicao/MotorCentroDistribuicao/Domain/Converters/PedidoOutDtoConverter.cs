using MotorCentroDistribuicao.Domain.Dtos;

namespace MotorCentroDistribuicao.Domain.Converters
{
    public static class PedidoOutDtoConverter
    {
        private const string MSG_NAO_ENCONTRADO = "não encontrado.";

        public static PedidoOutDto BuildOutDtoFrom(List<ItemDto> itens, IConfiguration configuration)
        {
            var isProcessamentoItensOk =
                itens.All(item => string.IsNullOrWhiteSpace(item.ErrorMessage));

            var itensNaoEncontrados =
                itens.All(item => item.ErrorMessage?.Contains(MSG_NAO_ENCONTRADO) == true);

            var validadePedidoMin =
                int.Parse(configuration.GetRequiredSection("Pedidos")["ValidadeMin"]!);

            return new PedidoOutDto
            {
                Id = (isProcessamentoItensOk && !itensNaoEncontrados) ? Guid.NewGuid() : Guid.Empty,
                Itens = itens,
                Validade = DateTime.Now.AddMinutes(validadePedidoMin),
                HasError = !isProcessamentoItensOk,
                ErrorMessage = isProcessamentoItensOk ? null : "Erro ao processar itens",
                NotFound = itensNaoEncontrados
            };
        }

        public static PedidoOutDto BuildOudDtoWithError(string errorMessage)
        {
            return new PedidoOutDto
            {
                Id = Guid.Empty,
                HasError = true,
                ErrorMessage = errorMessage,
            };
        }

        public static PedidoOutDto BuildOudDtoNotFounded()
        {
            return new PedidoOutDto
            {
                Id = Guid.Empty,
                NotFound = true,
            };
        }
    }
}
