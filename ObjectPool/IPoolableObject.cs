using System;

namespace IFB_Lib.ObjectPool
{
    public interface IPoolableObject
    {
        bool IsDestroyed { get; }
        bool IsActive { get; }
        void Show();
        void Hide();
        void DestroyCompletely();
    }
}
