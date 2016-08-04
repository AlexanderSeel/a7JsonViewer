using System;
using System.Windows.Input;

namespace a7JsonViewer.Utils
{
    public class LambdaCommand : ICommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;

        public LambdaCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute(parameter);
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter) =>
            _execute?.Invoke(parameter);
    }
}
