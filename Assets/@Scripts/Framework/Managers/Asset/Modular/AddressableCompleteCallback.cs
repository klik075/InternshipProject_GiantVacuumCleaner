 using System;
using System.Collections.Generic;
using Scripts.Framework.Managers.Asset.Core;
using Scripts.Framework.Managers.Asset.Core.Struct;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Scripts.Framework.Managers.Asset.Modular
{
    public class AddressableCompleteCallback
    {
        #region Field

        private AssetManager _assetManager;

        #endregion
        
        #region Constructor

        public AddressableCompleteCallback(AssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        #endregion



        #region Complete Init

        public void OnInitializeCompleted(AsyncOperationHandle<IResourceLocator> handle
            , Action onSucceeded = null
            , Action onFailed = null)
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                onFailed?.Invoke();
                return;
            }
        
            _assetManager.KeyAccessor(handle.Result.Keys);
            onSucceeded?.Invoke();
        }

        #endregion



        #region Complete ResourceLocation

        public void OnLoadLocationsCompleted(AsyncOperationHandle<IList<IResourceLocation>> handle
            , AssetLabelReference key
            , Action<object> onSucceeded = null
            , Action<object> onFailed = null)
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                onFailed?.Invoke(key);
                return;
            }

            bool succeeded = false;

            foreach (var location in handle.Result)
            {
                // 해당 레이블이 존재하지 않을 경우
                if (!_assetManager.Locations.ContainsKey(key.labelString))
                {
                    _assetManager.Locations.Add(key.labelString, new List<IResourceLocation>());
                }
            
                var list = _assetManager.Locations[key.labelString];
                var index = list.FindIndex(x => string.Equals(x.InternalId, location.InternalId));

                if (index >= 0) continue;
                list.Add(location);
                succeeded = true;
            }

            if (succeeded)  onSucceeded?.Invoke(key);
        }

        #endregion



        #region Complete LoadAsset

        public void OnLoadAssetCompleted<T>(AsyncOperationHandle<T> handle
            , AssetKey key
            , bool useReference
            , Action<AssetKey, T> onSucceeded = null
            , Action<AssetKey> onFailed = null) where T : Object
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                onFailed?.Invoke(key);
                return;
            }

            if (!handle.Result)
            {
                if (!_assetManager.SuppressErrorLogs)
                {
                    Debugger.LogError(useReference
                        ? Exceptions.CannotLoadAssetReference<T>(key.AssetReference.ToString())
                        : Exceptions.CannotLoadAssetKey<T>(key.AddressableName));
                }
            
                onFailed?.Invoke(key);
                return;
            }

            if (_assetManager.LoadedAssets.ContainsKey(key))
            {
                if (_assetManager.LoadedAssets[key] is not T)
                {
                    if (!_assetManager.SuppressErrorLogs)
                    {
                        if (useReference)
                        {
                            Debugger.LogError(Exceptions.AssetReferenceExist(
                                _assetManager.LoadedAssets[key].GetType(), key.AssetReference.ToString()));
                        }
                        else
                        {
                            Debugger.LogError(Exceptions.AssetKeyExist(
                                _assetManager.LoadedAssets[key].GetType(), key.AddressableName));
                        }
                    }

                    onFailed?.Invoke(key);
                    return;
                }
            }
            else
            {
                _assetManager.LoadedAssets.Add(key, handle.Result);
            }
        
            onSucceeded?.Invoke(key, handle.Result);
        }

        #endregion
    }
}
