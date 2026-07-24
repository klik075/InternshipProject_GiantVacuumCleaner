using System;
using UnityEngine.AddressableAssets;

namespace Scripts.Framework.Managers.Asset.Modular
{
    public static class Exceptions
    {
        #region Const Fields

        private const string ExCannotFindAssetByKey = "Cannot find any asset by key={0}.";
        private const string ExNoInstanceKeyInitialized = "No instance with key={0} has been instantiated through AddressablesManager.";
        private const string ExNoInstanceReferenceInitialized = "No instance with key={0} has been instantiated through AddressablesManager.";
        private const string ExAssetKeyNotInstanceOf = "The asset with key={0} is not an instance of {1}.";
        private const string ExAssetReferenceNotInstanceOf = "The asset with reference={0} is not an instance of {1}.";
        private const string ExNoSceneKeyLoaded = "No scene with key={0} has been loaded through AddressablesManager.";
        private const string ExNoSceneReferenceLoaded = "No scene with reference={0} has been loaded through AddressablesManager.";
        private const string ExCannotLoadAssetKey = "Cannot load any asset of type={0} by key={1}.";
        private const string ExCannotLoadAssetReference = "Cannot load any asset of type={0} by reference={1}.";
        private const string ExAssetKeyExist = "An asset of type={0} has been already registered with key={1}.";
        private const string ExAssetReferenceExist = "An asset of type={0} has been already registered with reference={1}.";
        private const string ExCannotInstantiateKey = "Cannot instantiate key={0}.";
        private const string ExCannotInstantiateReference = "Cannot instantiate reference={0}.";
        private const string ExExitsKey = "Already Exist Key In Dictonary Key : {0}.";
    
        public static readonly InvalidKeyException InvalidReference = new("Reference is invalid.");
        #endregion



        #region Get Exception Comment

        public static string CannotFindAssetByKey(string key) => string.Format(ExCannotFindAssetByKey, key);
        public static string NoInstanceKeyInitialized(string key) => string.Format(ExNoInstanceKeyInitialized, key);
        public static string NoInstanceReferenceInitialized(string key) => string.Format(ExNoInstanceReferenceInitialized, key);
        public static string AssetKeyNotInstanceOf<T>(string key) => string.Format(ExAssetKeyNotInstanceOf, key, typeof(T));
        public static string AssetReferenceNotInstanceOf<T>(string key) => string.Format(ExAssetReferenceNotInstanceOf, key, typeof(T));
        public static string NoSceneKeyLoaded(string key) => string.Format(ExNoSceneKeyLoaded, key);
        public static string NoSceneReferenceLoaded(string key) => string.Format(ExNoSceneReferenceLoaded, key);
        public static string CannotLoadAssetKey<T>(string key) => string.Format(ExCannotLoadAssetKey, typeof(T), key);
        public static string CannotLoadAssetReference<T>(string key) => string.Format(ExCannotLoadAssetReference, typeof(T), key);
        public static string AssetKeyExist(Type type, string key) => string.Format(ExAssetKeyExist, type, key);
        public static string AssetReferenceExist(Type type, string key) => string.Format(ExAssetReferenceExist, type, key);
        public static string CannotInstantiateKey(string key) => string.Format(ExCannotInstantiateKey, key);
        public static string CannotInstantiateReference(string key) => string.Format(ExCannotInstantiateReference, key);
        public static string AlreadyExitsKey(string key) => string.Format(ExExitsKey, key);

        #endregion
    }
}