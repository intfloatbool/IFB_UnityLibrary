namespace IFB_Lib.ObjectPool
{
    public interface IObjectsPoolManager
    {
        public T GetObjectFromPool<T>() where T : class, IPoolableObject;

        public void Clear();
    }
}