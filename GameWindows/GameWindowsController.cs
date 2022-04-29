using System;
using System.Collections.Generic;
using System.IO;
using IFB_Lib.ObjectPool.MonoBehaviourPool;
using UnityEngine;

namespace IFB_UnityLibrary.GameWindows
{
    public class GameWindowsController : MonoBehaviour, IGameWindowsController
    {
        [SerializeField] protected string _resourcesPath = "GameWindows";
        [SerializeField] protected Canvas _canvasForWindows;
        [SerializeField] protected MonoBehaviorPoolManager _objectsPoolManager;

        protected List<IGameWindow> _activeWindows = new List<IGameWindow>();
        protected List<IGameWindow> _windowsToRemoveBuffer = new List<IGameWindow>();
        
        public virtual T Show<T>() where T : class, IGameWindow
        {
            var windowTypeName = typeof(T).Name;
            if (!_objectsPoolManager.GamePrefabsDict.ContainsKey(windowTypeName))
            {
                var windowPrefab = Resources.Load<GameObject>(Path.Combine(_resourcesPath, windowTypeName));
                _objectsPoolManager.AddPrefab(windowPrefab);
            }
            var window = _objectsPoolManager.GetObjectFromPool<T>();
            var windowTransform = window.GetTransform();
            windowTransform.parent = _canvasForWindows.transform;
            window.Show();
            
            _activeWindows.Add(window);
            return window;
        }

        private void Update()
        {
            foreach (var activeWindow in _activeWindows)
            {
                if (activeWindow != null)
                {
                    activeWindow.UpdateWindow();
                    if (!activeWindow.IsActive)
                    {
                        _windowsToRemoveBuffer.Add(activeWindow);
                    }
                }
            }
            
            if (_windowsToRemoveBuffer.Count > 0)
            {
                foreach (var windowToRemove in _windowsToRemoveBuffer)
                {
                    _activeWindows.Remove(windowToRemove);
                }
                
                _windowsToRemoveBuffer.Clear();
            }
        }
    }
}