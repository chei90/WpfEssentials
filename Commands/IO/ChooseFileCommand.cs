using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfEssentials.Exceptions;

namespace WpfEssentials.Commands.IO
{
    public record ChooseFileCommandData : ChooseFolderCommandData
    {
        public string Filter { get; init; }
        public string DefaultExt { get; init; }
    }

    public class ChooseFileCommandHandler : IAsyncCommandHandler
    {
        public async Task<string> HandleAsync(CommandData cmd)
        {
            var path = string.Empty;

            var command = cmd as ChooseFileCommandData;
            if (command == null)
            {
                throw new InvalidCastException($"{nameof(ChooseFileCommandHandler)} is expecting {nameof(ChooseFolderCommandData)}, " +
                    $"you've provided {cmd.GetType().Name}!");
            }

            Thread t = new Thread(() =>
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = command.Filter;
                    ofd.DefaultExt = command.DefaultExt;

                    var res = ofd.ShowDialog();

                    if (res.HasFlag(DialogResult.OK) || res.HasFlag(DialogResult.Yes))
                        path = ofd.FileName;
                    else
                        path = ChooseFileCommandData.NO_PATH;
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            await Task.Yield();
            t.Join();

            return path;
        }
    }

    public class ChooseFileCommand : AsyncCommand
    {
        private readonly ChooseFileCommandData command;
        private readonly ChooseFileCommandHandler handler;

        private readonly Action<string> onPathAquired;

        public ChooseFileCommand(ChooseFileCommandData command, Action<string> onPathAquired)
            : base(LaunchException.Launch)
            => (this.command, handler, this.onPathAquired)
                = (command, new(), onPathAquired);

        protected override async Task ExecuteAsync()
        {
            var path = await handler.HandleAsync(command);

            if (string.IsNullOrEmpty(path))
                return;

            onPathAquired(path);
        }
    }
}
