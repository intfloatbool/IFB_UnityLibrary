namespace IFB_UnityLibrary.Commands
{
    public interface IGameCommand
    {
        bool IsDone { get; }
        void StartCommand();
        void UpdateCommand();
        void EndCommand();
    }
}