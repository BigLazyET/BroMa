using Microsoft.Extensions.DependencyInjection;

namespace DoraPocket.Common
{
    public class Startup : IStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // 注册一些不通过RegisterAsServiceAttribute标签标记的服务类
        }
    }
}
