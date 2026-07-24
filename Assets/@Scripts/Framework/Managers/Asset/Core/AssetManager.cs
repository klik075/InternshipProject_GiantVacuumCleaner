
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Scripts.Framework.Managers.Asset.Core.Struct;
using Scripts.Framework.Managers.Asset.Modular;
using Scripts.Framework.Managers.Interface;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Scripts.Framework.Managers.Asset.Core
{
    public class AssetManager : IndividualSingleton<AssetManager>, IManager
    {
        #region Fields
    
        public enum ExceptionHandleType { Log, Throw, Suppress }
    
        private readonly Dictionary<string, List<IResourceLocation>> _locations = new();
        private readonly Dictionary<AssetKey, Object> _loadedAssets = new();
        private readonly List<object> _keys = new();

        // TODO. 추 후 코루틴 버전 등 추가될 수 있음
        // Loader (Modular)
        private AddressableUniTaskModular _loaderUniTask;
    
        // Guid Manager
        private AssetGuidLoader _assetGuidLoader;

        // Initialize and Load Flag
        private bool _isInitialize; // false
        private bool _isLoadLocations; // false
        
        [Header("GUID TEXT")] 
        [SerializeField] private TextAsset jsonText;
        
        [Header("Debug Handle Type")] 
        [SerializeField] private ExceptionHandleType exceptionHandle;
        
        [Header("Debug Resource Streaming")] 
        [SerializeField] private bool isDebugStreaming = true;
        [SerializeField] private bool isDebugLoaded = true;
        
        [Header("Show Asset Label References")]
        [SerializeField][ReadOnly] private List<AssetLabelReference> _labels = new();
        public bool SerializeAssetLabels = false;
    
        #endregion



        #region Properties
    
        public IReadOnlyList<object> Keys => _keys;
        public Dictionary<string, List<IResourceLocation>> Locations => _locations;
        public Dictionary<AssetKey, Object> LoadedAssets => _loadedAssets;
        public AssetGuidLoader AssetGuidLoader => _assetGuidLoader;

        public bool SuppressWarningLogs { get; set; }
        public bool SuppressErrorLogs { get; set; }

        public bool IsInitialize => _isInitialize;
        public bool IsLoadLocations => _isLoadLocations;
        public bool IsDebugStreaming => isDebugStreaming;
        public bool IsDebugLoaded => isDebugLoaded;
        public ExceptionHandleType ExceptionHandle => exceptionHandle;

        public List<AssetLabelReference> Labels
        {
            get => _labels;
            set => _labels = value;
        }

        #endregion



        #region Initializer

        public async UniTask InitializeAddressable()
        {
            _loaderUniTask = new AddressableUniTaskModular(this);
            _assetGuidLoader = new AssetGuidLoader(jsonText);

            await _loaderUniTask.InitializeAddressable();
        
            DebugInitialize();
        }

        public async UniTask InitializeLocations()
        {
            for (int idx = 0; idx < Labels.Count; ++idx)
            {
                Debugger.LogWarning(Labels[idx].labelString);
                await _loaderUniTask.LoadLocationsAsync(_labels[idx], typeof(Object));
            }

            DebugStreamingResources();
        }

        public async UniTask LoadAssets<T>(AssetLabelReference assetLabelReference, Action<int,int> action= null) where T : Object
        {
            await _loaderUniTask.LoadAssetsAsync<T>(assetLabelReference, action);
        }
        
        public async UniTask LoadAssets<T>(string label, Action<int,int> action = null) where T : Object
        {
            await _loaderUniTask.LoadAssetsAsync<T>(label, action);
        }

        #endregion



        #region Get Assets
    
        public T GetAsset<T>(AssetReference assetReference) where T : Object
        {
            var assetReferenceKey = new AssetKey(assetReference);
            if (IsValidKey(assetReferenceKey, out var key, out bool isReference)) return GetAssetInternal<T>(key);
            HandleInvalidKeyException(key);
            return default;
        }
    
        public T GetAsset<T>(string addressableName) where T : Object
        {
            var addressableKey = new AssetKey(addressableName);
            if (IsValidKey(addressableKey, out var key, out bool isReference)) return GetAssetInternal<T>(key);
            HandleInvalidKeyException(key);
            return default;
        }
        
        private T GetAssetInternal<T>(AssetKey assetKey) where T : Object
        {
            if (!_loadedAssets.ContainsKey(assetKey))
            {
                if (!SuppressWarningLogs)  Debugger.LogWarning(Exceptions.CannotFindAssetByKey(assetKey.ToString()));
                return default;
            }
            if (_loadedAssets[assetKey] is T asset) return asset;
            if (!SuppressWarningLogs) Debugger.LogWarning(Exceptions.AssetKeyNotInstanceOf<T>(assetKey.ToString()));
            return default;
        }
        
        private async UniTask<T> GetAssetAsyncInternal<T>(AssetKey assetKey) where T : Object
        {
            if (!_loadedAssets.ContainsKey(assetKey))
            {
                if (!SuppressWarningLogs) Debugger.LogWarning(Exceptions.CannotFindAssetByKey(assetKey.ToString()));
                AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetKey.ToString());
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    T loadedAsset = handle.Result;
                    _loadedAssets[assetKey] = loadedAsset;
                    return loadedAsset;
                }
                if (!SuppressWarningLogs) Debugger.LogWarning($"Failed to load asset with key {assetKey.ToString()}");
                return default;
            }
            if (_loadedAssets[assetKey] is T asset) return asset;
            if (!SuppressWarningLogs) Debugger.LogWarning(Exceptions.AssetKeyNotInstanceOf<T>(assetKey.ToString()));
            return default;
        }
        
        #endregion



        #region Release

        public void ReleaseAsset(AssetReference assetReference)
        {
            var assetReferenceKey = new AssetKey(assetReference);
            if (!IsValidKey(assetReferenceKey, out var key, out bool isReference))
            {
                HandleInvalidKeyException(key);
                return;
            }
            ReleaseAssetInternal(key);
        }

        public void ReleaseAsset(string addressableName)
        {
            var addressableKey = new AssetKey(addressableName);
            if (!IsValidKey(addressableKey, out var key, out bool isReference))
            {
                HandleInvalidKeyException(key);
                return;
            }
            ReleaseAssetInternal(key);
        }

        private void ReleaseAssetInternal(AssetKey assetKey)
        {
            if (_loadedAssets.TryGetValue(assetKey, out var loadedAsset))
            {
                Debugger.Log($"Releasing asset: {assetKey.AddressableName}");
                Addressables.Release(loadedAsset);
                _loadedAssets.Remove(assetKey);
            }
            else
            {
                Debugger.LogWarning($"Asset not found for release: {assetKey.AddressableName}");
            }
        }

        public void ReleaseAllAssets()
        {
            foreach (var assetKey in _loadedAssets.Keys.ToList())
            {
                Debugger.Log($"Releasing asset: {assetKey.AddressableName}");
                Addressables.Release(_loadedAssets[assetKey]);
            }
            _loadedAssets.Clear();
        }

        public void ReleaseLocation(object key)
        {
            if (_locations.TryGetValue(key.ToString(), out var locations))
            {
                foreach (var location in locations)
                {
                    Addressables.Release(location);
                }
                _locations.Remove(key.ToString());
            }
            else
            {
                Debugger.LogWarning($"Location not found for release: {key}");
            }
        }

        public void ReleaseAllLocations()
        {
            foreach (var key in _locations.Keys.ToList())
            {
                foreach (var location in _locations[key])
                {
                    Addressables.Release(location);
                }
            }
            _locations.Clear();
        }

        public void ReleaseAssetsByLabel(AssetLabelReference label)
        {
            if (!_locations.TryGetValue(label.labelString, out var locations))
            {
                Debugger.LogWarning($"No locations found for label : {label.labelString}");
                return;
            }

            foreach (var location in locations)
            {
                var assetKey = new AssetKey(location.PrimaryKey);

                if (_loadedAssets.TryGetValue(assetKey, out var loadedAsset))
                {
                    Debugger.Log($"Releasing asset: {location.PrimaryKey}");
                    _loadedAssets.Remove(assetKey);
                    Addressables.Release(loadedAsset);
                }
                else
                {
                    Debugger.LogWarning($"Asset not found for release: {location.PrimaryKey}");
                }
            }
            
            _locations.Remove(label.labelString);
        }

        #endregion



        #region Utils

        private void DebugInitialize()
        {
            if (_isInitialize)
            {
                Debugger.Log("Addressable is initialize complete.");
            }
            else
            {
                Debugger.LogError("Addressable can't initialize.");
            }
        }

        private void DebugStreamingResources()
        {
            if (!isDebugStreaming) return;
        
            foreach (var location in _locations)
            {
                Debugger.Log("Location Key : " + location.Key);
                foreach (var value in location.Value)
                {
                    Debugger.Log("Value To String : " + value);
                }
            }
        }

        public bool IsValidKey(AssetKey assetKey, out AssetKey resultKey, out bool isReference)
        {
            resultKey = default;
            isReference = false;

            if (assetKey.AssetReference == null && string.IsNullOrEmpty(assetKey.AddressableName))
            {
                switch (ExceptionHandle)
                {
                    case ExceptionHandleType.Throw:
                        throw new ArgumentNullException(nameof(assetKey), 
                            "Both AssetReferenceGuid and AddressableName are null or empty");
                    case ExceptionHandleType.Log:
                        Debugger.LogException(new ArgumentNullException(nameof(assetKey), 
                            "Both AssetReferenceGuid and AddressableName are null or empty"));
                        break;
                }
                return false;
            }

            // Check by AddressableName
            if (!string.IsNullOrEmpty(assetKey.AddressableName))
            {
                var keys = _loadedAssets.Keys.ToList(); // _loadedAssets.Keys를 List로 변환
                foreach (var key in keys.Where(key => key.AddressableName == assetKey.AddressableName))
                {
                    resultKey = key;
                    isReference = false;
                    return true;
                }
            }

            // Check by AssetReferenceGuid
            if (assetKey.AssetReference == null) return false;
            {
                var keys = _loadedAssets.Keys.ToList(); // _loadedAssets.Keys를 List로 변환
                foreach (var key in keys.Where(key => key.AssetReference.AssetGUID == assetKey.AssetReference.AssetGUID))
                {
                    resultKey = key;
                    isReference = true;
                    return true;
                }
            }

            return false;
        }

        private void HandleInvalidKeyException(object key)
        {
            switch (ExceptionHandle)
            {
                case ExceptionHandleType.Throw:
                    throw new InvalidKeyException(key);
                case ExceptionHandleType.Log:
                    Debugger.LogException(new InvalidKeyException(key));
                    break;
            }
        }

        public void SetInitializeFlag(bool isInitialize = true)
        {
            _isInitialize = isInitialize;
        }

        public void SetLoadLocationFlag(bool isLoadLocations = true)
        {
            _isLoadLocations = isLoadLocations;
        }

        public void KeyAccessor(IEnumerable<object> keys)
        {
            _keys.AddRange(keys);
        }

        public void Clear()
        {
            _keys.Clear();
            _locations.Clear();
            _loadedAssets.Clear();
        }

        private void OnDisable()
        {
            Clear();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        
            OnDisable();
        }

        #endregion
    }
}