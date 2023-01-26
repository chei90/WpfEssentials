using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfEssentials.Threading;

namespace WpfEssentials.Commands
{
    public abstract class AsyncCommand : ICommand
    {
        public virtual event EventHandler CanExecuteChanged;

        private readonly Action<Exception> onException;
        private readonly AtomicBool isExecuting = new(false);

        public bool IsExecuting
        {
            get => isExecuting.Value;
            set
            {
                if (value != isExecuting.Value)
                {
                    isExecuting.Change(value);
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public AsyncCommand(Action<Exception> onException)
            => this.onException = onException;

        public bool CanExecute(object parameter) => !IsExecuting && CanExecute();
        protected virtual bool CanExecute() => true;

        public async void Execute(object parameter)
        {
            IsExecuting = true;

            try
            {
                await ExecuteAsync();
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
            }

            IsExecuting = false;
        }

        protected abstract Task ExecuteAsync();
    }

    public abstract class AsyncCommand<T> : ICommand
    {
        public virtual event EventHandler CanExecuteChanged;

        private readonly Action<Exception> onException;
        private readonly AtomicBool isExecuting = new(false);

        public bool IsExecuting
        {
            get => isExecuting.Value;
            set
            {
                if (value != isExecuting.Value)
                {
                    isExecuting.Change(value);
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public AsyncCommand(Action<Exception> onException)
            => this.onException = onException;

        public bool CanExecute(object parameter) => !IsExecuting && CanExecute((T)parameter);
        protected virtual bool CanExecute(T parameter) => true;

        protected virtual void PostAction() { }
        protected virtual void PreAction() { }

        public async void Execute(object parameter)
        {

            PreAction();
            IsExecuting = true;

            try
            {
                await ExecuteAsync((T)parameter);
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
            }

            IsExecuting = false;
            PostAction();
        }

        protected abstract Task ExecuteAsync(T parameter);
    }
}