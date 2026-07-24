
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class StateMachineDebugger
{
    #region Fields

    /* Dev Toggle */
    [Header("Debug Log 'Transition' Toggle")] [Tooltip("디버그 로그 토글, 상태 전환 표기")]
    [SerializeField] private bool _debugTransition;
    
    [Tooltip("전체 리스트 (컨디션), ConditionName == BooleanResult [PassedTest]")]
    [SerializeField] private bool _appendConditionsInfo;
    
    [Tooltip("전체 리스트 (액션), 활성화된 액션 by 새로운 상태")]
    [SerializeField] private bool _appendActionsInfo;
    
    [Tooltip("현재 상태(스테이트) 이름 [Readonly]")]
    [SerializeField] private string _currentStateName;

    /* Member */
    private StateMachine _stateMachine;
    private StringBuilder _logBuilder;
    private string _targetState = string.Empty;
    
    /* Const */
    private const string CHECK_MARK = "\u2714";
    private const string UNCHECK_MARK = "\u2718";
    private const string THICK_ARROW = "\u279C";
    private const string SHARP_ARROW = "\u27A4";
        
    #endregion



    #region Init

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _logBuilder = new StringBuilder();
        _currentStateName = _stateMachine.CurrentState.StateOriginSO.name;
    }

    #endregion
    
    

    #region Process (Transition & Conditions)

    public void TransitionEvaluationBegin(string targetState)
    {
        _targetState = targetState;

        if (!_debugTransition) return;

        _logBuilder.Clear();
        _logBuilder.AppendLine($"{_stateMachine.name} state changed");
        _logBuilder.AppendLine($"{_currentStateName}   {SHARP_ARROW}    {_targetState}");
        
        if (_appendConditionsInfo)
        {
            _logBuilder.AppendLine();
            _logBuilder.AppendLine("Transition Conditions");
        }
    }
    
    public void TransitionConditionResult(string conditionName, bool result, bool isMet)
    {
        if (!_debugTransition || _logBuilder.Length == 0 || !_appendConditionsInfo) return;

        _logBuilder.Append($"     {THICK_ARROW} {conditionName} == {result}");

        _logBuilder.AppendLine(isMet ? $"  [{CHECK_MARK}]" : $"  [{UNCHECK_MARK}]");
    }
    
    public void TransitionEvaluationEnd(bool passed, StateAction[] actions)
    {
        if (passed)
        {
            _currentStateName = _targetState;
        }

        if (!_debugTransition || _logBuilder.Length == (0)) return;

        if (passed)
        {
            LogActions(actions);
            PrintDebugLog();
        }

        _logBuilder.Clear();
    }

    #endregion
    
    
    
    #region Process (Actions)

    private void LogActions(IEnumerable<StateAction> actions)
    {
        if (!_appendActionsInfo)
            return;

        _logBuilder.AppendLine();
        _logBuilder.AppendLine("State Actions:");

        foreach (StateAction action in actions)
        {
            _logBuilder.AppendLine($"    {THICK_ARROW} {action.ActionOriginSO.name}");
        }
    }
    
    private void PrintDebugLog()
    {
        _logBuilder.AppendLine();
        _logBuilder.Append("--------------------------------");

        Debugger.Log(_logBuilder.ToString());
    }

    #endregion
}
