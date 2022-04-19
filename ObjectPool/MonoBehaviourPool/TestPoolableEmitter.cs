using UnityEngine;

namespace IFB_Lib.ObjectPool.MonoBehaviourPool
{
    public class TestPoolableEmitter : MonoBehaviour
    {
        [SerializeField] private MonoBehaviorPoolManager _poolManager;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var poolable = _poolManager.GetObjectFromPool<TestPoolable>();
                poolable.transform.position = transform.position;
                poolable.transform.rotation = transform.rotation;
            }
        }
    }
}