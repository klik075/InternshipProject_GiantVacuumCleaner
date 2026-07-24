using System;
using Scripts.Framework.Managers.ObjectPool.Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace Scripts.Framework.Managers.ObjectPool.Modular
{
    public abstract class PoolBehavior : MonoBehaviour, IPoolAble
    {
        #region Events

        public static event Action<IPoolAble> OnPoolGet = delegate { };
        public static event Action<IPoolAble> OnPoolReleased = delegate { };
        public static event Action<IPoolAble> OnPoolDestroyed = delegate { };

        #endregion
    
    
    
        #region Properties

        /* Implements */
        public GameObject GameObject => gameObject;
        public IObjectPool<PoolBehavior> Pool { get; set; }

        #endregion



        #region Interface Implements Methods

        public void OnGet()
        {
            GameObject.SetActive(true);
            OnPoolGet.Invoke(this);
        }

        public void OnRelease()
        {
            GameObject.SetActive(false);
            OnPoolReleased.Invoke(this);
        }

        public void OnDestroy()
        {
            OnPoolDestroyed.Invoke(this);
        }

        #endregion
    }
}
