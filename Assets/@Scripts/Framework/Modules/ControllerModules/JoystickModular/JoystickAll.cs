
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickAll : InputScreenJoystickStrategy
{
    #region Fields

    private readonly RectTransform _Handle;
    private readonly RectTransform _Direction;
    private readonly RectTransform _Background;

    public float HandleRange = 0.7f;
    public Vector2 Radius;

    #endregion
    
    
    public JoystickAll(InputScreenJoystick joystick) : base(joystick)
    {
        var rects = Joystick.RectTransforms;
        
        // Handle
        if (!rects.TryGetValue(InputScreenJoystick.JoystickModularName.Handle, out _Handle))
        {
            Debugger.LogError("Handle not found in RectTransforms");
            return;
        }

        // Direction
        if (!rects.TryGetValue(InputScreenJoystick.JoystickModularName.Direction, out _Direction))
        {
            Debugger.LogError("Background not found in RectTransforms");
            return;
        }
        
        // Background
        if (!rects.TryGetValue(InputScreenJoystick.JoystickModularName.Background, out _Background))
        {
            Debugger.LogError("Background not found in RectTransforms");
            return;
        }

        // Setup Starting Position
        startingPosition = _Handle.anchoredPosition;
        
        // Setup Radius
        Radius = _Background.sizeDelta / InputConfig.Input_Radius;
    }



    #region Override

    public override void PointerDownInteraction(PointerEventData eventData)
    {
        if (Joystick.ControlJoystickType != InputScreenJoystick.JoystickType.Fixed)
        {
            SetAllAnchoredPosition(Joystick.AnchoredFingerPosition);
            SetActiveAll(true);
        }
        
        PointerDragInteraction(eventData);
    }

    public override void PointerUpInteraction(PointerEventData eventData)
    {
        if (Joystick.ControlJoystickType != InputScreenJoystick.JoystickType.Fixed)
        {
            SetActiveAll(false);
        }
        
        _Direction.up = Vector3.zero;
        SetAllAnchoredPosition(startingPosition);
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
        var delta = anchoredFingerPos - startingPosition;
        RotateDirection(anchoredFingerPos);

        delta = Vector2.ClampMagnitude(delta, Radius.x * HandleRange);
        _Handle.anchoredPosition = startingPosition + delta;

        SendToDirectionValue(delta);
    }

    private void FloatingMoveStick()
    {
        var anchoredFingerPos = Joystick.AnchoredFingerPosition;
        var anchoredInitFingerPos = Joystick.AnchoredInitialFingerPosition;
        var delta = anchoredFingerPos - anchoredInitFingerPos;
        RotateDirection(anchoredFingerPos);

        delta = Vector2.ClampMagnitude(delta, Radius.x * HandleRange);
        _Handle.anchoredPosition = _Background.anchoredPosition + delta;

        SendToDirectionValue(delta);
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
    }

    private void SendToDirectionValue(Vector2 delta)
    {
        float radiusHandleX = (Radius.x * HandleRange);
        float radiusHandleY = (Radius.y * HandleRange);
        Vector2 direction = new Vector2(delta.x / radiusHandleX, delta.y / radiusHandleY).normalized;
        
        RaiseOnSendValueToControl(direction);
    }

    #endregion



    #region Utils

    private void SetActiveAll(bool isActive)
    {
        _Handle.gameObject.SetActive(isActive);
        _Direction.gameObject.SetActive(isActive);
        _Background.gameObject.SetActive(isActive);
    }

    private void SetAllAnchoredPosition(Vector2 anchoredPosition)
    {
        _Handle.anchoredPosition = anchoredPosition;
        _Direction.anchoredPosition = anchoredPosition;
        _Background.anchoredPosition = anchoredPosition;
    }

    public override void SetJoystickMode(InputScreenJoystick.JoystickType joystickType, Action onSetSelfRectTransformMethod)
    {
        base.SetJoystickMode(joystickType, onSetSelfRectTransformMethod);

        _Direction.up = Vector3.zero;

        switch (Joystick.ControlJoystickType)
        {
            case InputScreenJoystick.JoystickType.Fixed:
                SetAllAnchoredPosition(startingPosition);
                SetActiveAll(true);
                break;
            case InputScreenJoystick.JoystickType.Floating:
                SetActiveAll(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}
