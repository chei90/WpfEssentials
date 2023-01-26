using System;
using System.Threading.Tasks;

namespace WpfEssentials.Commands
{
    public class AsyncRelayCommand : AsyncCommand
    {
        private readonly Func<Task> callback;

        public AsyncRelayCommand(Func<Task> callback, Action<Exception> onException) : base(onException)
            => this.callback = callback;

        protected override async Task ExecuteAsync()
            => await callback();
    }

    public class AsyncRelayCommand<T> : AsyncCommand<T>
    {
        private readonly Func<T, Task> callback;

        public AsyncRelayCommand(Func<T, Task> callback, Action<Exception> onException) : base(onException)
            => this.callback = callback;

        protected override async Task ExecuteAsync(T param)
            => await callback(param);
    }
}
