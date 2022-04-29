using UnityEngine;

namespace IFB_UnityLibrary.ScenesHandling
{
    [CreateAssetMenu(fileName = "SceneDataSO", menuName = "Scene control/SceneDataSO", order = 0)]
    public class SceneDataSO : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private UnityEditor.SceneAsset _sceneAsset;
#endif
        [SerializeField] private string _sceneName;
        public string SceneName => _sceneName;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _sceneName = _sceneAsset ? _sceneAsset.name : "!? MISSING ?!";
        }  
#endif

        public SceneDataContainer GetSceneDataContainer()
        {
            return new SceneDataContainer(_sceneName);
        }
    }
}