
using UnityEngine.AddressableAssets;

namespace Scripts.Framework.Managers.Asset.Core.Struct
{
    public readonly struct AssetKey
    {
        #region Fields

        public readonly string AddressableName;
        public readonly AssetReference AssetReference;

        #endregion
    
        #region Constructor

        public AssetKey(string addressableName)
        {
            AddressableName = addressableName;
            AssetReference = null;
        }
    
        public AssetKey(AssetReference assetReference)
        {
            AddressableName = null;
            AssetReference = assetReference;
        }

        public AssetKey(string addressableName, AssetReference assetReference)
        {
            AddressableName = addressableName;
            AssetReference = assetReference;
        }

        #endregion
    
        #region Utils
    
        // Dictionary Contain을 위한 유틸 오버라이드 메서드
        public override int GetHashCode()
        {
            return (AddressableName?.GetHashCode() ?? 0) ^ (AssetReference?.GetHashCode() ?? 0);
        }

        public override bool Equals(object obj)
        {
            if (obj is AssetKey key)
            {
                return AddressableName == key.AddressableName && AssetReference == key.AssetReference;
            }
            return false;
        }
        #endregion
    }
}