using Scripts.Framework.Managers.ObjectPool.Interfaces;
using Scripts.Framework.Managers.ObjectPool.Modular;
using UnityEngine;
using UnityEngine.Pool;

namespace Scripts.Framework.Managers.ObjectPool.Factory
{
    public class PoolFactory : IPoolFactory
    {
        public IObjectPool<PoolBehavior> CreatePool(PoolData poolData, bool collection, int defaultCapacity = 10, int maxCapacity = 100)
        {
            return new ObjectPool<PoolBehavior>(
                () =>
                {
                    GameObject go = Object.Instantiate(poolData.Prefab);
                    PoolBehavior pool = go.GetComponent<PoolBehavior>();
                    if (pool == null)
                    {
                        Debugger.LogWarning("오브젝트 풀 컴포넌트가 비어 있습니다. 'PoolBehavior'");
                        // pool = go.AddComponent<PoolBehavior>();
                    }

                    return pool;
                },
                pool => pool.OnGet(),
                pool => pool.OnRelease(),
                pool => pool.OnDestroy(),
                collection,
                defaultCapacity,
                maxCapacity);
        }
    }
}
