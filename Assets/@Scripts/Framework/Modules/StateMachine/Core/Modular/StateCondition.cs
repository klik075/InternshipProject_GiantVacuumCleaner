
public abstract class StateCondition : IStateComponent, IStateInitializer
{
    #region Fields

    private bool _isCached; // 조건 캐싱 여부
    private bool _isCachedStatement; // 캐시된 조건의 결과 값
    
    // 상태 전환을 수정 및 반환 받을 수 있는 프로퍼티
    public StateConditionSO ConditionOriginSO { get; set; }

    #endregion


    
    #region Process Caching

    /// <summary>
    /// 상태 조건을 반환하는 메서드, 서브 클래스에서 구현
    /// </summary>
    protected abstract bool Statement();

    /// <summary>
    /// 조건의 결과 값 반환.
    /// 캐시된 결과를 반환
    /// </summary>
    /// <returns></returns>
    public bool GetStatement()
    {
        if (!_isCached)
        {
            _isCached = true;
            _isCachedStatement = Statement();
        }

        return _isCachedStatement;
    }

    public void ClearStatementCache()
    {
        _isCached = false;
    }

    #endregion



    #region Override

    public virtual void OnStateEnter() { }

    public virtual void OnStateExit() { }

    public virtual void Initialize(StateMachine stateMachine) { }

    #endregion
}
