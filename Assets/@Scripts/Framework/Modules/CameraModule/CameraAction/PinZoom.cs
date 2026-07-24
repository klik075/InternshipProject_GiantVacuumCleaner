using DG.Tweening;
using Scripts.Framework.Modules.CameraModule.Resolution;
using UnityEngine;

namespace Scripts.Framework.Modules.CameraModule.CameraAction
{

    public class PinConfig
    {
        public bool UseTween;
        public Vector2 PinLimit;
        public float PinDuration;
        public float TweenPinDuration;
    }
    public class PinZoom : MonoBehaviour
    {
        private CameraFitterBase _camBase;
        private PinConfig _conf;

        public void PinCreate(CameraFitterBase baseCam, PinConfig conf)
        {
            _camBase = baseCam;
            _conf = conf;
            // TODO : Tick Event Subscribe (Use Update)
            // TODO : event += PinZoomMotion;
        }
        
        private void PinZoomMotion()
        {
            if (Input.touchCount != 2) return;
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            float nextSize = _camBase.Camera.orthographicSize + deltaMagnitudeDiff * _conf.PinDuration;
            if (_conf.UseTween)
            {
                nextSize = Mathf.Clamp(nextSize,  _conf.PinLimit.x,  _conf.PinLimit.y);
                _camBase.Camera.DOOrthoSize(nextSize, _conf.TweenPinDuration).SetEase(Ease.OutQuad);
            }
            else _camBase.Camera.orthographicSize = Mathf.Clamp(nextSize,  _conf.PinLimit.x,  _conf.PinLimit.y);
        }
    }
}