using DoraPocket.Common;
using DoraPocket.Common.Observers;
using DoraPocket.ViewModel.Depreciation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DoraPocket.WPF.Views.Depreciation
{
    /// <summary>
    /// DepreciationView.xaml 的交互逻辑
    /// 折旧
    /// </summary>
    public partial class DepreciationView : Page
    {
        private readonly IEventSource eventSource;
        private readonly DepreciationViewModel depreciationViewModel;

        public DepreciationView()

        {
            InitializeComponent();

            depreciationViewModel = DataContext as DepreciationViewModel;

            eventSource = ServiceProviderAccessor.Current.GetRequiredService<IEventSource>();
            
            if(eventSource.CanSubscribe(EventSourceKeys.Depreciation_Valid))
                eventSource.Subscribe(EventSourceKeys.Depreciation_Valid, async args => await Dispatcher.InvokeAsync(()=> MessageBox.Show(args.ToString())));
        }

        private void PickFileBtn_Click(object sender, RoutedEventArgs e)
        {
            // configure open file dialog box
            var dialog = new OpenFileDialog();
            //dialog.FileName = "折旧"; // 默认文件名
            //dialog.DefaultExt = ".xls"; // 默认文件扩展名
            dialog.Filter = "All files(.)|*.*"; // 过滤扩展名

            // show open file dialog box
            bool? result = dialog.ShowDialog();

            // process open file dialog box result
            if (result == true)
            {
                var filePath = dialog.FileName;
                var extension = Path.GetExtension(filePath);
                if (extension != ".xlsx" || extension != ".xls")
                {
                    MessageBox.Show("请选择扩展名为xlsx或者xls的excel类型的文件！");
                    return;
                }
                depreciationViewModel.FilePath = dialog.FileName;
            }
        }
    }
}
