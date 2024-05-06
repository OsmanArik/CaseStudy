using Microsoft.Extensions.Configuration;
using Shared.Core.Utilies.IoC;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDependencyResolversEx(this IServiceCollection services, IConfiguration configuration, ICoreModule[] modules)
        {
            foreach (var module in modules)
                module.Load(services, configuration);
        }
    }
}