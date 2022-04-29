namespace IFB_UnityLibrary.ScenesHandling
{
    public abstract class SceneLoadingHandlerBase
    {
        private IScenesController _scenesController;
        public SceneLoadingHandlerBase(IScenesController scenesController)
        {
            _scenesController = scenesController;
            
            _scenesController.OnSceneLoaded += OnSceneLoaded;
            _scenesController.OnSceneStartLoading += OnSceneStartLoading;
        }

        ~SceneLoadingHandlerBase()
        {
            if (_scenesController != null)
            {
                _scenesController.OnSceneLoaded -= OnSceneLoaded;
                _scenesController.OnSceneStartLoading -= OnSceneStartLoading;
            }
        }
        
        
        protected abstract void OnSceneStartLoading(SceneDataContainer sceneData);
        protected abstract void OnSceneLoaded(SceneDataContainer sceneData);
    }
}