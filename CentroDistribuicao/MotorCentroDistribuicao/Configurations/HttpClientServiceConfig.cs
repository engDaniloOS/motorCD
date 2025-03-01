namespace MotorCentroDistribuicao.Configurations
{
    public static class HttpClientServiceConfig
    {
        public static int MaxRequisicoesParalelas { get; private set; }

        public const string HTTP_CLIENT_CD = "centro_distribuicao";

        private const string RETRY_POLICY = "RetryPolicy";
        private const string CIRCUIT_BREAK_POLICY = "CircuitBreakerPolicy";
        private const int QTDD_REQ_PARALELAS_DEFAULT = 4;

        public static void ConfigureHttpClient(this IServiceCollection services, IConfiguration configurations)
        {
            ConfigureRequisicoesParalelas();

            var centroDistribuicaoUrl = GetCentroDistribuicaoUrl(configurations);

            var policyRegistry = services.AddHttpClient().AddPolicyRegistry();

            policyRegistry.Add(RETRY_POLICY, ResilienceServiceConfig.BuildRetryPolicy());
            policyRegistry.Add(CIRCUIT_BREAK_POLICY, ResilienceServiceConfig.BuildCircuitBreakPolicy());

            services
                .AddHttpClient(HTTP_CLIENT_CD, client =>
                {
                    client.BaseAddress = new Uri(centroDistribuicaoUrl!);
                    client.Timeout = TimeSpan.FromSeconds(10);
                })
                .AddPolicyHandlerFromRegistry(RETRY_POLICY)
                .AddPolicyHandlerFromRegistry(CIRCUIT_BREAK_POLICY); ;
        }

        private static string? GetCentroDistribuicaoUrl(IConfiguration configurations)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            return environment.ToLower().Equals("docker") ?
                configurations.GetRequiredSection("Http")["CentroDistribuicaoUrlDocker"] :
                configurations.GetRequiredSection("Http")["CentroDistribuicaoUrl"];
        }

        private static void ConfigureRequisicoesParalelas()
        {
            var maxReqParalelas = Environment.GetEnvironmentVariable("MAX_REQ_PARALELAS");

            MaxRequisicoesParalelas = string.IsNullOrEmpty(maxReqParalelas) ?
                QTDD_REQ_PARALELAS_DEFAULT :
                int.Parse(maxReqParalelas);
        }
    }
}
