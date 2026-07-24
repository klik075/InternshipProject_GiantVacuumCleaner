using UnityEngine;

namespace Scripts.Framework.Modules.SingletonModule.IndividualSingleton
{
    public abstract class SingletonInternal<T> : MonoBehaviour where T : MonoBehaviour 
    {
        #region Fields

        private static T _instance;
        private static bool _isApplicationQuit;
        private static readonly object _lock = new();

        #endregion
        
        #region Properties

        protected static T Instance
        {
            get
            {
                if (_isApplicationQuit)
                {
                    Debugger.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed.");
                    return null;
                }

                lock (_lock)
                {
                    return _instance != null ? _instance : GetInstance();
                }
            }
        }

        private static T GetInstance()
        {
            _instance = FindFirstObjectByType<T>();

            if (_instance != null) return _instance;
            var singleton = new GameObject { name = "[Singleton] " + typeof(T) };
            _instance = singleton.AddComponent<T>();
            Debugger.Log("Singleton an instance of '" + typeof(T) + "' created.");

            return _instance;
        }

        #endregion



        #region Unity Events

        protected virtual void Awake()
        {
            if (_instance == null) _instance = this as T;
            else Destroy(gameObject);
        }

        private void OnApplicationQuit()
        {
            _isApplicationQuit = true;
        }

        protected virtual void OnDestroy()
        {
            lock (_lock)
            {
                if (_instance != this) return;
                _instance = null;
            }
        }

        #endregion
    }
}