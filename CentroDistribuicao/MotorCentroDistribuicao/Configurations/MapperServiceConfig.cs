using AutoMapper;
using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Models;

namespace MotorCentroDistribuicao.Configurations
{
    public static class MapperServiceConfig
    {

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<PedidoOutDto, Pedido>();
                config.CreateMap<Pedido, PedidoOutDto>();
                config.CreateMap<Item, ItemDto>();
                config.CreateMap<ItemDto, Item>();
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
