using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Scripts.Framework.Modules.UIModules.UI_Elements_Controller
{
    [CreateAssetMenu(fileName = "BtnEvent", menuName = "UI/Btn Events")]
    public class BtnSO : DescriptionSO
    {
        public AssetReference Reference { get; private set; }
        public UnityEvent<BtnCallBack> OnBtnEventTriggered = new();
        public void RaiseEvent(BtnCallBack callBack) =>  OnBtnEventTriggered.Invoke(callBack);
        public void AssignEvent(AssetReference assetReference) => Reference = assetReference;
        public void ResetListeners() => OnBtnEventTriggered.RemoveAllListeners();
    }
}