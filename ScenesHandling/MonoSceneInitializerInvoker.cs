using UnityEngine;

namespace IFB_UnityLibrary.ScenesHandling
{
    public abstract class MonoSceneInitializerInvoker : MonoBehaviour
    {
        protected virtual void Start()
        {
            GetInitializer().InitializeScene();
        }

        protected abstract ISceneInitializer GetInitializer();
    }
}