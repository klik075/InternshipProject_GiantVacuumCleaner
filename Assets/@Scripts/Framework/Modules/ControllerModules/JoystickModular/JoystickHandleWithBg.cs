
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickHandleWithBg : InputScreenJoystickStrategy
{
    #region Fields
    
    private readonly RectTransform _Handle;
    private readonly RectTransform _Background;

    public float HandleRange = 0.7f;
    public Vector2 Radius;

    #endregion
    
    #region Constructor

    public JoystickHandleWithBg(InputScreenJoystick joystick) : base(joystick)
    {
        var rects = Joystick.RectTransforms;
        
        // Handle
        if (!rects.TryGetValue(InputScreenJoystick.JoystickModularName.Handle, out _Handle))
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
        startingPosition = _Handle.anchoredPosition;
        
        // Setup Radius
        Radius = _Background.sizeDelta / InputConfig.Input_Radius;
    }
    
    #endregion



    #region Override

    public override void PointerDownInteraction(PointerEventData eventData)
    {
        if (Joystick.ControlJoystickType != InputScreenJoystick.JoystickType.Fixed)
        {
            _Background.anchoredPosition = Joystick.AnchoredFingerPosition;
            _Handle.anchoredPosition = Joystick.AnchoredFingerPosition;
            _Background.gameObject.SetActive(true);
            _Handle.gameObject.SetActive(true);
        }

        PointerDragInteraction(eventData);
    }

    public override void PointerUpInteraction(PointerEventData eventData)
    {
        if (Joystick.ControlJoystickType != InputScreenJoystick.JoystickType.Fixed)
        {
            _Background.gameObject.SetActive(false);
            _Handle.gameObject.SetActive(false);
        }

        _Background.anchoredPosition = startingPosition;
        _Handle.anchoredPosition = startingPosition;
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

        delta = Vector2.ClampMagnitude(delta, Radius.x * HandleRange);
        _Handle.anchoredPosition = startingPosition + delta;

        SendToDirectionValue(delta);
    }

    private void FloatingMoveStick()
    {
        var anchoredFingerPos = Joystick.AnchoredFingerPosition;
        var anchoredInitFingerPos = Joystick.AnchoredInitialFingerPosition;
        var delta = anchoredFingerPos - anchoredInitFingerPos;

        delta = Vector2.ClampMagnitude(delta, Radius.x * HandleRange);
        _Handle.anchoredPosition = _Background.anchoredPosition + delta;

        SendToDirectionValue(delta);
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

    public override void SetJoystickMode(InputScreenJoystick.JoystickType joystickType, Action onSetSelfRectTransformMethod)
    {
        base.SetJoystickMode(joystickType, onSetSelfRectTransformMethod);

        switch (Joystick.ControlJoystickType)
        {
            case InputScreenJoystick.JoystickType.Fixed:
                _Handle.anchoredPosition = startingPosition;
                _Background.anchoredPosition = startingPosition;
                _Handle.gameObject.SetActive(true);
                _Background.gameObject.SetActive(true);
                break;
            case InputScreenJoystick.JoystickType.Floating:
                _Handle.gameObject.SetActive(false);
                _Background.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}
