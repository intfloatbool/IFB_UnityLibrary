using IFB_Lib.ObjectPool.MonoBehaviourPool;
using UnityEngine;

namespace IFB_UnityLibrary.GameWindows
{
    public abstract class GameWindowBase : PoolableMonoBehaviour, IGameWindow
    {
        public abstract string WindowName { get; }

        public Transform GetTransform()
        {
            return transform;
        }

        public virtual void UpdateWindow() {  }
    }
}