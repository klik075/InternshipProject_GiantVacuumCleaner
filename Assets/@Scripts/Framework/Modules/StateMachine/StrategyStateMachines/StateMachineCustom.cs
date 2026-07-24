
public class StateMachineCustom : IStateMachine
{
    #region Field

    private StateMachine _stateMachine;

    #endregion
    
    #region Implements
    
    public void Start(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Stop() { }

    public void Update() { }

    public void FixedUpdate() { }

    #endregion
}
