
using Scripts.Framework.Modules.SO_Base;
using UnityEngine;

public abstract class DescriptionSO : SerializableScriptableObject
{
    [SerializeField, TextArea] private string _description;

    public string Description => _description;
}