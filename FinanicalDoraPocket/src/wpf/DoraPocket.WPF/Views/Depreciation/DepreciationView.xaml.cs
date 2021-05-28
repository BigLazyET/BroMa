using DoraPocket.ViewModel.Depreciation;
using Microsoft.Win32;
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
        private readonly DepreciationViewModel depreciationViewModel;

        public DepreciationView()

        {
            InitializeComponent();

            depreciationViewModel = DataContext as DepreciationViewModel;
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
                depreciationViewModel.FilePath = dialog.FileName;
            }
        }
    }
}
