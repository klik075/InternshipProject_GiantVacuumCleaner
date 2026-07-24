
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickDirectionAndBg : InputScreenJoystickStrategy
{
    #region Fields
    
    private readonly RectTransform _Direction;
    private readonly RectTransform _Background;

    #endregion
    
    
    
    #region Constructor & Initialize
    
    public JoystickDirectionAndBg(InputScreenJoystick joystick) : base(joystick)
    {
        var rects = Joystick.RectTransforms;
        
        // Direction
        if (!rects.TryGetValue(InputScreenJoystick.JoystickModularName.Direction, out _Direction))
        {
            Debugger.LogError("Handle not found in RectTransforms");
            return;
        }

        // Background
        if (!rects.TryGetValue(InputScreenJoystick.JoystickModularName.Background, out _Background))
        {
            Debugger.LogError("Background not found in RectTransforms");
            return;
        }
        
        // Setup Starting Position
        startingPosition = _Direction.anchoredPosition;
    }
    
    #endregion



    #region Override

    public override void PointerDownInteraction(PointerEventData eventData)
    {
        if (Joystick.ControlJoystickType != InputScreenJoystick.JoystickType.Fixed)
        {
            _Background.anchoredPosition = Joystick.AnchoredFingerPosition;
            _Direction.anchoredPosition = Joystick.AnchoredFingerPosition;
            _Background.gameObject.SetActive(true);
            _Direction.gameObject.SetActive(true);
        }

        PointerDragInteraction(eventData);
    }

    public override void PointerUpInteraction(PointerEventData eventData)
    {
        if (Joystick.ControlJoystickType != InputScreenJoystick.JoystickType.Fixed)
        {
            _Background.gameObject.SetActive(false);
            _Direction.gameObject.SetActive(false);
        }

        _Background.anchoredPosition = startingPosition;
        _Direction.anchoredPosition = startingPosition;
        _Direction.up = Vector3.zero;
        RaiseOnSendValueToControl(Vector2.zero);
    }

    public override void PointerDragInteraction(PointerEventData eventData)
    {
        Action moveStickMethod = Joystick.ControlJoystickType switch
        {
            InputScreenJoystick.JoystickType.Fixed => FixedMoveStick,
            InputScreenJoystick.JoystickType.Floating => FloatingMoveStick,
            _ => throw new ArgumentOutOfRangeException(nameof(Joystick.ControlJoystickType))
        };

        moveStickMethod.Invoke();
    }

    #endregion



    #region Process

    private void FixedMoveStick()
    {
        var anchoredFingerPos = 
            Joystick.ScreenPointToAnchoredPosition(Joystick.FingerPosition, Joystick.RootRectTransform);
        RotateDirection(anchoredFingerPos);
    }

    private void FloatingMoveStick()
    {
        var anchoredFingerPos = Joystick.AnchoredFingerPosition;
        RotateDirection(anchoredFingerPos);
    }
    
    private void RotateDirection(Vector2 anchoredFingerPos)
    {
        Vector2 directionVector;
        if (Joystick.ControlJoystickType == InputScreenJoystick.JoystickType.Floating)
        {
            // Floating 모드에서는 Background를 기준으로 회전 계산
            directionVector = anchoredFingerPos - _Background.anchoredPosition;
        }
        else
        {
            // Fixed 모드에서는 기존 방식대로 계산
            directionVector = anchoredFingerPos - startingPosition;
        }
        
        Vector2 normalizedDirection = directionVector.normalized;
        _Direction.up = normalizedDirection;
        
        RaiseOnSendValueToControl(normalizedDirection);
    }

    #endregion



    #region Utils

    public override void SetJoystickMode(InputScreenJoystick.JoystickType joystickType, Action onSetSelfRectTransformMethod)
    {
        base.SetJoystickMode(joystickType, onSetSelfRectTransformMethod);

        _Direction.up = Vector3.zero;

        switch (Joystick.ControlJoystickType)
        {
            case InputScreenJoystick.JoystickType.Fixed:
                _Direction.anchoredPosition = startingPosition;
                _Background.anchoredPosition = startingPosition;
                _Direction.gameObject.SetActive(true);
                _Background.gameObject.SetActive(true);
                break;
            case InputScreenJoystick.JoystickType.Floating:
                _Direction.gameObject.SetActive(false);
                _Background.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}
