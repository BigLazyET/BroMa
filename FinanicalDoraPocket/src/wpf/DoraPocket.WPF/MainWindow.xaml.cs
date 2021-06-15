using DoraPocket.WPF.Setting;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Resources;

namespace DoraPocket.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// https://docs.microsoft.com/zh-cn/dotnet/desktop/wpf/windows/?view=netdesktop-5.0
    /// 主副屏1显示：https://www.cnblogs.com/xiefang2008/p/9594104.html
    /// 图片生成操作为Resource时如何获取：https://www.cnblogs.com/davidzhou11225/p/4761965.html
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Debug.WriteLine($"window actual width: {ActualWidth}, actual height: {ActualHeight}");
            Debug.WriteLine($"window width: {Width}, height: {Height}");
            Debug.WriteLine($"window SystemParameters WorkArea width: {SystemParameters.WorkArea.Width}, SystemParameters WorkArea height: {SystemParameters.WorkArea.Height}");

            //Top = SystemParameters.WorkArea.Height - Height;
            //Left = SystemParameters.WorkArea.Width - Width;
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            // 窗口所有权：https://docs.microsoft.com/zh-cn/dotnet/desktop/wpf/windows/?view=netdesktop-5.0#window-ownership
            // Create a window and make the current window its owner
            var settingWindow = new SettingWindow();
            settingWindow.Owner = this;
            settingWindow.Show();
        }

        /// <summary>
        /// 窗口激活（活动窗口）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Activated(object sender, System.EventArgs e)
        {

        }

        /// <summary>
        /// 窗口失去激活（非活动窗口）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Deactivated(object sender, System.EventArgs e)
        {

        }

        /// <summary>
        /// 窗口关闭前触发
        /// 用于判断数据是否保存或者阻止窗口关闭等情形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // If data is dirty, prompt user and ask for a response
            // MessageBox: https://docs.microsoft.com/zh-cn/dotnet/desktop/wpf/windows/how-to-open-message-box?view=netdesktop-5.0
            var result = MessageBox.Show("请注意保存当前工作，确定关闭吗？", "询问", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            // User doesn't want to close, cancel closure
            if (result == MessageBoxResult.No)
                e.Cancel = true;
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, System.EventArgs e)
        {

        }

        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {

        }

        private void Depreciation_Click(object sender, RoutedEventArgs e)
        {
            GetOrCreateNewTab("折旧", "Views/Depreciation/DepreciationView.xaml");
        }

        private void Induction_Click(object sender, RoutedEventArgs e)
        {
            GetOrCreateNewTab("归纳", "Views/Induction/InductionView.xaml");
        }

        private void GetOrCreateNewTab(string head, string uri)
        {
            var index = GetTabItemIndex(head);
            if (index == -1)
            {
                index = tabControl.Items.Count;
                var frame = new Frame();
                frame.Source = new Uri(uri, UriKind.Relative);
                var tabItem = new TabItem { Header = head, Content = frame };
                tabControl.Items.Add(tabItem);
            }
            tabControl.SelectedIndex = index;
        }

        private int GetTabItemIndex(string head)
        {
            var count = tabControl.Items.Count;
            for (int i = 0; i < count; i++)
            {
                var tabItem = tabControl.Items[i] as TabItem;
                if (head.Equals(tabItem.Header.ToString()))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
