using System;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Framework.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Framework.Modules.UIModules.UI_Elements_Controller
{
    [Serializable]
    public class EaseAction
    {
        public enum ActionType { Move, Scale }
        public enum Direction { In, Out, Both }
        public enum Ease { Linear, Quad, Cubic, Quart, Quint, Sine, Back, Circ, Bounce, Elastic }
        public enum LoopAction { Yoyo, Restart, Incremental }
    }
    [Serializable]
    public class ContentAction
    {
        [SerializeField] private EaseAction.ActionType actionType;
        [SerializeField] private EaseAction.Direction direction;
        [SerializeField] private EaseAction.Ease ease = EaseAction.Ease.Linear;
        [SerializeField] private float duration;
        [SerializeField] private bool blendBeforeAction;
        [SerializeField] private bool blendAfterAction;
        [SerializeField] private bool loop;
        [SerializeField] private EaseAction.LoopAction loopAction;

        public EaseAction.ActionType ActionType => actionType;
        public EaseAction.Direction Direction => direction;
        public EaseAction.Ease Ease => ease;
        public float Duration => duration;
        public bool BlendBeforeAction => blendBeforeAction;
        public bool BlendAfterAction => blendAfterAction;
        public bool Loop => loop;
        public EaseAction.LoopAction LoopAction => loopAction;
    }
    public class ContentController : MonoBehaviour
    {
        [SerializeField] private bool playOnAwake;
        [SerializeField] private List<ContentAction> actionSequence;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 endScale;
        [SerializeField] private UnityEvent onComplete;
     
        private RectTransform _rect;
        private Vector3 _endPosition;
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _endPosition = transform.position;
        }
        private void OnEnable()
        {
            if (playOnAwake) ExecuteActionSequence();
        }

        public void ExecuteActionSequence()
        {
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < actionSequence.Count; i++)
            {
                ContentAction action = actionSequence[i];
                if (action.BlendBeforeAction && i > 0) sequence.Join(CreateTween(action));
                else sequence.Append(CreateTween(action));
                if (action.BlendAfterAction && i < actionSequence.Count - 1) sequence.Join(CreateTween(actionSequence[i + 1]));
            }
            sequence.Play();
        }
        private Tweener CreateTween(ContentAction action)
        {
            Tweener tweener;

            switch (action.ActionType)
            {
                case EaseAction.ActionType.Move:
                    tweener = action.Direction switch
                    {
                        EaseAction.Direction.In => _rect.DOMove(_endPosition, action.Duration).SetEase(ConvertEx.ConvertEase(action.Ease, action.Direction)),
                        EaseAction.Direction.Out => _rect.DOMove(startPosition, action.Duration).SetEase(ConvertEx.ConvertEase(action.Ease, action.Direction)),
                        EaseAction.Direction.Both => _rect.DOMove(_endPosition, action.Duration).SetEase(ConvertEx.ConvertEase(action.Ease, EaseAction.Direction.In))
                            .OnComplete(() => _rect.DOMove(startPosition, action.Duration).SetEase(ConvertEx.ConvertEase(action.Ease, EaseAction.Direction.Out))),
                        _ => null
                    };
                    break;
                case EaseAction.ActionType.Scale:
                    tweener = action.Direction switch
                    {
                        EaseAction.Direction.In => _rect.DOScale(endScale, action.Duration).SetEase(ConvertEx.ConvertEase(action.Ease, action.Direction)),
                        EaseAction.Direction.Out => _rect.DOScale(Vector3.one, action.Duration).SetEase(ConvertEx.ConvertEase(action.Ease, action.Direction)),
                        EaseAction.Direction.Both => _rect.DOScale(endScale, action.Duration).SetEase(ConvertEx.ConvertEase(action.Ease, EaseAction.Direction.In))
                            .OnComplete(() => _rect.DOScale(Vector3.one, action.Duration).SetEase(ConvertEx.ConvertEase(action.Ease, EaseAction.Direction.Out))),
                        _ => null
                    };
                    break;
                default:
                    return null;
            }

            if (!action.Loop) return tweener.OnComplete(()=>onComplete.Invoke());
            int loops = -1;
            LoopType loopType = action.LoopAction switch
            {
                EaseAction.LoopAction.Yoyo => LoopType.Yoyo,
                EaseAction.LoopAction.Incremental => LoopType.Incremental,
                _ => LoopType.Restart
            };
            tweener.SetLoops(loops, loopType);
            return tweener;
        }
    }
}
