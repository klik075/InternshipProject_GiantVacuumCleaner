using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Scripts.Framework.Managers.ObjectPool.Modular
{
    [Serializable]
    public class PoolData
    {
        #region Fields

        [SerializeField] private string _key;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _warmUpCount;

        #endregion



        #region Properties

        public string Key => _key;
        public int WarmUpCount => _warmUpCount;
        public GameObject Prefab => _prefab;
        public IObjectPool<PoolBehavior> Pool { get; set; }

        #endregion



        #region Constructor

        public PoolData(string key, GameObject prefab, int warmUpCount = 0)
        {
            _key = key;
            _prefab = prefab;
            _warmUpCount = warmUpCount;
        }

        #endregion
    }
}