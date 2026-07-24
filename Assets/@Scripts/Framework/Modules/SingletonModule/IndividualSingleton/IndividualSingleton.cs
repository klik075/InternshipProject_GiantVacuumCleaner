using UnityEngine;

    public abstract class IndividualSingleton<T> : MonoBehaviour where T : MonoBehaviour 
    {
        #region Fields

        private static T _instance;
        private static bool _isApplicationQuit;
        private static readonly object Lock = new();

        #endregion
        
        #region Properties

        public static T Instance
        {
            get
            {
                if (_isApplicationQuit)
                {
#if DEV
                    Debugger.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed.");
#endif
                    return null;
                }
                lock (Lock) return _instance != null ? _instance : GetInstance();
            }
        }
        
        private static T GetInstance()
        {
            if (_instance != null) return _instance;
            _instance = FindFirstObjectByType<T>();
            if (_instance != null) return _instance;
            GameObject singleton = new GameObject { name = "[Singleton] " + typeof(T).Name };
            _instance = singleton.AddComponent<T>();
#if DEV
            Debugger.Log("Singleton an instance of '" + typeof(T) + "' created.");
#endif
            return _instance;
        }

        #endregion
        
        #region Unity Events

        protected virtual void Awake()
        {
            if (_instance == null || _instance == this) _instance = this as T;
            else  Destroy(gameObject);
        }

        protected virtual void OnApplicationQuit()
        {
            _isApplicationQuit = true;
        }

        protected virtual void OnDestroy()
        {
            lock (Lock)
            {
                if (_instance != this) return;
                _instance = null;
            }
        }

        #endregion
    }
