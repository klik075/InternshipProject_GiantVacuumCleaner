
using System;
using Cysharp.Threading.Tasks;
using Scripts.Framework.Events.SO;
using Scripts.Framework.Managers.Asset.Core;
using Scripts.Framework.Scenes.SO;
using Unity.VisualScripting;
using UnityEngine;

public class SequenceSystem : MonoBehaviour
{
    #region Fields

    [Header("Initialize Sequence Delay")]
    [SerializeField] [Range(0.2f, 1f)] private float _delaySeconds = 0.2f;

    [Header("Game Initialize sequence Use loading Cover")] 
    [SerializeField] private bool _showInitializeLoadingCover = true;

    [Header("Listening to")] 
    [SerializeField] private RequestSequenceEventChannelSO _requestSequenceInitialize;
    
    [Header("Broadcasting on")]
    [SerializeField] private LoadSceneEventChannelSO _loadSceneEventChannel;
    [SerializeField] private GameEventSO<bool> _toggleLoadingInitializeEvent;
    
    // Broadcasting on (None So)
    // SO를 사용하기 어려우면 아래 이벤트를 사용하세요.
    public static event Action OnCompleteInitializeSequence = delegate { };

    #endregion



    #region Event Subscribe

    private void OnEnable()
    {
        _requestSequenceInitialize.OnSequenceRequested += InitializeSequenceRequest;
    }

    private void OnDisable()
    {
        _requestSequenceInitialize.OnSequenceRequested -= InitializeSequenceRequest;
    }

    #endregion



    #region Request Loaded

    private async UniTask InitializeSequenceRequest(GameSceneSO sceneToLoad, bool showLoadScreen, Action<int, int> action = null, bool fadeScreen = false)
    {
        ToggleLoadingCover(true);
        int milliSeconds = (int)_delaySeconds * 1000;
        
        await UniTask.Delay(milliSeconds);

        await InitializeSequenceInternalProcess(sceneToLoad, false, action, fadeScreen);
        
        // Callback
        OnCompleteInitializeSequence.Invoke();
        ToggleLoadingCover(false);
    }

    /// <summary>
    /// Sequence Processing To Methods
    /// </summary>
    private async UniTask InitializeSequenceInternalProcess(GameSceneSO sceneToLoad, bool showLoadScreen , Action<int, int> action = null, bool fadeScreen = false)
    {
        
        // 1. Addressable Initialize
        await AMS.InitializeAssets(action);
        
        // 2. SDK Initialize
        await SDKIntegrationSystem.Instance.Initialize();
        
        // 3. Scene Loaded Async (This context last.)
        await _loadSceneEventChannel.RaiseEvent(sceneToLoad, showLoadScreen, action, fadeScreen);
    }

    #endregion



    #region Utils

    private void ToggleLoadingCover(bool onEnable)
    {
        if (_showInitializeLoadingCover)
        {
            _toggleLoadingInitializeEvent.RaiseEvent(onEnable);
        }
    }

    #endregion
}
