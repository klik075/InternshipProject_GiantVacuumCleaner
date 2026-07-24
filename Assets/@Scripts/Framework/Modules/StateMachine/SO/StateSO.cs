
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "State Machine/State")]
public class StateSO : DescriptionSO
{
    #region Fields

    [SerializeField] private StateActionSO[] _stateActions;

    #endregion



    #region Getter

    public State GetState(StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstance)
    {
        if (createdInstance.TryGetValue(this, out var obj))
        {
            return obj as State;
        }

        var state = new State();
        createdInstance.Add(this, state);
        
        state.StateOriginSO = this;
        state.StateMachine = stateMachine;
        state.StateTransitions = Array.Empty<StateTransition>();
        state.StateActions = GetStateActions(_stateActions, stateMachine, createdInstance);

        return state;
    }

    private static StateAction[] GetStateActions(
        StateActionSO[] scriptableActions,
        StateMachine stateMachine,
        Dictionary<ScriptableObject, object> createdInstance)
    {
        int actionCount = scriptableActions.Length;
        var actions = new StateAction[actionCount];

        for (int idx = 0; idx < actionCount; ++idx)
        {
            actions[idx] = scriptableActions[idx].GetStateAction(stateMachine, createdInstance);
        }

        return actions;
    }

    #endregion
}
