
using Scripts.Framework.Events.Listeners;
using UnityEngine;

public class LoadingInterfaceController : MonoBehaviour
{
    #region Fields

    [Header("Init Loading Cover")] 
    [SerializeField] private GameObject _loadingInit;
    
    [Header("Custom Loading Cover")] 
    [SerializeField] private GameObject _loadingInterface;
    
    [Header("Listening on")]
    [SerializeField] private GameEventListener<bool> _toggleLoadingInitEvent;
    [SerializeField] private GameEventListener<bool> _toggleLoadingCustomEvent;

    #endregion



    #region Unity Behavior

    private void OnEnable()
    {
        _toggleLoadingInitEvent.SubstitutionEvent(ToggleLoadingScreenToInit);
        _toggleLoadingCustomEvent.SubstitutionEvent(ToggleLoadingScreenToCustom);
        
        _toggleLoadingInitEvent.Subscribe();
        _toggleLoadingCustomEvent.Subscribe();
    }

    private void OnDisable()
    {
        _toggleLoadingInitEvent.Unsubscribe();
        _toggleLoadingCustomEvent.Unsubscribe();
    }

    #endregion



    #region Toggle

    private void ToggleLoadingScreenToInit(bool activateValue)
        => _loadingInit.SetActive(activateValue);
    
    private void ToggleLoadingScreenToCustom(bool activateValue)
        => _loadingInterface.SetActive(activateValue);

    #endregion
}