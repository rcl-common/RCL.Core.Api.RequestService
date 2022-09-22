using RCL.Core.Api.RequestService;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiRequestServiceExtension
    {
        public static IServiceCollection AddRCLCoreApiRequestServices(this IServiceCollection services, Action<ApiOptions> setupAction)
        {
            services.Configure(setupAction);

            return services;
        }
    }
}
