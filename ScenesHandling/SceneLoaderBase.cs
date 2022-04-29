using UnityEngine;

namespace IFB_UnityLibrary.ScenesHandling
{
    public abstract class SceneLoaderBase : MonoBehaviour
    {
        [SerializeField] protected SceneDataSO _sceneData;

        public abstract void LoadScene();
    }
}