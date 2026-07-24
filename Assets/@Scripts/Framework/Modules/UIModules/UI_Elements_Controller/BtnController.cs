using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Framework.Modules.UIModules.UI_Elements_Controller
{
    
    public abstract class BtnController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {

        [SerializeField] private float minScale = 0.8f;
        [SerializeField] private float duration = 0.5f;
        private ContentController _controller;
        private bool _hasContents;
        private void Awake()
        {
            _controller = GetComponent<ContentController>();
            _hasContents = _controller != null;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Debugger.Log("Click Event Invoke");
            ScaleClickAction();
            if (_hasContents) _controller.ExecuteActionSequence();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Debugger.Log("Down Event Invoke");
            ScaleClickAction();
            if (_hasContents) _controller.ExecuteActionSequence();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Debugger.Log("Up Event Invoke");
            ScaleClickAction();
            if (_hasContents) _controller.ExecuteActionSequence();
        }

        private void ScaleClickAction()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(minScale, duration).SetEase(Ease.InOutQuad))
                .Append(transform.DOScale(Vector3.one, duration).SetEase(Ease.InOutQuad));
            sequence.Play();
        }

        private void ScaleDownAction() => transform.DOScale(new Vector3(minScale, minScale, minScale), 0.3f).SetEase(Ease.InOutQuad);
        private void ScaleUpAction() => transform.DOScale(Vector3.one, duration).SetEase(Ease.InOutQuad);
    }
}