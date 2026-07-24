using Scripts.Framework.Managers.ObjectPool.Modular;
using UnityEngine;
using UnityEngine.Pool;

namespace Scripts.Framework.Managers.ObjectPool.Interfaces
{
    public interface IPoolAble
    {
        GameObject GameObject { get; }
        IObjectPool<PoolBehavior> Pool { get; set; }

        void OnGet();
        void OnRelease();
        void OnDestroy();
    }
}