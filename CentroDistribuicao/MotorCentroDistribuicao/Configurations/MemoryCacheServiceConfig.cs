namespace MotorCentroDistribuicao.Configurations
{
    public static class MemoryCacheServiceConfig
    {
        public static void ConfigureMemoryCache(this IServiceCollection services) => services.AddMemoryCache();
    }
}
