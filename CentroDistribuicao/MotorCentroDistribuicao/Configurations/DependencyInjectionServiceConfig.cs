using MotorCentroDistribuicao.Domain;
using MotorCentroDistribuicao.Domain.Providers.Repository;
using MotorCentroDistribuicao.Domain.Providers.Rest;
using MotorCentroDistribuicao.Domain.UseCases;
using MotorCentroDistribuicao.Providers.Repositories;
using MotorCentroDistribuicao.Providers.Rest;

namespace MotorCentroDistribuicao.Configurations
{
    public static class DependencyInjectionServiceConfig
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IProcessarPedidoUseCase, ProcessarPedidoUseCase>();
            services.AddScoped<IGetPedidoUseCase, GetPedidoUseCase>();

            services.AddScoped<ICentroDistribuicaoProvider, CentroDistribuicaoProvider>();

            services.AddSingleton<IPedidoRepository, PedidoRepository>();
        }
    }
}
