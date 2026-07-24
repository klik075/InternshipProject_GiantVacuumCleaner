
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public abstract class InputScreenBase : OnScreenControl, IPointerDownHandler, IPointerUpHandler
{
    #region Fields

    // Binding Override Path
    protected virtual string ControlPath { get; set; }

    // Input Control Action을 재정의
    // 상속 받는 서브 클래스에서 ControlPath를 오버라이드
    protected override string controlPathInternal
    {
        get => ControlPath;
        set => ControlPath = value;
    }
    
    /* Input Base */
    public enum InputScreenState { Active, Inactive, Pressed }

    [NonSerialized] public Canvas RootCanvas;
    [NonSerialized] public Camera TargetCamera;

    /* Parameters */
    public RectTransform RootRectTransform { get; private set; }
    [NonSerialized] public RectTransform RectTransformSelf;
    [NonSerialized] public InputScreenState InputState;
    [NonSerialized] public float SelfRadius;
    [NonSerialized] public bool IsActive = true;
    
    // Fingers Info
    [NonSerialized] public bool IsFingerDown;
    [NonSerialized] public int FingerUniqueId = InputConfig.FINGER_ID_BASE;

    // 처음 터치 했을 때에 초기 지점
    [NonSerialized] public Vector2 InitialFingerPosition;
    [NonSerialized] public Vector2 FingerPosition;
    
    #endregion



    #region Abstract & Virtual

    protected abstract void Initialize();
    protected abstract void PointerDownInteraction(PointerEventData eventData);
    protected abstract void PointerUpInteraction(PointerEventData eventData);

    protected virtual void SubscribeEvents() { }
    protected virtual void DisposeEvents() { }
    
    #endregion



    #region Unity Behavior

    private void Awake()
    {
        /* Setup */
        InitializeInternal();
    }

    private void InitializeInternal()
    {
        RootRectTransform = GetComponent<RectTransform>();
        RectTransformSelf = RootRectTransform;
        RootCanvas = GetComponentInParent<Canvas>();

        if (RootCanvas == null)
        {
            Debugger.LogError("Canvas is not placed. Can't GetComponent.");
            return;
        }
        
        Initialize();
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        DisposeEvents();
    }

    #endregion



    #region Implements Interface

    /// <summary>
    /// 해당하는 Rect Transform을 눌렀을 때 반응
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        // 버튼이 비 활성화일 경우 작동하지 않음
        if (InputState != InputScreenState.Active) return;

        // RectTransformSelf 내부에 대한 터치가 아닐 경우 동작하지 않음
        if (!RectTransformUtility.RectangleContainsScreenPoint(
                RectTransformSelf, eventData.position, TargetCamera)) return;
        
        // 인터랙션 시작
        BeginInteraction(eventData);
        PointerDownInteraction(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 버튼이 눌린 상태가 아닐 경우
        if (InputState != InputScreenState.Pressed) return;

        EndInteraction(eventData);
        PointerUpInteraction(eventData);
    }

    #endregion



    #region Interaction

    private void BeginInteraction(PointerEventData eventData)
    {
        // 초기 터치 시 UI 카메라 존재 여부에 따라 셋팅
        SetupTargetUICamera();
        
        // Finger Setup
        SetupFingerInfoEnter(eventData);

        // 해당 버튼이 눌러졌다 표시
        InputState = InputScreenState.Pressed;
    }

    private void EndInteraction(PointerEventData eventData)
    {
        SetupFingerInfoExit(eventData);
        UpdateInputScreenState();
    }

    #endregion



    #region Interaction Setting Methods

    private void SetupTargetUICamera()
    {
        TargetCamera = RootCanvas.renderMode switch
        {
            RenderMode.ScreenSpaceOverlay => null,
            RenderMode.ScreenSpaceCamera => RootCanvas.worldCamera,
            _ => TargetCamera
        };
    }

    private void SetupFingerInfoEnter(PointerEventData eventData)
    {
        IsFingerDown = true;
        FingerUniqueId = eventData.pointerId;
        InitialFingerPosition = eventData.position;
        FingerPosition = InitialFingerPosition;
    }

    private void SetupFingerInfoExit(PointerEventData eventData)
    {
        IsFingerDown = false;
        FingerUniqueId = InputConfig.FINGER_ID_BASE;
        InitialFingerPosition = Vector2.zero;
        FingerPosition = InitialFingerPosition;
    }

    #endregion



    #region Utils

    private void UpdateInputScreenState()
    {
        InputState = (IsActive) ? InputScreenState.Active : InputScreenState.Inactive;
    }

    protected virtual void UpdateRectBoundary()
    {
        SelfRadius = RectTransformSelf.rect.width / InputConfig.Input_Radius * RootCanvas.scaleFactor;
    }

    #endregion



    #region Public Utils (Accessor)

    public void SetActiveInputScreenState(bool isActive)
    {
        IsActive = isActive;
        UpdateInputScreenState();
    }

    /// <summary>
    /// 앵커 포지션을 구하는 메서드 (기본값 : Rect Transform Self를 기준
    /// </summary>
    /// <param name="screenPosition">화면을 터치한 좌표를 보내야됌 (ex. eventData.position)</param>
    /// <returns>RectTransformSelf 기준의 앵커 포지션 반환 아닐 경우 Zero 반환</returns>
    public Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            RectTransformSelf, screenPosition, TargetCamera, out var localPoisition)
            ? localPoisition
            : Vector2.zero;
    }

    /// <summary>
    /// 앵커 포지션을 구한느 메서드 (매개변수로 Root를 전달 : 기준 값이 RootRect가 됌)
    /// </summary>
    public Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition, RectTransform rootRect)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootRect, screenPosition, TargetCamera, out var localPosition) 
            ? localPosition 
            : Vector2.zero;
    }

    #endregion
}
