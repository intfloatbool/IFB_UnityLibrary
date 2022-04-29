using System;

namespace IFB_UnityLibrary.ScenesHandling
{
    public interface IScenesController
    {
        event Action<SceneDataContainer> OnSceneLoaded;
        event Action<SceneDataContainer> OnSceneStartLoading;
        void LoadScene(SceneDataContainer sceneDataContainer);
    }
}