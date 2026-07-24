
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    #region Fields

    public enum StateUpdateMode { Update, Coroutine, UniTask, Custom }

    [SerializeField] private StateUpdateMode _stateUpdateMode;
    [SerializeField] protected StateTransitionTableSO _stateTransitionTable;

    protected readonly Dictionary<Type, Component> _cachedComponents = new();
    protected State _currentState;
    
    [ConditionalField(nameof(_stateUpdateMode), (int)StateUpdateMode.Coroutine, (int)StateUpdateMode.UniTask)]
    [SerializeField] private bool _isUpdateCustomTime;
    [ConditionalField(new[] { nameof(_stateUpdateMode), nameof(_isUpdateCustomTime) }, (int)StateUpdateMode.Coroutine, (int)StateUpdateMode.UniTask)]
    [SerializeField] private float _updateInterval = 0.1f;

    private IStateMachine _stateMachineUpdater;
    private bool _isStarting;

    public event Action OnStateChanged = delegate { };

#if UNITY_EDITOR
    [Space] [SerializeField] public StateMachineDebugger Debugger;
#endif

    #endregion



    #region Properties
    
    public float UpdateInterval => _updateInterval;
    public bool IsUpdateCustomTime => _isUpdateCustomTime;
    public State CurrentState => _currentState;
    public virtual StateUpdateMode UpdateMode
    {
        get => _stateUpdateMode;
        set => _stateUpdateMode = value;
    }

    #endregion
    
    
    
    #region Unity Behavior

    private void Awake()
    {
        _currentState = _stateTransitionTable.GetInitialState(this);
#if UNITY_EDITOR
        Debugger.Initialize(this);
#endif
        SetStateMachineUpdater();
    }

    private void Start()
    {
        _currentState.OnStateEnter();
        _stateMachineUpdater.Start(this);
        _isStarting = true;
    }

    private void Update()
    {
        _stateMachineUpdater.Update();
    }

    private void FixedUpdate()
    {
        _stateMachineUpdater.FixedUpdate();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        UnityEditor.AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
#endif
        if (!_isStarting) return;
        _currentState = _stateTransitionTable.GetInitialState(this);
        _currentState.OnStateEnter();
        _stateMachineUpdater.Start(this);
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        UnityEditor.AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
#endif
        _currentState.OnStateExit();
        _stateMachineUpdater.Stop();
    }

    #endregion

    
    
    #region Transition State

    private void TransitionState(State transitionState)
    {
        OnStateChanged.Invoke();
        _currentState.OnStateExit();
        _currentState = transitionState;
        _currentState.OnStateEnter();
    }

    public void TransitionStateAccessor(State transitionState) => TransitionState(transitionState);

    #endregion

    
    
    #region Utils (Try Get Add Component)

    public new bool TryGetComponent<T>(out T component) where T : Component
    {
        var typeT = typeof(T);

        if (!_cachedComponents.TryGetValue(typeT, out var value))
        {
            if (base.TryGetComponent(out component))
            {
                _cachedComponents.Add(typeT, component);
            }

            return component != null;
        }

        component = value as T;
        return true;
    }

    public new T GetComponent<T>() where T : Component
    {
        return TryGetComponent(out T component)
            ? component
            : throw new InvalidOperationException($"{typeof(T).Name} not found in {name}");
    }

    public T GetStateAction<T>() where T : StateAction
        => CurrentState?.StateActions.OfType<T>().FirstOrDefault();

#if UNITY_EDITOR
    private void OnAfterAssemblyReload()
    {
        _currentState = _stateTransitionTable.GetInitialState(this);
        Debugger.Initialize(this);
    }
#endif

    private void SetStateMachineUpdater()
    {
        _stateMachineUpdater = StateMachineStrategyFactory.CreateStateMachineUpdater(UpdateMode);
    }

    #endregion

    
    
    #region Utils

    public bool IsCurrentState(string stateName)
        => _currentState.StateOriginSO.name == stateName;

    public bool IsCurrentState(StateSO state)
        => _currentState.StateOriginSO.Equals(state);

    #endregion
}
