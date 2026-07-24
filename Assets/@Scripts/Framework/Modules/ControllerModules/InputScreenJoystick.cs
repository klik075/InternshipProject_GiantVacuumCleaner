
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

public class InputScreenJoystick : InputScreenBase, IDragHandler
{
    #region Fields
    
    /* Joystick Base */
    public enum JoystickType { Fixed, Floating }
    public enum JoystickHandlerType { OnlyHandle, HandleWithBg, DirectionWithBg, HandleBgDirection}
    public enum JoystickModularName { Background, Direction, Handle }

    /* Override */
    [InputControl(layout = "Vector2")]
    [SerializeField] private string _serializedControlPath;
    

    protected override string ControlPath
    {
        get => _serializedControlPath;
        set => _serializedControlPath = value;
    }
    
    // JoystickType -> 플레이어가 설정 가능한 조이스틱 타입
    // JoystickHandlerType -> 자동으로 결정 됌
    public JoystickType ControlJoystickType = JoystickType.Floating;
    [NonSerialized] public JoystickHandlerType ControlJoystickHandlerType = JoystickHandlerType.HandleWithBg;
    
    // 1. 해당 모듈라 이름으로 지정된 오브젝트가 존재하는지 판단.
    // 2. 존재 하면 해당 모듈라 이름에 맞는 RectTransform을 딕셔너리에 삽입
    //   - 최대 3개를 지닐 수 있다. (Enum 개수만큼)
    //   - 1 ~ 3개에 따라 자동으로 JoystickHandleType을 결정하는 용도
    [NonSerialized] public Dictionary<JoystickModularName, RectTransform> RectTransforms;
    
    /* Anchored Finger Info */
    [NonSerialized] public Vector2 AnchoredInitialFingerPosition;
    [NonSerialized] public Vector2 AnchoredFingerPosition;
    
    // Strategy Pattern Joystick
    private InputScreenJoystickStrategy _joystickStrategy;
    // 디렉션 사용 여부 (자동으로 인식 됌, 건들 필요 없음)
    private bool _isUseDirection;

    #endregion
    
    #region Setup Joystick

    private void JoystickInitialize()
    {
        // 조이스틱 아래에 존재하는 자식들을 가져옴 자동화를 위함.
        SetupChildRectTransforms();
        SetupJoystickHandlerType();
        SetupJoystickRectTransformSelf();
    }

    private void SetupChildRectTransforms()
    {
        RectTransforms ??= new Dictionary<JoystickModularName, RectTransform>();

        foreach (Transform child in RectTransformSelf)
        {
            if (Enum.TryParse(child.gameObject.name, out JoystickModularName modularName))
            {
                RectTransforms[modularName] = child.GetComponent<RectTransform>();
            }
        }

        if (RectTransformSelf.childCount > 3)
        {
            Debugger.LogWarning(
                "조이스틱이 3개이상의 자식을 두고 있습니다. 오로지 'Background', 'Direction', 'Handle'만 가능합니다.");
        }
    }

    private void SetupJoystickHandlerType()
    {
        bool hasHandle = RectTransforms.ContainsKey(JoystickModularName.Handle);
        bool hasBackground = RectTransforms.ContainsKey(JoystickModularName.Background);
        bool hasDirection = RectTransforms.ContainsKey(JoystickModularName.Direction);
        
        // Boolean Has 조합을 통해 HandleType을 결정
        ControlJoystickHandlerType = DetermineJoystickType(hasHandle, hasBackground, hasDirection);

        SetupIsUseDirection(ControlJoystickHandlerType 
            is JoystickHandlerType.DirectionWithBg
            or JoystickHandlerType.HandleBgDirection);
    }
    
    private void SetupIsUseDirection(bool isUsed)
    {
        _isUseDirection = isUsed;
    }

    private void SetupJoystickRectTransformSelf()
    {
        RectTransformSelf = ControlJoystickType switch
        {
            JoystickType.Fixed => ControlJoystickHandlerType switch
            {
                JoystickHandlerType.OnlyHandle => RectTransforms[JoystickModularName.Handle],
                _ => RectTransforms[JoystickModularName.Background]
            },
            JoystickType.Floating => RootRectTransform,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    #endregion

    

    #region Override
    
    protected override void Initialize()
    {
        /* Joystick Setup */
        JoystickInitialize();
        
        /* Create Joystick Strategy */
        CreateJoystick();
        
        /* Setting Joystick Type : Fixed or Floating */
        SetJoystickType(ControlJoystickType);
    }

    protected override void SubscribeEvents()
    {
        _joystickStrategy.OnSendValueToControl += SendValueToControl;
    }

    protected override void DisposeEvents()
    {
        _joystickStrategy.OnSendValueToControl -= SendValueToControl;
    }
    
    protected override void PointerDownInteraction(PointerEventData eventData)
    {
        BeginInputProcess(eventData);
        _joystickStrategy.PointerDownInteraction(eventData);
    }

    protected override void PointerUpInteraction(PointerEventData eventData)
    {
        _joystickStrategy.PointerUpInteraction(eventData);
    }

    #endregion



    #region Initialize

    private void CreateJoystick()
    {
        _joystickStrategy = JoystickStrategyFactory.CreateStrategyJoystick(ControlJoystickHandlerType, this);
    }

    #endregion



    #region Implements Drag

    public void OnDrag(PointerEventData eventData)
    {
        if (InputState != InputScreenState.Pressed || eventData.pointerId != FingerUniqueId) return;

        DragInputProcess(eventData);
        _joystickStrategy.PointerDragInteraction(eventData);
    }

    #endregion



    #region Process Touch Input

    private void BeginInputProcess(PointerEventData eventData)
    {
        AnchoredInitialFingerPosition = ScreenPointToAnchoredPosition(eventData.position);
        AnchoredFingerPosition = AnchoredInitialFingerPosition;
    }
    
    private void DragInputProcess(PointerEventData eventData)
    {
        if (RootCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            TargetCamera = RootCanvas.worldCamera;
        }

        FingerPosition.x = eventData.position.x;
        FingerPosition.y = eventData.position.y;

        // Touch Zone (Rect Transform Self)에 맞는 Anchored Position => AnchoredFingerPosition 변환
        AnchoredFingerPosition = ScreenPointToAnchoredPosition(FingerPosition);
    }

    #endregion



    #region Utils
    
    /// <summary>
    /// # Joystick Handle Type을 결정해서 반환해주는 메서드
    /// </summary>
    /// <param name="hasHandle">핸들이 존재하는지</param>
    /// <param name="hasBackground">백그라운드가 존재하는지</param>
    /// <param name="hasDirection">디렉션이 존재하는지</param>
    private JoystickHandlerType DetermineJoystickType(bool hasHandle, bool hasBackground, bool hasDirection)
    {
        // Tuple로 결합하여 사용
        return (hasHandle, hasBackground, hasDirection) switch
        {
            (true, true, true) => JoystickHandlerType.HandleBgDirection,
            (true, true, false) => JoystickHandlerType.HandleWithBg,
            (false, true, true) => JoystickHandlerType.DirectionWithBg,
            (true, false, false) => JoystickHandlerType.OnlyHandle,
            _ => throw new InvalidOperationException("Invalid or Missing Joystick Components.")
        };
    }

    public void SetJoystickType(JoystickType joystickType)
    {
        _joystickStrategy.SetJoystickMode(joystickType, SetupJoystickRectTransformSelf);
    }

    #endregion
}
