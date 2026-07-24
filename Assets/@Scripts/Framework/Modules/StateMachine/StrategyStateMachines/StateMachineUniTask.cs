
using System.Threading;
using Cysharp.Threading.Tasks;

public class StateMachineUniTask : IStateMachine
{
    #region Fields

    private StateMachine _stateMachine;
    private CancellationTokenSource _cancellationTokenSource;
    // private bool _isRunning = false;

    #endregion

    #region Implements

    // Update is called once per frame
    public void Start(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _cancellationTokenSource = new CancellationTokenSource();
        
        // _isRunning = true;
        StateUpdateUniTask(_cancellationTokenSource.Token).Forget();
    }

    public void Stop()
    {
        // _isRunning = false;
        _cancellationTokenSource?.Cancel();
    }

    // UniTask에서는 업데이트 구문이 필요 없음
    public void Update() { }
    public void FixedUpdate() { }

    #endregion

    #region UniTask

    private async UniTaskVoid StateUpdateUniTask(CancellationToken cancellationToken)
    {
        var updateIntervalInteger = (int)(_stateMachine.UpdateInterval * 1000);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_stateMachine.CurrentState.TryGetTransition(out var transitionState))
            {
                _stateMachine.TransitionStateAccessor(transitionState);
            }

            if (!_stateMachine.IsUpdateCustomTime)
            {
                _stateMachine.CurrentState.OnStateUpdate();
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
            else
            {
                _stateMachine.CurrentState.OnStateUpdate();
                await UniTask.Delay(updateIntervalInteger, cancellationToken: cancellationToken);
            }
        }
    }

    #endregion
}
