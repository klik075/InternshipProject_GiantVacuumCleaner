
using System.Collections.Generic;
using UnityEngine;

public abstract class StateActionSO : DescriptionSO
{
    #region Get Action
    
    public StateAction GetStateAction(StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstance)
    {
        // 딕셔너리에 해당 스테이트 액션이 존재하면 그대로 반환
        if (createdInstance.TryGetValue(this, out var obj))
        {
            return obj as StateAction;
        }
        
        // 딕셔너리에 존재하지 않을 경우
        // 새로운 액션을 만들어서 딕셔너리 추가 후 반환
        var stateAction = CreateStateAction();
        createdInstance.Add(this, stateAction);
        
        stateAction.ActionOriginSO = this;
        stateAction.Initialize(stateMachine);

        return stateAction;
    }
    
    /// <summary>
    /// 상태 액션을 만들어서 반환, 서브 클래스에서 구현
    /// </summary>
    protected abstract StateAction CreateStateAction();

    #endregion
}

public abstract class StateActionSO<T> : StateActionSO where T : StateAction, new()
{
    protected override StateAction CreateStateAction() => new T();
}