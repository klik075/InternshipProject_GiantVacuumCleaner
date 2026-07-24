using Scripts.Framework.Managers.Asset.Core;
using Scripts.Framework.Modules.DeviceModules;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class LocalDemo_UI : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    private async void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int i1 = i;
            buttons[i].onClick.AddListener(()=> ChangeLanguage(i1));
        }
        ChangeLanguage((int)DeviceManager.CurrentLanguage);
    }

    private void ChangeLanguage(int num)
    {
        Image img = buttons[num].GetComponentInParent<Image>();
        img.color = Color.green;
        DeviceManager.CurrentLanguage = (Language)num;
    }
}
