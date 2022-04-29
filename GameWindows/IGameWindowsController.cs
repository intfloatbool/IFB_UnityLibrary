namespace IFB_UnityLibrary.GameWindows
{
    public interface IGameWindowsController
    {
        T Show<T>() where T : class, IGameWindow;
    }
}