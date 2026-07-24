using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scripts.Framework.Managers.Asset.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CentralSingletonBase : MonoBehaviour
    {
        protected static readonly object Locked = new();
    }
    
    public class CentralProcessor : CentralSingletonBase
    {
        public static CentralProcessor I
        {
            get
            {
                lock (Locked) return GetInstance();
            }
        }
        
        private static CentralProcessor _i;
        private readonly Dictionary<Type, MonoBehaviour> _singletonObjects = new();

        private void Awake()
        {
            if (_i == null)
            {
                _i = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_i != this)
            {
                Destroy(gameObject);
            }
        }

        private static CentralProcessor GetInstance()
        {
            if (_i == null)
            {
                _i = FindObjectOfType<CentralProcessor>();
                if (_i == null)
                {
                    GameObject singleton = new GameObject { name = "[Singleton] " + nameof(CentralProcessor) };
#if DEV
                    Debugger.Log("Singleton an instance of '" + typeof(CentralProcessor) + "' created.");
#endif
                    _i = singleton.AddComponent<CentralProcessor>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _i;
        }

        private async UniTask<T> AddSingleton<T>() where T : MonoBehaviour
        {
            Type singletonComponentType = typeof(T);
            if (_singletonObjects.TryGetValue(singletonComponentType, out MonoBehaviour o)) return o as T;
            // TODO : Temp Async Load
            GameObject loadedAsset = AMS.GetAsset<GameObject>(typeof(T).Name);
            T singletonObject = Instantiate(loadedAsset, transform).GetComponent<T>();
            // T singletonObject = Instantiate(loadedAsset, transform);
            // TODO -------------------------------------------
#if DEV
            Debugger.Log($"Immediate Create Component is {typeof(T)}");
#endif
            _singletonObjects[singletonComponentType] = singletonObject;
            return singletonObject;
        }

        public async UniTask<T> GetSingleton<T>() where T : MonoBehaviour
        {
            Type singletonComponentType = typeof(T);
            if (_singletonObjects.TryGetValue(singletonComponentType, out MonoBehaviour component)) return component as T;
            return await AddSingleton<T>();
        }

        public void ReleaseComponent<T>() where T : MonoBehaviour
        {
            Type singletonComponentType = typeof(T);

            if (_singletonObjects.TryGetValue(singletonComponentType, out MonoBehaviour component))
            {
                _singletonObjects.Remove(singletonComponentType);
                Destroy(component);
            }
            else
            {
#if DEV
                Debug.LogWarning($"Component of type {singletonComponentType} does not exist in the singleton objects.");
#endif
            }
        }
    }