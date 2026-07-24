
public class StateTransition : IStateComponent
{
    #region Fields

    private State _targetState;
    private StateConditionST[] _stateConditionResults;
    private int[] _resultGroups;
    private bool[] _results;

    #endregion



    #region Constructor

    public StateTransition(State targetState, StateConditionST[] stateConditionResults, int[] resultGroups)
    {
        _targetState = targetState;
        _stateConditionResults = stateConditionResults;
        _resultGroups = resultGroups is { Length: > 0 }
            ? resultGroups
            : new int[1];
        _results = new bool[_resultGroups.Length];
    }

    #endregion



    #region Transition

    public bool TryGetTransition(out State state)
    {
        state = ShouldTransition() ? _targetState : null;

        return state != null;
    }

    private bool ShouldTransition()
    {
#if UNITY_EDITOR
        _targetState.StateMachine.Debugger.TransitionEvaluationBegin(_targetState.StateOriginSO.name);
#endif
        
        int count = _resultGroups.Length;

        for (int i = 0, idx = 0; i < count && idx < _stateConditionResults.Length; ++i)
        {
            for (int j = 0; j < _resultGroups[i]; ++j, ++idx)
            {
                _results[i] = (j == 0)
                    ? _stateConditionResults[idx].IsMet()
                    : _results[i] && _stateConditionResults[idx].IsMet();
            }
        }

        bool result = false;
        for (int i = 0; i < count && !result; ++i)
        {
            result = _results[i];
        }

#if UNITY_EDITOR
        _targetState.StateMachine.Debugger.TransitionEvaluationEnd(result, _targetState.StateActions);
#endif
        return result;
    }

    public void ClearConditionsCache()
    {
        for (int i = 0; i < _stateConditionResults.Length; ++i)
        {
            _stateConditionResults[i].StateCondition.ClearStatementCache();
        }
    }

    #endregion



    #region Implement State Transition

    public void OnStateEnter()
    {
        for (int idx = 0; idx < _stateConditionResults.Length; ++idx)
        {
            _stateConditionResults[idx].StateCondition.OnStateEnter();
        }
    }

    public void OnStateExit()
    {
        for (int idx = 0; idx < _stateConditionResults.Length; ++idx)
        {
            _stateConditionResults[idx].StateCondition.OnStateExit();
        }
    }

    #endregion
}
