using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IFB_Lib.ObjectPool.MonoBehaviourPool
{
    public class MonoBehaviorPoolManager : MonoBehaviour, IObjectsPoolManager
    {
        [SerializeField] private List<GameObject> _gamePrefabs;
        private Dictionary<string, GameObject> _gamePrefabsDict = new Dictionary<string, GameObject>();
        private Dictionary<string, List<IPoolableObject>> _currentObjectsDict = new Dictionary<string, List<IPoolableObject>>();
        private Dictionary<string, Transform> _rootObjectsDict = new Dictionary<string, Transform>();

        private Predicate<IPoolableObject> _destroyPredicate;

#if UNITY_EDITOR
        [Space] 
        [SerializeField] private string _pathToPrefabs;

        [ContextMenu("UpdatePrefabs from folder")]
        private void UpdatePrefabsFromFolder()
        {
            if (!string.IsNullOrEmpty(_pathToPrefabs))
            {
                _gamePrefabs = new List<GameObject>();
                var guidToPrefabs = UnityEditor.AssetDatabase.FindAssets("t:prefab", new string[] {_pathToPrefabs});
                foreach (var guid in guidToPrefabs)
                {
                    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>( path );
                    if(prefab)
                        _gamePrefabs.Add(prefab);
                }
            }  
        }
        
#endif
        
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
                CheckForDestroyedObjects(poolCollection);
                targetObject = TryGetFreeObject(poolCollection) as T;
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

        private IPoolableObject TryGetFreeObject(List<IPoolableObject> poolableObjectsCollection)
        {
            foreach (var poolObj in poolableObjectsCollection)
            {
                if (!poolObj.IsActive)
                    return poolObj;
            }

            return default;
        }
        
        private void CheckForDestroyedObjects(List<IPoolableObject> poolObjectsCollection)
        {
            bool isAnyDestroyed = false;
            foreach (var poolObj in poolObjectsCollection)
            {
                if (poolObj.IsDestroyed)
                {
                    isAnyDestroyed = true;
                    break;
                }
            }

            if (isAnyDestroyed)
            {
                if (_destroyPredicate == null)
                {
                    _destroyPredicate = o => o.IsDestroyed;
                }
                
                poolObjectsCollection.RemoveAll(_destroyPredicate);
            }

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