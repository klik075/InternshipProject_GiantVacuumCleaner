using UnityEngine;

public class ConditionalFieldAttribute : PropertyAttribute
{
    public string[] ConditionFields { get; private set; }
    public int[] ConditionValues { get; private set; }

    public ConditionalFieldAttribute(string conditionField, params int[] conditionValues)
    {
        ConditionFields = new[] { conditionField };
        ConditionValues = conditionValues;
    }

    public ConditionalFieldAttribute(string[] conditionFields, params int[] conditionValues)
    {
        ConditionFields = conditionFields;
        ConditionValues = conditionValues;
    }
}