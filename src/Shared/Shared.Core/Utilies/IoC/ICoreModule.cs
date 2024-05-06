using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Core.Utilies.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection serviceCollection, IConfiguration configuration);
    }
}