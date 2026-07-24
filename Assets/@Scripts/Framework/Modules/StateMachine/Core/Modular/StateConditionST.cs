
/// <summary>
/// 상태 머신과 상태 조건, 예상 결과를 결합
/// 해당 조건을 충족하는지 확인하는 기능
/// </summary>
public readonly struct StateConditionST
{
    #region Fields

    public readonly StateMachine StateMachine;
    public readonly StateCondition StateCondition;
    public readonly bool ExpectedResult; // StateCondition에 대한 기대 결과 값

    #endregion



    #region Constructor & Utils

    public StateConditionST(StateMachine stateMachine, StateCondition stateCondition, bool expectedResult)
    {
        StateMachine = stateMachine;
        StateCondition = stateCondition;
        ExpectedResult = expectedResult;
    }

    /// <summary>
    /// 조건이 기대한 결과값과 일치하는지 여부 확인
    /// </summary>
    /// <returns>조건 충족시 TRUE , 아닐시 FALSE</returns>
    public bool IsMet()
    {
        bool statement = StateCondition.GetStatement();
        bool isMet = (statement == ExpectedResult);
        
        #if UNITY_EDITOR
        StateMachine.Debugger.TransitionConditionResult(StateCondition.ConditionOriginSO.name, statement, isMet);
        #endif

        return isMet;
    }

    #endregion
}
