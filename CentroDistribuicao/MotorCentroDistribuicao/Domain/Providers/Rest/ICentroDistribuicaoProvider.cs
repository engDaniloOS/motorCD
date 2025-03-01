using MotorCentroDistribuicao.Domain.Providers.Rest.Dtos;

namespace MotorCentroDistribuicao.Domain.Providers.Rest
{
    public interface ICentroDistribuicaoProvider
    {
        Task<CentroDistribuicaoProviderDto> GetCentrosDistribuicaoPorItem(long item);
    }
}
