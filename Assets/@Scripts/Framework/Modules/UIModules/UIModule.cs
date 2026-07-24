using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Scripts.Framework.Managers.Asset.Core;
using Scripts.Framework.Modules.UIModules.UI_Elements_Controller;
using Scripts.Framework.Modules.UIModules.UI_Panel;
using Scripts.Framework.Modules.UIModules.UIComponent;
using Scripts.Framework.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.Framework.Modules.UIModules
{
    public class UIModule : MonoBehaviour
    {
        [SerializeField] private BtnSO uiEvents;
        [SerializeField, ReadOnly] private SerializedDictionary<int, Popup_UI> openedPopup;
        private static readonly Stack<Popup_UI> PopupStack = new();
        private GameObject _root;
        private GameObject Root
        {
            get
            {
                if (_root == null) _root = GameObject.Find("@UI_Root") ?? new GameObject("@UI_Root");
                return _root;
            }
        }

        private void Awake()
        {
            uiEvents.OnBtnEventTriggered.AddListener(HandleBtnEvent);
        }

        private void OnDestroy()
        {
            uiEvents.OnBtnEventTriggered.RemoveListener(HandleBtnEvent);
        }

        private void HandleBtnEvent(BtnCallBack callBack)
        {
            switch (callBack.btnCallbackType)
            {
                case BtnCallBack.BtnCallbackType.Open:
                    OpenPopup<Popup_UI>(callBack.actionReference);
                    break;
                case BtnCallBack.BtnCallbackType.Close:
                    ClosePopup();
                    break;
            }
        }
        private T UICreate<T>(GameObject prefab, string uiName) where T : UIBase
        {  
            GameObject uiObject = Instantiate(prefab, Root.transform);
            T uiComponent = Util.GetAddComponent<T>(uiObject);
            uiObject.name = uiName;
            return uiComponent;
        }

        private void CameraMode(GameObject gb, Canvas canvas, bool cameraMode)
        {
            if (!cameraMode) return;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
        }

        private void SortOrder(GameObject gb, bool cameraMode, string sortingName = "Popup")
        {
            Canvas canvas = Util.GetAddComponent<Canvas>(gb);
            canvas.overrideSorting = true;
            canvas.sortingLayerName = sortingName;
            canvas.sortingOrder = PopupStack.Count;
            CameraMode(gb, canvas, cameraMode);
        }
        
        public T OpenPopup<T>(bool cameraMode = true) where T : Popup_UI
        {
            string uiName = typeof(T).Name;
            GameObject uiPrefab = AMS.GetAsset<GameObject>(uiName);
            T uiObj = PushStack<T>(uiPrefab,cameraMode);
            return uiObj;
        }
        public T OpenPopup<T>(AssetReference reference, bool cameraMode = true) where T : Popup_UI
        {
            GameObject uiPrefab = AMS.GetAsset<GameObject>(reference);
            T uiObj = PushStack<T>(uiPrefab, cameraMode);
            return uiObj;
        }

        private T PushStack<T>(GameObject uiPrefab, bool cameraMode) where T : Popup_UI
        {
            string uiName = typeof(T).Name;
            T uiObj = UICreate<T>(uiPrefab, uiName);
            PopupStack.Push(uiObj);
            openedPopup[PopupStack.Count] = uiObj;
            SortOrder(uiObj.gameObject, cameraMode);
            return uiObj;
        }
        
        public static T FindUI<T>() where T : UIBase
        {
            T uiObj = FindObjectOfType<T>();
            return uiObj == null ? null : uiObj;
        }
        
        public static void ClosePopup()
        {
            if (PopupStack.Count <= 0) return;
            Popup_UI popup = PopupStack.Pop();
            if (popup != null) Destroy(popup.gameObject);
        }
    }
}