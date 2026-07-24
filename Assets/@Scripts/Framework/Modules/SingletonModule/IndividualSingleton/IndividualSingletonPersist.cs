using UnityEngine;

public abstract class IndividualSingletonPersist<T> : IndividualSingleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}