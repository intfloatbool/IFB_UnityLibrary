using IFB_Lib.ObjectPool;
using UnityEngine;

namespace IFB_UnityLibrary.GameWindows
{
    public interface IGameWindow : IPoolableObject
    {
        string WindowName { get; }
        Transform GetTransform();
        void UpdateWindow();
    }
}