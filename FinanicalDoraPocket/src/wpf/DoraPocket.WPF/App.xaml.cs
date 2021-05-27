using DoraPocket.Common;
using DoraPocket.Common.Observers;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DoraPocket.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Override
        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureServices();

            base.OnStartup(e);
        }
        #endregion

        #region Method
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
        #endregion

        #region Event
        /// <summary>
        /// 可以使用 Application.Activated 事件处理应用程序范围的激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Activated(object sender, System.EventArgs e)
        {

        }

        /// <summary>
        /// 可以使用 Application.Deactivated 事件处理应用程序范围的非激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Deactivated(object sender, System.EventArgs e)
        {

        }
        #endregion
    }
}
