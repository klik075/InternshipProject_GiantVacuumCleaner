
using Scripts.Framework.Managers.ObjectPool.Modular;
using UnityEngine.Pool;

namespace Scripts.Framework.Managers.ObjectPool.Interfaces
{
    public interface IPoolFactory
    {
        IObjectPool<PoolBehavior> CreatePool(PoolData poolData, bool collection, int defaultCapacity = 10, int maxCapacity = 100);
    }
}