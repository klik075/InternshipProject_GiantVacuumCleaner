using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateConditionSO : DescriptionSO
{
    #region Get Condition

    public StateConditionST GetCondition(StateMachine stateMachine, bool expectedResult,
        Dictionary<ScriptableObject, object> createdInstance)
    {
        if (!createdInstance.TryGetValue(this, out var obj))
        {
            var stateCondition = CreateStateCondition();
            stateCondition.ConditionOriginSO = this;
            stateCondition.Initialize(stateMachine);
            obj = stateCondition;
        }

        return new StateConditionST(stateMachine, obj as StateCondition, expectedResult);
    }

    /// <summary>
    /// 상태 전환을 만들어서 반환, 서브 클래스에서 구현
    /// </summary>
    protected abstract StateCondition CreateStateCondition();

    #endregion
}

public abstract class StateConditionSO<T> : StateConditionSO where T : StateCondition, new()
{
    protected override StateCondition CreateStateCondition() => new T();
}