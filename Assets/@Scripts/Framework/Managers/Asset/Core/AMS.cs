using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.Framework.Managers.Asset.Core
{
    /// <summary>
    /// Asset Manager Wrapper
    /// </summary>
    public static class AMS
    {
        #region Fields

        private static AssetManager _asset;

        #endregion

        #region Initializer

        public static async UniTask InitializeAssets(Action<int,int> action = null) 
        {
            _asset = AssetManager.Instance;
            
            // Initialize Addressable From UniTask
            await _asset.InitializeAddressable();
        
            // Load All Label Locations
            await _asset.InitializeLocations();
        }

        #endregion
    
        #region Get Assets (Wrapper)

        public static T GetAsset<T>(AssetReference assetReference) where T : UnityEngine.Object => _asset.GetAsset<T>(assetReference);

        public static T GetAsset<T>(string addressableName) where T : UnityEngine.Object => _asset.GetAsset<T>(addressableName);
        

        #endregion
    
        #region Load Assets (Wrapper)

        public static UniTask LoadAssets<T>(AssetLabelReference assetLabelReference,Action<int,int> action = null) where T : UnityEngine.Object
            => _asset.LoadAssets<T>(assetLabelReference, action);
        public static UniTask LoadAssets<T>(string label,Action<int,int> action = null) where T : UnityEngine.Object
            => _asset.LoadAssets<T>(label, action);

        #endregion
    
        #region Release Assets (Wrapper)

        public static void ReleaseAsset(AssetReference assetReference)
            => _asset.ReleaseAsset(assetReference);

        public static void ReleaseAsset(string addressableName)
            => _asset.ReleaseAsset(addressableName);

        public static void ReleaseAssetLabel(AssetLabelReference assetLabelReference)
            => _asset.ReleaseAssetsByLabel(assetLabelReference);

        public static void ReleaseAllAssets()
            => _asset.ReleaseAllAssets();

        public static void ReleaseLocation(object key)
            => _asset.ReleaseLocation(key);

        public static void ReleaseAllLocations()
            => _asset.ReleaseAllLocations();

        #endregion
    }
}