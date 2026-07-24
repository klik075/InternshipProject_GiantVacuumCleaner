using System.Collections.Generic;
using Scripts.Framework.Managers.ObjectPool.Factory;
using Scripts.Framework.Managers.ObjectPool.Interfaces;
using Scripts.Framework.Managers.ObjectPool.Modular;
using Scripts.Framework.Modules.SingletonModule.IndividualSingleton;
using UnityEngine;

namespace Scripts.Framework.Managers.ObjectPool.Core
{
    public class PoolManager : SingletonInternal<PoolManager>
    {
        #region Fields

        private readonly Dictionary<string, PoolData> _poolDictionary = new();
        private readonly Dictionary<PoolData, Transform> _poolHolderDictionary = new();
        private readonly IPoolFactory _poolFactory = new PoolFactory();

        #endregion



        #region Wrapping

        public static void Register(PoolData poolData, Transform holder = null) => Instance.RegisterPool(poolData, holder);
        public static void Unregister(string key) => Instance.UnregisterPool(key);
        public static GameObject OnGet(string key, Transform parent = null) => Instance.Get(key, parent);
        public static T OnGet<T>(string key, Transform parent = null) where T : PoolBehavior => Instance.Get<T>(key, parent);
        public static void OnRelease(GameObject poolObject) => Instance.Release(poolObject);
        public static void OnRelease(PoolBehavior poolBehavior) => Instance.Release(poolBehavior);

        #endregion
    
    

        #region Regist UnRegist

        private void RegisterPool(PoolData poolData, Transform holder = null)
        {
            if (poolData.Prefab == null || string.IsNullOrEmpty(poolData.Key)) return;
            poolData.Pool = _poolFactory.CreatePool(poolData, false);
            if (_poolDictionary.ContainsKey(poolData.Key)) return;
            _poolDictionary.TryAdd(poolData.Key, poolData);
            _poolHolderDictionary.TryAdd(poolData, holder);
        }

        private void UnregisterPool(string key)
        {
            if (!_poolDictionary.TryGetValue(key, out PoolData poolData)) return;
            DestroyAllPooledItems(poolData);
            _poolDictionary.Remove(key);
        }

        private void DestroyAllPooledItems(PoolData poolData)
        {
            while (poolData.Pool.CountInactive > 0)
            {
                PoolBehavior item = poolData.Pool.Get();
                Destroy(item.GameObject);
            }
        }

        #endregion



        #region Get And Release

        private GameObject Get(string key, Transform parent = null)
        {
            if (!_poolDictionary.TryGetValue(key, out PoolData poolData)) return null;
            PoolBehavior item = poolData.Pool.Get();
            item.Pool = poolData.Pool;
            item.GameObject.transform.SetParent(_poolHolderDictionary.GetValueOrDefault(poolData, parent));
            return item.GameObject;

        }

        private T Get<T>(string key, Transform parent = null) where T : PoolBehavior
        {
            if (!_poolDictionary.TryGetValue(key, out PoolData poolData)) return default;
            PoolBehavior item = poolData.Pool.Get();
            item.Pool = poolData.Pool;
            item.GameObject.transform.SetParent(_poolHolderDictionary.GetValueOrDefault(poolData, parent));
            return item.GameObject.GetComponent<T>();

        }

        private void Release(GameObject item)
        {
            PoolBehavior poolAble = item.GetComponent<PoolBehavior>();
            Release(poolAble);
        }

        private void Release(PoolBehavior item)
        {
            if (item != null) item.Pool.Release(item);
        }

        #endregion
    }
}
