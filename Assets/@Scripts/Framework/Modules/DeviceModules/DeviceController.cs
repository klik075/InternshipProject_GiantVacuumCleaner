using UnityEngine;

namespace Scripts.Framework.Modules.DeviceModules
{
    public class DeviceController : MonoBehaviour
    {
        protected DeviceManager Manager;
        protected bool _init;
        //protected virtual void Awake()
        //{

        //}
        protected virtual async void Init()
        {
            if (_init) return;
            _init = true;
            Manager = await CentralProcessor.I.GetSingleton<DeviceManager>();
        }
    }
}