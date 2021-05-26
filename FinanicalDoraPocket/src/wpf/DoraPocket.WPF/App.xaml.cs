using DoraPocket.Common;
using DoraPocket.Common.Observers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DoraPocket.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureServices();

            base.OnStartup(e);
        }

        private void ConfigureServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IEventSource, EventSource>()  // 手动显式注册服务
                .RegisterServices("DoraPocket.Common")      // 利用标签批量注册各个程序集中的服务
                .RegisterServices("DoraPocket.Domain")
                .RegisterServices("DoraPocket.Infrastructure")
                .RegisterServices("DoraPocket.ViewModel")
                .RegisterServices("DoraPocket.WPF")
                .BuildServiceProvider();
            ServiceProviderAccessor.Current = serviceProvider;  // 方便无法通过构造注入的方式获取服务的情形去获取服务
        }
    }
}
