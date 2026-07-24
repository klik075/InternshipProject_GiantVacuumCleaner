
using System;

public static class StateMachineStrategyFactory
{
    #region Factory

    public static IStateMachine CreateStateMachineUpdater(StateMachine.StateUpdateMode stateUpdateMode)
    {
        return stateUpdateMode switch
        {
            StateMachine.StateUpdateMode.Update => new StateMachineUpdate(),
            StateMachine.StateUpdateMode.Coroutine => new StateMachineCoroutine(),
            StateMachine.StateUpdateMode.UniTask => new StateMachineUniTask(),
            StateMachine.StateUpdateMode.Custom => new StateMachineCustom(),
            _ => throw new ArgumentOutOfRangeException(nameof(stateUpdateMode), stateUpdateMode, null)
        };
    }

    #endregion
}
