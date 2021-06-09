
using DoraPocket.Common;
using System;
using System.Windows;
using System.Windows.Input;

namespace DoraPocket.ViewModel
{
    /// <summary>
    /// 自定义Command
    /// https://www.codeproject.com/tips/813345/basic-mvvm-and-icommand-usage-example
    /// </summary>
    public class BaseCommand : ICommand
    {
        private Action<object> execute;

        private Predicate<object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public BaseCommand(Action<object> execute):this(execute, DefaultCanExecute) { }

        public BaseCommand(Action<object> execute, Predicate<object> canExecute)
        {
            Guard.ArgumentNotNull(execute, nameof(execute));
            Guard.ArgumentNotNull(canExecute, nameof(canExecute));

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            var handler = this.CanExecuteChangedInternal;
            if(handler != null)
            {
                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    handler.Invoke(this, EventArgs.Empty);
                //});
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }
    }
}
