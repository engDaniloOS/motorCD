using AutoMapper;
using MotorCentroDistribuicao.Domain.Converters;
using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Providers.Repository;
using MotorCentroDistribuicao.Domain.UseCases;

namespace MotorCentroDistribuicao.Domain
{
    public class GetPedidoUseCase(
        IPedidoRepository pedidoRepository,
        IMapper mapper,
        ILogger<GetPedidoUseCase> logger) : IGetPedidoUseCase
    {
        public PedidoOutDto GetPedidoProcessado(Guid pedidoId)
        {
            try
            {
                var pedido = pedidoRepository.Get(pedidoId);

                if (pedido == null)
                    return PedidoOutDtoConverter.BuildOudDtoNotFounded();

                var resultado = mapper.Map<PedidoOutDto>(pedido);

                logger.LogInformation($"Processamento de pedido {pedidoId} finalizado", resultado);

                return resultado;
            }
            catch (Exception ex)
            {
                var erroMessage = $"Erro ao buscar o pedido. Pedido {pedidoId}. Erro: {ex.Message}";
                logger.LogError(erroMessage);

                return new PedidoOutDto
                {
                    HasError = true,
                    ErrorMessage = erroMessage
                };
            }
        }
    }
}
