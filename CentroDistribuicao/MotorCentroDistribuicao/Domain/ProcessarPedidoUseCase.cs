using AutoMapper;
using MotorCentroDistribuicao.Configurations;
using MotorCentroDistribuicao.Domain.Converters;
using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Models;
using MotorCentroDistribuicao.Domain.Providers.Repository;
using MotorCentroDistribuicao.Domain.Providers.Rest;
using MotorCentroDistribuicao.Domain.UseCases;
using MotorCentroDistribuicao.Domain.Validators;
using System.Collections.Concurrent;

namespace MotorCentroDistribuicao.Domain
{
    public class ProcessarPedidoUseCase(
        ICentroDistribuicaoProvider cdprovider,
        IPedidoRepository pedidoRepository,
        IConfiguration configuration,
        IMapper mapper,
        ILogger<ProcessarPedidoUseCase> logger) : IProcessarPedidoUseCase
    {

        public async Task<PedidoOutDto> GetCentrosDistribuicao(PedidoDto pedido)
        {
            logger.LogInformation("Iniciando processamento do pedido");

            var validationMessage = pedido.IsValidOrGetErrorMessage();

            if (!string.IsNullOrWhiteSpace(validationMessage))
                return PedidoOutDtoConverter.BuildOudDtoWithError(validationMessage);

            var itensParaProcessamento = pedido.Itens.Distinct().ToList();
            var itensProcessados = await ProcessarItensEmParalelo(itensParaProcessamento);
            var respostaPedido = PedidoOutDtoConverter.BuildOutDtoFrom(itensProcessados, configuration);

            SalvarPedido(respostaPedido);

            logger.LogInformation("Processamento finalizado");

            return respostaPedido;
        }

        private void SalvarPedido(PedidoOutDto pedidoOutDto)
        {
            try
            {
                if (pedidoOutDto.Id == Guid.Empty)
                    return;

                var modeloPedido = mapper.Map<Pedido>(pedidoOutDto);
                pedidoRepository.Salvar(modeloPedido);
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro ao salvar o pedido {pedidoOutDto.Id} na base de dados. Erro: {ex.Message}");
            }
        }

        private async Task<List<ItemDto>> ProcessarItensEmParalelo(List<long> itens)
        {
            var itensProcessados = new ConcurrentBag<ItemDto>();
            var semaforo = new SemaphoreSlim(HttpClientServiceConfig.MaxRequisicoesParalelas);

            await Parallel.ForEachAsync(itens, async (item, cancellationToken) =>
            {
                await semaforo.WaitAsync(cancellationToken);

                try
                {
                    var respostaProvider = await cdprovider.GetCentrosDistribuicaoPorItem(item);

                    if (respostaProvider.CentrosDistribuicao == null)
                    {
                        var error = $"Item {item} não encontrado.";
                        logger.LogError(error);

                        itensProcessados.Add(new ItemDto { Id = item, ErrorMessage = error });

                        return;
                    }

                    itensProcessados.Add(
                        new ItemDto
                        {
                            Id = item,
                            CentrosDistribuicao = respostaProvider.CentrosDistribuicao,
                            ErrorMessage = respostaProvider.CentrosDistribuicao.Any() ? null : "Item indisponível"
                        });
                }
                catch(Exception ex)
                {
                    logger.LogError($"Erro ao processar o item {item}. Erro: {ex.Message}");
                    itensProcessados.Add(new ItemDto { Id = item, ErrorMessage = $"Não foi possível processar o item" });
                }
                finally
                {
                    semaforo.Release();
                }
            });

            return [.. itensProcessados];
        }
    }
}
