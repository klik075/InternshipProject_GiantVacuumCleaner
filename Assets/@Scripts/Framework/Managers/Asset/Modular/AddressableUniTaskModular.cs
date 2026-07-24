using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scripts.Framework.Managers.Asset.Core;
using Scripts.Framework.Managers.Asset.Core.Struct;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Scripts.Framework.Managers.Asset.Modular
{
    public class AddressableUniTaskModular
    {
        #region Fields

        private AssetManager _assetManager;
        private AddressableCompleteCallback _completeCallback;

        #endregion



        #region Constructor & Initialize

        public AddressableUniTaskModular(AssetManager assetManager)
        {
            _assetManager = assetManager;
            _completeCallback = new AddressableCompleteCallback(_assetManager);
        }

        /// <summary>
        /// Addressable Initializer
        /// </summary>
        /// <returns>초기화 성공 여부를 IResourceLocator로 판단</returns>
        public async UniTask<IResourceLocator> InitializeAddressable(bool autoReleaseHandle = true)
        {
            // 혹시 모를 상황을 대비한 초기화 전 클리어
            _assetManager.Clear();

            try
            {
                AsyncOperationHandle<IResourceLocator> operationHandle = Addressables.InitializeAsync(false);
                await operationHandle;
                _completeCallback.OnInitializeCompleted(operationHandle, () => _assetManager.SetInitializeFlag());
                IResourceLocator operationResult = operationHandle.Result;
                if (!autoReleaseHandle) return operationResult;
                Debugger.Log("Auto Released 'Addressable Initializer OperationHandle'.");
                Addressables.Release(operationHandle);

                return operationResult;
            }
            catch (Exception exception)
            {
                switch (_assetManager.ExceptionHandle)
                {
                    case AssetManager.ExceptionHandleType.Throw:
                        throw;
                    case AssetManager.ExceptionHandleType.Log:
                        Debugger.LogException(exception);
                        break;
                }

                return default;
            }
        }

        #endregion



        #region Load Location

        public async UniTask<IList<IResourceLocation>> LoadLocationsAsync(object key, Type type = null)
        {
            if (key == null)
            {
                switch (_assetManager.ExceptionHandle)
                {
                    case AssetManager.ExceptionHandleType.Throw: 
                        throw new InvalidKeyException((object)null);
                    case AssetManager.ExceptionHandleType.Log: Debugger.LogException(new InvalidKeyException((object)null));
                        break;
                }

                return null;
            }

            try
            {
                AsyncOperationHandle<IList<IResourceLocation>> operationHandle = Addressables.LoadResourceLocationsAsync(key, type);
                await operationHandle;
                _completeCallback.OnLoadLocationsCompleted(operationHandle, key as AssetLabelReference, _ => _assetManager.SetLoadLocationFlag());
                return operationHandle.Result;
            }
            catch (Exception exception)
            {
                switch (_assetManager.ExceptionHandle)
                {
                    case AssetManager.ExceptionHandleType.Throw: 
                        throw;
                    case AssetManager.ExceptionHandleType.Log: Debugger.LogException(exception);
                        break;
                }
            
                return null;
            }
        }

        #endregion



        #region Load Asset

        public async UniTask LoadAssetsAsync<T>(AssetLabelReference labelReference
            , Action<int,int> onCallbackLoaded = null
            , Action onCallbackCompleted = null) where T : Object
        {
            if (labelReference == null)
            {
                switch (_assetManager.ExceptionHandle)
                {
                    case AssetManager.ExceptionHandleType.Throw:
                        throw new InvalidKeyException((object)null);
                    case AssetManager.ExceptionHandleType.Log:
                        Debugger.LogException(new InvalidKeyException((object)null));
                        break;
                }

                return;
            }

            // Async
            try
            {
                if (!_assetManager.Locations.TryGetValue(labelReference.labelString, out var locations))
                {
                    if (_assetManager.IsDebugLoaded)
                    {
                        Debugger.LogWarning("Label Reference is can't Found.");
                    }
                    return;
                }
            
                int loadedCount = 0;

                foreach (var location in locations)
                {
                    var assetReference = new AssetReference(_assetManager.AssetGuidLoader.GetGuidForPath(location.InternalId));
                    var assetKey = new AssetKey(location.PrimaryKey, assetReference);
                    var asset = await LoadAssetAsync<T>(assetKey, onCallbackLoaded);

                    if (asset != null)
                    {
                        if (_assetManager.IsDebugLoaded)
                        {
                            Debugger.Log($"'{asset.name}' is Loaded.\n"
                                         + $"Location KEY : {location.PrimaryKey}\n"
                                         + $"AssetReference : {assetReference.RuntimeKey} / {assetReference.AssetGUID}");
                        }
                    
                        ++loadedCount;
                        onCallbackLoaded?.Invoke(loadedCount, locations.Count);
                    }
                    else
                    {
                        if (_assetManager.IsDebugLoaded)
                        {
                            Debugger.LogWarning("Asset NULL");
                        }
                    }
                }

                if (_assetManager.IsDebugLoaded)
                {
                    Debugger.Log($"Loaded {loadedCount} assets for label: {labelReference.labelString}");
                }
                
                onCallbackCompleted?.Invoke();
            }
            catch (Exception exception)
            {
                switch (_assetManager.ExceptionHandle)
                {
                    case AssetManager.ExceptionHandleType.Throw:
                        throw;
                    case AssetManager.ExceptionHandleType.Log:
                        Debugger.LogException(exception);
                        break;
                }
            }
        }
        
                public async UniTask LoadAssetsAsync<T>(string label
            , Action<int, int> onCallbackLoaded = null
            , Action onCallbackCompleted = null) where T : Object
        {
            if (label == null)
            {
                switch (_assetManager.ExceptionHandle)
                {
                    case AssetManager.ExceptionHandleType.Throw:
                        throw new InvalidKeyException((object)null);
                    case AssetManager.ExceptionHandleType.Log:
                        Debugger.LogException(new InvalidKeyException((object)null));
                        break;
                }

                return;
            }

            // Async
            try
            {
                if (!_assetManager.Locations.TryGetValue(label, out var locations))
                {
                    if (_assetManager.IsDebugLoaded)
                    {
                        Debugger.LogWarning("Label Reference is can't Found.");
                    }
                    return;
                }
            
                int loadedCount = 0;

                foreach (var location in locations)
                {
                    var assetReference = new AssetReference(_assetManager.AssetGuidLoader.GetGuidForPath(location.InternalId));
                    var assetKey = new AssetKey(location.PrimaryKey, assetReference);
                    var asset = await LoadAssetAsync<T>(assetKey, onCallbackLoaded);

                    if (asset != null)
                    {
                        if (_assetManager.IsDebugLoaded)
                        {
                            Debugger.Log($"'{asset.name}' is Loaded.\n"
                                         + $"Location KEY : {location.PrimaryKey}\n"
                                         + $"AssetReference : {assetReference.RuntimeKey} / {assetReference.AssetGUID}");
                        }
                    
                        ++loadedCount;
                        onCallbackLoaded?.Invoke(loadedCount, locations.Count);
                    }
                    else
                    {
                        if (_assetManager.IsDebugLoaded)
                        {
                            Debugger.LogWarning("Asset NULL");
                        }
                    }
                }

                if (_assetManager.IsDebugLoaded)
                {
                    Debugger.Log($"Loaded {loadedCount} assets for label: {label}");
                }
                
                onCallbackCompleted?.Invoke();
            }
            catch (Exception exception)
            {
                switch (_assetManager.ExceptionHandle)
                {
                    case AssetManager.ExceptionHandleType.Throw:
                        throw;
                    case AssetManager.ExceptionHandleType.Log:
                        Debugger.LogException(exception);
                        break;
                }
            }
        }

        public async UniTask<T> LoadAssetAsync<T>(AssetKey assetKey, Action<int,int> onCallback = null) where T : Object
        {
            // Key Available Check
            if (_assetManager.IsValidKey(assetKey, out var key, out bool isReference))
            {
                switch (_assetManager.ExceptionHandle)
                {
                    case AssetManager.ExceptionHandleType.Throw:
                        throw Exceptions.InvalidReference;
                    case AssetManager.ExceptionHandleType.Log:
                        Debugger.LogException(new InvalidKeyException(assetKey));
                        break;
                }

                return null;
            }
        
            // Exist Asset
            if (_assetManager.LoadedAssets.TryGetValue(assetKey, out Object loadedAsset))
            {
                if (loadedAsset is T asset) return asset;
                if (!_assetManager.SuppressWarningLogs) Debugger.LogWarning(Exceptions.AssetReferenceNotInstanceOf<T>(assetKey.ToString()));
            }
        
            // Async
            try
            {
                AsyncOperationHandle<T> operationHandle = isReference ? assetKey.AssetReference.LoadAssetAsync<T>() : Addressables.LoadAssetAsync<T>(assetKey.AddressableName);
                await operationHandle;
                _completeCallback.OnLoadAssetCompleted(operationHandle, assetKey, isReference);
                //onCallback?.Invoke();
                return operationHandle.Result;
            }
            catch (Exception exception)
            {
                switch (_assetManager.ExceptionHandle)
                {
                    case AssetManager.ExceptionHandleType.Throw:
                        throw;
                    case AssetManager.ExceptionHandleType.Log:
                        Debugger.LogException(exception);
                        break;
                }
                return null;
            }
        }
        
        #endregion
    }
}
