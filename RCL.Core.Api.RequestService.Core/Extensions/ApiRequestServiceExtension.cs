using Microsoft.Extensions.DependencyInjection.Extensions;
using RCL.Core.Api.RequestService;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiRequestServiceExtension
    {
        public static IServiceCollection AddApiRequestServices(this IServiceCollection services, Action<ApiOptions> setupAction)
        {
            services.Configure(setupAction);

            return services;
        }
    }
}
