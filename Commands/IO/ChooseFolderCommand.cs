using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfEssentials.Exceptions;

namespace WpfEssentials.Commands.IO
{
    public delegate void PathChosen(string path);

    public record ChooseFolderCommandData : CommandData
    {
        public const string NO_PATH = ""; //string.Empty would be nicer but no constexpr...
    }

    public class ChooseFolderCommandHandler : IAsyncCommandHandler
    {
        public async Task<string> HandleAsync(CommandData input)
        {
            var path = string.Empty;

            Thread t = new Thread(() =>
            {
                using (FolderBrowserDialog ofd = new FolderBrowserDialog())
                {
                    var res = ofd.ShowDialog();

                    if (res.HasFlag(DialogResult.OK) || res.HasFlag(DialogResult.Yes))
                        path = ofd.SelectedPath;
                    else
                        path = ChooseFolderCommandData.NO_PATH;
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            await Task.Yield();
            t.Join();

            return path;
        }

    }

    public class ChooseFolderCommand : AsyncCommand
    {
        private readonly ChooseFolderCommandData command;
        private readonly ChooseFolderCommandHandler handler;

        private readonly Action<string> onPathAquired;

        public ChooseFolderCommand(ChooseFolderCommandData command, Action<string> onPathAquired)
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
