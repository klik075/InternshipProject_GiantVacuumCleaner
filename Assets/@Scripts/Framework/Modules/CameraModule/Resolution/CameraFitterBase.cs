using System;
using Scripts.Framework.Modules.CameraModule.CameraAction;
using UnityEngine;

namespace Scripts.Framework.Modules.CameraModule.Resolution
{
    public class CameraFitterBase : MonoBehaviour
    {
        public Camera Camera { get; protected set; }
        protected float[] TargetSizes;
        protected float[] CameraSizes;
        [SerializeField] private bool usePinZoom;
        [SerializeField] private bool useTween;
        [SerializeField] private float zoomDuration = 0.01f;
        [SerializeField] private float tweenZoomDuration = 1f;
        [SerializeField] private Vector2 pinSize = new(3f, 7f);
            
        private int _index;

        protected virtual void Awake()
        {
            Camera = GetComponent<Camera>();
            DynamicCameraSize();
            UsePin();
        }

        private void UsePin()
        {
            if (!usePinZoom) return;
            PinConfig config = new PinConfig
            {
                UseTween = useTween,
                PinLimit = pinSize,
                PinDuration = zoomDuration,
                TweenPinDuration = tweenZoomDuration
            };
            PinZoom zoom = gameObject.AddComponent<PinZoom>();
            zoom.PinCreate(this, config);
        }

        private void DynamicCameraSize()
        {
            Camera dynamicCamera = Camera.main;
            float resolution = Screen.width / (float)Screen.height;
            float closestAspect = FindClosestAspectRatio(resolution);
            _index = Array.IndexOf(TargetSizes, closestAspect);
            if (_index == -1 || _index >= CameraSizes.Length) return;
            if (dynamicCamera != null) dynamicCamera.orthographicSize = CameraSizes[_index];
        }

        private float FindClosestAspectRatio(float currentAspect)
        {
            float closest = TargetSizes[0];
            float minDifference = Mathf.Abs(currentAspect - closest);
            foreach (float aspect in TargetSizes)
            {
                float difference = Mathf.Abs(currentAspect - aspect);
                if (!(difference < minDifference)) continue;
                minDifference = difference;
                closest = aspect;
            }
            return closest;
        }
    }
}