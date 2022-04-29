namespace IFB_UnityLibrary.Commands
{
    public interface ICommandInvoker
    {
        void Execute(IGameCommand gameCommand);
    }
}