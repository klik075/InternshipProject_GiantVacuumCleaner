
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InputScreenJoystickStrategy
{
    #region Fields

    protected readonly InputScreenJoystick Joystick;
    protected readonly RectTransform SelfRectTransform;
    protected Vector2 startingPosition;

    public event Action<Vector2> OnSendValueToControl = delegate { };

    #endregion



    #region Constructor

    public InputScreenJoystickStrategy(InputScreenJoystick joystick)
    {
        if (joystick == null)
        {
            Debugger.LogError("Joystick base is null. 'InputScreenJoystick Component'");
            return;
        }
        
        /* Caching */
        Joystick = joystick;
        SelfRectTransform = Joystick.RectTransformSelf;
    }

    #endregion



    #region Event Control Send

    protected void RaiseOnSendValueToControl(Vector2 value) => OnSendValueToControl.Invoke(value);

    #endregion



    #region Abstract & Virtual

    public abstract void PointerDownInteraction(PointerEventData eventData);
    public abstract void PointerUpInteraction(PointerEventData eventData);
    public abstract void PointerDragInteraction(PointerEventData eventData);

    public virtual void SetJoystickMode(InputScreenJoystick.JoystickType joystickType, Action onSetSelfRectTransformMethod)
    {
        Joystick.ControlJoystickType = joystickType;
        
        // Self Rect Transform Changed.
        onSetSelfRectTransformMethod.Invoke();
    }

    #endregion
}
