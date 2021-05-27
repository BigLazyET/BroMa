using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace DoraPocket.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, string assemblyName)
        {
            Guard.ArgumentNotNull(services, nameof(services));
            Guard.ArgumentNotNullOrWhiteSpace(assemblyName, nameof(assemblyName));

            var assembly = Assembly.Load(new AssemblyName(assemblyName));
            if (null != assembly)
            {
                var result = from type in assembly.GetExportedTypes()
                             let attributes = type.GetCustomAttributes<RegisterAsServiceAttribute>()
                             where attributes.Any()
                             select new { ImplementType = type, ServiceTypes = attributes.SelectMany(it => it.ServiceTypes) };

                foreach (var item in result)
                {
                    foreach (var serviceType in item.ServiceTypes)
                    {
                        services.AddSingleton(serviceType, item.ImplementType);
                    }
                }
            }

            var startupAttribute = assembly.GetCustomAttribute<StartupAttribute>();
            if (startupAttribute != null)
            {
                var startup = (IStartup)Activator.CreateInstance(startupAttribute.Startup);
                startup.ConfigureServices(services);
            }

            return services;
        }
    }
}
