using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

namespace Scripts.Framework.Modules.UIModules.UI_Elements_Controller
{
    [Serializable]
    public class BtnCallBack
    {
        public enum BtnCallbackType { Open, Close }
        public BtnCallbackType btnCallbackType;
        public AssetReference actionReference;
        public BtnSO eventSO;
    }
    public class BtnComponent : BtnController
    {
        private enum PointType { Click, Down, Up }
        [SerializeField] private PointType btnActionType;
        [SerializeField] private BtnCallBack callBack;

        private void Awake()
        {
            callBack.eventSO.AssignEvent(callBack.actionReference);
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (btnActionType != PointType.Click) return;
            base.OnPointerClick(eventData);
            callBack.eventSO.RaiseEvent(callBack);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (btnActionType != PointType.Down) return;
            base.OnPointerDown(eventData);
            callBack.eventSO.RaiseEvent(callBack);
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (btnActionType != PointType.Up) return;
            base.OnPointerUp(eventData);
            callBack.eventSO.RaiseEvent(callBack);
        }
    }
}