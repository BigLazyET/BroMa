using DoraPocket.Common;
using DoraPocket.Common.Observers;
using DoraPocket.ViewModel.Induction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace DoraPocket.WPF.Views.Induction
{
    /// <summary>
    /// InductionView.xaml 的交互逻辑
    /// 归纳
    /// </summary>
    public partial class InductionView : Page
    {
        private readonly IEventSource eventSource;
        private readonly InductionViewModel viewModel;

        public InductionView()
        {
            InitializeComponent();

            viewModel = DataContext as InductionViewModel;
            eventSource = ServiceProviderAccessor.Current.GetRequiredService<IEventSource>();

            // Dispatcher.Invoke 在UI Thread上运行
            // https://stackoverflow.com/questions/11625208/accessing-ui-main-thread-safely-in-wpf
            if (eventSource.CanSubscribe(EventSourceKeys.Depreciation_Valid))
                eventSource.Subscribe(EventSourceKeys.Depreciation_Valid, async args => await Dispatcher.InvokeAsync(() => MessageBox.Show(args.ToString())));
        }

        private void PickInductionFileBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.InductionFilePath = ChooseFile();
        }

        private void PickRuleFileBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RuleFilePath = ChooseFile();
        }

        private string ChooseFile()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Excels (*.xls;*.xlsx)|*.xls;*.xlsx";

            bool? result = dialog.ShowDialog();

            return result == true? dialog.FileName : string.Empty;
        }
    }
}
