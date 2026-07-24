
public abstract class StateAction : IStateComponent, IStateUpdater, IStateInitializer
{
    #region Fields

    public enum SpecificMoment { OnEnter, OnExit, OnUpdate, OnFixedUpdate }

    // 실제 액션을 수정 및 반환 받을 수 있는 프로퍼티
    public StateActionSO ActionOriginSO { get; set; }

    #endregion
    
    #region Override

    public virtual void OnStateEnter() { }

    public virtual void OnStateExit() { }

    public virtual void OnStateFixedUpdate() { }
    
    public virtual void OnStateUpdate() { }

    /// <summary>
    /// Initializer
    ///   - 새 인스턴스 생성시 호출
    ///   - 생성자와 같은 역할
    /// </summary>
    public virtual void Initialize(StateMachine stateMachine) { }

    #endregion
}
