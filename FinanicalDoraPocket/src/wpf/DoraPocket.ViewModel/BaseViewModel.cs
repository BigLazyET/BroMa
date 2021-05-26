using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DoraPocket.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        // https://www.cnblogs.com/liuzhendong/p/3286278.html
        // 在创建Binding对象并将它作为数据源绑定到控件时，控件自动订阅了这个PropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T item, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(item,value))
            {
                item = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}
