using System;
using System.Windows.Input;

namespace WpfEssentials.Commands
{
    public class RelayCommand<Param> : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected readonly Action<Param> execute;
        protected readonly Predicate<Param> canExecute;

        public RelayCommand(Action<Param> execute) : this(execute, null) { }
        public RelayCommand(Action<Param> execute, Predicate<Param> canExecute)
            => (this.execute, this.canExecute) = (execute ?? throw new ArgumentException(nameof(execute)), canExecute);

        public virtual bool CanExecute(object parameter)
            => canExecute?.Invoke((Param)parameter) ?? true;
        public virtual void Execute(object parameter) => execute((Param)parameter);
    }

    public sealed class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute) : this(execute, null) { }
        public RelayCommand(Action execute, Func<bool> canExecute)
            => (this.execute, this.canExecute) = (execute ?? throw new ArgumentException(nameof(execute)), canExecute);

        public bool CanExecute(object parameter)
            => canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => execute();
    }


    public class RelayCommandManualUpdate<Param> : ICommandManualUpdate
    {
        public event EventHandler CanExecuteChanged;

        protected readonly Action<Param> execute;
        protected readonly Predicate<Param> canExecute;

        public RelayCommandManualUpdate(Action<Param> execute) : this(execute, null) { }
        public RelayCommandManualUpdate(Action<Param> execute, Predicate<Param> canExecute)
            => (this.execute, this.canExecute) = (execute ?? throw new ArgumentException(nameof(execute)), canExecute);

        public virtual bool CanExecute(object parameter)
            => canExecute?.Invoke((Param)parameter) ?? true;
        public virtual void Execute(object parameter) => execute((Param)parameter);

        public void RevalidateCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public sealed class RelayCommandManualUpdate : ICommandManualUpdate
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommandManualUpdate(Action execute) : this(execute, null) { }
        public RelayCommandManualUpdate(Action execute, Func<bool> canExecute)
            => (this.execute, this.canExecute) = (execute ?? throw new ArgumentException(nameof(execute)), canExecute);

        public bool CanExecute(object parameter)
            => canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => execute();

        public void RevalidateCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}