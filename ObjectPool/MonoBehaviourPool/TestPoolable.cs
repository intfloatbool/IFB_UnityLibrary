using System.Collections;
using UnityEngine;

namespace IFB_Lib.ObjectPool.MonoBehaviourPool
{
    public class TestPoolable : PoolableMonoBehaviour
    {
        [SerializeField] private float _speed = 8f;
        public override void Show()
        {
            base.Show();

            IEnumerator hideTimeout()
            {
                yield return new WaitForSeconds(3);
                Hide();
            }

            StartCoroutine(hideTimeout());
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime );
        }
    }
}