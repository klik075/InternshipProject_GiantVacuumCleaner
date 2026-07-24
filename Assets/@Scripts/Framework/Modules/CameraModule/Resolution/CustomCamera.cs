using UnityEngine;

namespace Scripts.Framework.Modules.CameraModule.Resolution
{
    public class CustomCamera : CameraFitterBase
    {
        protected override void Awake()
        {
            Camera = Camera.main;
            TargetSizes = new []{ 9f / 16f, 9f / 20f, 9f / 19f, 9f / 22f }; 
            CameraSizes = new []{ 5.3f, 5.4f, 5.3f, 5.8f };
            base.Awake();
        }
    }
}