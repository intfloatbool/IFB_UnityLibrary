using System;
using UnityEngine;

namespace IFB_Lib.ObjectPool.MonoBehaviourPool
{
    public abstract class PoolableMonoBehaviour : MonoBehaviour, IPoolableObject
    {
        public bool IsDestroyed { get; protected set; }

        public bool IsActive
        {
            get
            {
                if (IsDestroyed)
                    return false;

                return gameObject.activeInHierarchy;
            }
        }

        public virtual void Show()
        {
            if(IsDestroyed)
                return;
            
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            if(IsDestroyed)
               return;
            
            gameObject.SetActive(false);
        }

        public virtual void DestroyCompletely()
        {
            if(gameObject)
                Destroy(gameObject);
        }
        
        protected virtual void OnDestroy()
        {
            IsDestroyed = true;
        }
    }
}