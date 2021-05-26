using Microsoft.Extensions.DependencyInjection;

namespace DoraPocket.Common
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}
