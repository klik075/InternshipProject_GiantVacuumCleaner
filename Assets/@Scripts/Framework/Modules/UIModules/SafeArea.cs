using UnityEngine;

namespace Scripts.Framework.Modules.UIModules
{
    public class SafeArea : MonoBehaviour
    {
        private void Start()
        {
            if (Screen.safeArea.size.y == Screen.height) return;
            int offsetY = 20;
            if (Application.platform == RuntimePlatform.IPhonePlayer) offsetY = 60;
            RectTransform rt = GetComponent<RectTransform>();
            Vector2 anchorMin = Screen.safeArea.position;
            Vector2 anchorMax = Screen.safeArea.position + Screen.safeArea.size + new Vector2(0, offsetY);
            anchorMin.x = rt.anchorMin.x;
            anchorMax.x = rt.anchorMax.x;
            anchorMin.y /= Screen.height;
            anchorMax.y /= Screen.height;
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
        }
    }
}