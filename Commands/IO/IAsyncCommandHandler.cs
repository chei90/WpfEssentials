using System.Threading.Tasks;

namespace WpfEssentials.Commands.IO
{
    public interface IAsyncCommandHandler
    {
        Task<string> HandleAsync(CommandData command);
    }
}
