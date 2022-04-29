using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IFB_UnityLibrary.ScenesHandling
{
    public class ScenesController : MonoBehaviour,  IScenesController
    {
        public virtual event Action<SceneDataContainer> OnSceneLoaded;
        public virtual event Action<SceneDataContainer> OnSceneStartLoading;

        protected Coroutine _sceneLoadingCoroutine;

        protected SceneDataContainer? _lastSceneDataContainer;
        
        public virtual void LoadScene(SceneDataContainer sceneDataContainer)
        {
            if (_sceneLoadingCoroutine != null)
            {
                Debug.LogError($"Scene {_lastSceneDataContainer.Value.SceneName} is already in progress.");
                return;
            }

            _lastSceneDataContainer = sceneDataContainer;
            _sceneLoadingCoroutine = StartCoroutine(SceneLoadingCoroutine());
        }

        protected virtual IEnumerator SceneLoadingCoroutine()
        {
            var sceneContainer = _lastSceneDataContainer.Value;
            OnSceneStartLoading?.Invoke(sceneContainer);

            var asyncOp = SceneManager.LoadSceneAsync(sceneContainer.SceneName);
            while (!asyncOp.isDone)
            {
                yield return null;
            }
            
            _sceneLoadingCoroutine = null;
            OnSceneLoaded?.Invoke(sceneContainer);
            yield return null;
        }
    }
}