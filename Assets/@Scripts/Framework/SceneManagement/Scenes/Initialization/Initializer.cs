
using Cysharp.Threading.Tasks;
using Scripts.Framework.Scenes.SO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private GameSceneSO _persistentSystem;

    /// <summary>
    /// Initialize 'Persistent' and Move to the scene.
    /// ex) Lobby or MainGame you want.
    /// </summary>
    [Header("Move to the scene - Loading Scene")] 
    [SerializeField] private GameSceneSO _loadingModular;

    /// <summary>
    /// Loading Modular Scene load request channel SO
    /// </summary>
    [Header("Broadcasting on")] 
    [SerializeField] private AssetReference _loadSceneEventChannel;

    #endregion



    #region Initialization

    private async void Awake()
    {
        await InitializeSequence();
    }

    private async UniTask InitializeSequence()
    {
        await _persistentSystem.SceneReference.LoadSceneAsync(LoadSceneMode.Additive).ToUniTask();

        await LoadSceneEventChannel();
    }

    #endregion



    #region Event Channel

    private async UniTask LoadSceneEventChannel()
    {
        // await _loadingModular.SceneReference.LoadSceneAsync(LoadSceneMode.Additive).ToUniTask();
        var loadEventChannelOperation = _loadSceneEventChannel.LoadAssetAsync<LoadSceneEventChannelSO>().ToUniTask();
        var loadEventChannel = await loadEventChannelOperation;
        await loadEventChannel.RaiseEvent(_loadingModular, false);

        // Initialization is the only scene. BuildSettings, has index 0
        await SceneManager.UnloadSceneAsync(0);
    }

    #endregion
}
