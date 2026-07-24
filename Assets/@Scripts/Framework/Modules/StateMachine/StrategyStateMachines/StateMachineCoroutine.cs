
using System.Collections;
using UnityEngine;

public class StateMachineCoroutine : IStateMachine
{
    #region Fields

    private StateMachine _stateMachine;
    private Coroutine _stateCoroutine;

    #endregion

    #region Implements

    // Update is called once per frame
    public void Start(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _stateCoroutine = _stateMachine.StartCoroutine(StateUpdateCoroutine());
    }

    public void Stop()
    {
        if (_stateCoroutine != null)
        {
            _stateMachine.StopCoroutine(_stateCoroutine);
            _stateCoroutine = null;
        }
    }

    // 코루틴에서는 업데이트 구문이 필요 없음
    public void Update() { }
    public void FixedUpdate() { }

    #endregion

    #region Coroutine Update

    private IEnumerator StateUpdateCoroutine()
    {
        var waitForSeconds = new WaitForSeconds(_stateMachine.UpdateInterval);
        
        while (true)
        {
            if (_stateMachine.CurrentState.TryGetTransition(out var transitionState))
            {
                _stateMachine.TransitionStateAccessor(transitionState);
            }

            if (!_stateMachine.IsUpdateCustomTime)
            {
                _stateMachine.CurrentState.OnStateUpdate();
                yield return null;
            }
            else
            {
                _stateMachine.CurrentState.OnStateUpdate();
                yield return waitForSeconds;
            }
        }
    }

    #endregion
}
