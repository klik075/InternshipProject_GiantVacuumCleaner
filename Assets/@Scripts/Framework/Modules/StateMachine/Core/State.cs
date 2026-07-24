
using System.Collections.Generic;

public class State : IStateComponent, IStateUpdater
{
    #region Fields

    // 상태 머신
    public StateMachine StateMachine;
    
    // 트랜지션 및 상태 행동
    public StateTransition[] StateTransitions;
    public StateAction[] StateActions;
    
    // 상태에 대한 SO
    public StateSO StateOriginSO;

    #endregion



    #region Implement State

    public void OnStateEnter()
    {
        ProcessStateEnter(StateTransitions);
        ProcessStateEnter(StateActions);
    }

    public void OnStateExit()
    {
        ProcessStateExit(StateTransitions);
        ProcessStateExit(StateActions);
    }

    public void OnStateUpdate()
    {
        ProcessStateUpdate();
    }

    public void OnStateFixedUpdate()
    {
        ProcessStateFixedUpdate();
    }

    #endregion



    #region Process State

    private void ProcessStateEnter(IEnumerable<IStateComponent> stateComponents)
    {
        foreach (var stateComponent in stateComponents)
        {
            stateComponent.OnStateEnter();
        }
    }
    
    private void ProcessStateExit(IEnumerable<IStateComponent> stateComponents)
    {
        foreach (var stateComponent in stateComponents)
        {
            stateComponent.OnStateExit();
        }
    }
    
    private void ProcessStateUpdate()
    {
        foreach (var stateAction in StateActions)
        {
            stateAction.OnStateUpdate();
        }
    }
    
    private void ProcessStateFixedUpdate()
    {
        foreach (var stateAction in StateActions)
        {
            stateAction.OnStateFixedUpdate();
        }
    }

    #endregion



    #region Utils

    public bool TryGetTransition(out State state)
    {
        state = null;

        foreach (var transition in StateTransitions)
        {
            if (transition.TryGetTransition(out state)) break;
        }

        foreach (var transition in StateTransitions)
        {
            transition.ClearConditionsCache();
        }

        return state != null;
    }

    #endregion
}
