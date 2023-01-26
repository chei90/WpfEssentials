using System.Windows.Input;

namespace WpfEssentials.Commands
{
    public interface ICommandManualUpdate : ICommand
    {
        public void RevalidateCanExecute();
    }
}
