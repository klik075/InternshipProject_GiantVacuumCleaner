
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StateTransitionTable", menuName = "State Machine/State Transition Table")]
public class StateTransitionTableSO : ScriptableObject
{
    #region Struct & Enum

    /* STRUCT */
    [Serializable]
    public struct ConditionUsage
    {
        public Result ExpectedResult;
        public Operator Operator;
        public StateConditionSO ConditionSO;
    }
    
    [Serializable]
    public struct TransitionItem
    {
        public StateSO FromState;
        public StateSO ToState;
        public ConditionUsage[] Conditions;
    }
    
    /* ENUMS */
    public enum Result { True, False }

    public enum Operator { And, Or }

    #endregion



    #region Fields

    [SerializeField] private TransitionItem[] _transitions;

    #endregion



    #region Getter

    public State GetInitialState(StateMachine stateMachine)
    {
        var states = new List<State>();
        var transitions = new List<StateTransition>();
        var createdInstance = new Dictionary<ScriptableObject, object>();
        var fromStates = _transitions.GroupBy(transitions => transitions.FromState);

        foreach (var fromState in fromStates)
        {
            if (fromState.Key == null)
            {
                throw new ArgumentNullException(nameof(fromState.Key), $"TransitionTable : {name}");
            }

            State state = fromState.Key.GetState(stateMachine, createdInstance);
            states.Add(state);
            transitions.Clear();

            foreach (var transitionItem in fromState)
            {
                if (transitionItem.ToState == null)
                {
                    throw new ArgumentNullException(nameof(transitionItem.ToState),
                        $"TransitionTable: {name}, From State: {fromState.Key.name}");
                }

                var toState = transitionItem.ToState.GetState(stateMachine, createdInstance);
                ProcessConditionUsage(stateMachine, transitionItem.Conditions, createdInstance, out var conditions, out var resultGroup);
                transitions.Add(new StateTransition(toState, conditions, resultGroup));
            }

            state.StateTransitions = transitions.ToArray();
        }

        return (states.Count > 0)
            ? states[0]
            : throw new InvalidOperationException($"TransitionTable {name} is empty.");
    }

    #endregion



    #region Utility

    private static void ProcessConditionUsage(
        StateMachine stateMachine, ConditionUsage[] conditionUsages,
        Dictionary<ScriptableObject, object> createdInstance,
        out StateConditionST[] conditions,
        out int[] resultGroup)
    {
        int count = conditionUsages.Length;
        conditions = new StateConditionST[count];

        for (int i = 0; i < count; ++i)
        {
            conditions[i] = conditionUsages[i].ConditionSO.GetCondition(
                stateMachine, conditionUsages[i].ExpectedResult == Result.True, createdInstance);
        }

        List<int> resultGroupList = new();
        for (int i = 0; i < count; ++i)
        {
            int idx = resultGroupList.Count;
            resultGroupList.Add(1);
            while (i < count - 1 && conditionUsages[i].Operator == Operator.And)
            {
                ++i;
                ++resultGroupList[idx];
            }
        }

        resultGroup = resultGroupList.ToArray();
    }

    #endregion
}
