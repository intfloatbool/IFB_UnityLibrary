using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IFB_Lib.ObjectPool.MonoBehaviourPool
{
    public class MonoBehaviorPoolManager : MonoBehaviour, IObjectsPoolManager
    {
        [SerializeField] private GameObject[] _gamePrefabs;
        private Dictionary<string, GameObject> _gamePrefabsDict = new Dictionary<string, GameObject>();
        private Dictionary<string, List<IPoolableObject>> _currentObjectsDict = new Dictionary<string, List<IPoolableObject>>();
        private Dictionary<string, Transform> _rootObjectsDict = new Dictionary<string, Transform>();

        private void Awake()
        {
            _gamePrefabsDict = _gamePrefabs.ToDictionary(gp => gp.name);
        }

        public T GetObjectFromPool<T>() where T : class, IPoolableObject
        {
            var type = typeof(T);
            var typeName = type.Name;
            GameObject targetPrefab = default;
            _gamePrefabsDict.TryGetValue(typeName, out targetPrefab);

            if (targetPrefab == null)
            {
                Debug.LogError($"There is no prefab with name {typeName}!");
                return default;
            }

            T targetObject = default;

            if(_currentObjectsDict.TryGetValue(typeName, out var poolCollection))
            {
                targetObject = poolCollection.FirstOrDefault(po => !po.IsActive) as T;
            }
            else
            {
                _currentObjectsDict[typeName] = new List<IPoolableObject>();
            }
            
            if (targetObject == null)
            {
                Transform root = default;
                if (!_rootObjectsDict.TryGetValue(typeName, out root))
                {
                    var newRoot = new GameObject($"RootFor-[ {typeName} ]");
                    newRoot.transform.parent = transform;
                    newRoot.transform.localPosition = Vector3.zero;
                    newRoot.transform.localRotation = Quaternion.identity;
                    
                    _rootObjectsDict[typeName] = newRoot.transform;
                }
                var instance = Instantiate(targetPrefab, _rootObjectsDict[typeName]);
                targetObject = instance.GetComponent<T>();
                _currentObjectsDict[typeName].Add(targetObject);
            }

            targetObject.Show();
            
            return targetObject;

        }

        private void OnDestroy()
        {
            Clear();
        }

        public void Clear()
        {
            foreach (var kvp in _currentObjectsDict)
            {
                foreach (var poolable in kvp.Value)
                {
                    poolable.DestroyCompletely();
                }
            }

            foreach (var kvp in _rootObjectsDict)
            {
                if (kvp.Value)
                {
                    Destroy(kvp.Value);
                }
            }
            
            _currentObjectsDict.Clear();
            _rootObjectsDict.Clear();
        }
        
    }
}