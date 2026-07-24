
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class JoystickOnlyHandle : InputScreenJoystickStrategy
{
    #region Fields
    
    private readonly RectTransform _handle;

    public float MoveRange = 100f;

    #endregion



    #region Constructor

    public JoystickOnlyHandle(InputScreenJoystick joystick) : base(joystick)
    {
        var rects = Joystick.RectTransforms;
        
        // Handle
        if (!rects.TryGetValue(InputScreenJoystick.JoystickModularName.Handle, out _handle))
        {
            Debugger.LogError("Handle not found in RectTransforms");
            return;
        }

        // Setup Starting Pos
        startingPosition = _handle.anchoredPosition;
    }

    #endregion



    #region Override

    public override void PointerDownInteraction(PointerEventData eventData)
    {
        if (Joystick.ControlJoystickType != InputScreenJoystick.JoystickType.Fixed)
        {
            _handle.anchoredPosition = Joystick.AnchoredFingerPosition;
            _handle.gameObject.SetActive(true);
        }

        PointerDragInteraction(eventData);
    }

    public override void PointerUpInteraction(PointerEventData eventData)
    {
        if (Joystick.ControlJoystickType != InputScreenJoystick.JoystickType.Fixed)
        {
            _handle.gameObject.SetActive(false);
        }
        
        _handle.anchoredPosition = startingPosition;
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

        delta = Vector2.ClampMagnitude(delta, MoveRange);
        _handle.anchoredPosition = startingPosition + delta;

        SendToDirectionValue(delta);
    }

    private void FloatingMoveStick()
    {
        var anchoredFingerPos = Joystick.AnchoredFingerPosition;
        var anchoredInitFingerPos = Joystick.AnchoredInitialFingerPosition;
        var delta = anchoredFingerPos - anchoredInitFingerPos;

        delta = Vector2.ClampMagnitude(delta, MoveRange);
        _handle.anchoredPosition = anchoredInitFingerPos + delta;
        
        SendToDirectionValue(delta);
    }

    private void SendToDirectionValue(Vector2 delta)
    {
        Vector2 direction = new Vector2(delta.x / MoveRange, delta.y / MoveRange).normalized;
        
        RaiseOnSendValueToControl(direction);
    }

    #endregion
    
    
    
    #region Utils

    public override void SetJoystickMode(InputScreenJoystick.JoystickType joystickType, Action onSetSelfRectTransformMethod)
    {
        base.SetJoystickMode(joystickType, onSetSelfRectTransformMethod);
        
        switch (Joystick.ControlJoystickType)
        {
            case InputScreenJoystick.JoystickType.Fixed:
                _handle.anchoredPosition = startingPosition;
                _handle.gameObject.SetActive(true);
                break;
            case InputScreenJoystick.JoystickType.Floating:
                _handle.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}
