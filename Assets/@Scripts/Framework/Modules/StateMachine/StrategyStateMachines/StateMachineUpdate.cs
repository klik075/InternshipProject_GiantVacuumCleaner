
public class StateMachineUpdate : IStateMachine
{
    #region Field

    private StateMachine _stateMachine;

    #endregion

    #region Implements

    public void Start(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Stop() { } // 업데이트를 통한 스테이트 머신에서는 필요 없음.

    public void Update()
    {
        if (_stateMachine.CurrentState.TryGetTransition(out var transitionState))
        {
            _stateMachine.TransitionStateAccessor(transitionState);
        }
        _stateMachine.CurrentState.OnStateUpdate();
    }

    public void FixedUpdate()
    {
        _stateMachine.CurrentState.OnStateFixedUpdate();
    }

    #endregion
}
