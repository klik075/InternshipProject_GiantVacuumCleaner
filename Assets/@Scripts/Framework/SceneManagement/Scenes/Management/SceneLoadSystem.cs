
using Cysharp.Threading.Tasks;
using Scripts.Framework.Events.SO;
using Scripts.Framework.Scenes.SO;
using System;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadSystem : MonoBehaviour
{
    #region Fields

    [Header("Gameplay Supporter")] 
    [SerializeField] private GameSceneSO _gameplaySupporter;

    [Header("Listening to")] 
    [SerializeField] private LoadSceneEventChannelSO _loadSplashScene;
    [SerializeField] private LoadSceneEventChannelSO _loadSceneRequest;

    [Header("Broadcasting on")] 
    [SerializeField] private GameEventSO<bool> _loadingInterfaceEvent;

    private SceneLoadHelper _helper = new();
    private SceneLoadState _state = new();

    #endregion

    
    
    #region Event Subscribe

    private void OnEnable()
    {
        _loadSplashScene.OnSceneLoadRequest += LoadSplashScene;
        _loadSceneRequest.OnSceneLoadRequest += LoadScene;
    }

    private void OnDisable()
    {
        _loadSplashScene.OnSceneLoadRequest -= LoadSplashScene;
        _loadSceneRequest.OnSceneLoadRequest -= LoadScene;
    }

    #endregion



    #region Scene Load

    private async UniTask LoadScene(GameSceneSO sceneToLoad, bool showLoadScreen,Action<int, int> action = null, bool fadeScreen = false)
    {
        if (_state.IsLoadingFlag) return;
        if (_state.ShowLoadingCover) _loadingInterfaceEvent.RaiseEvent(true);

        SetSceneState(sceneToLoad, showLoadScreen);
        
        await _helper.LoadAssetsFromScene(_state.SceneToLoad,action);
        await LoadGameplaySupporterScene(action);
        await LoadSceneSequence();
    }

    private async UniTask LoadGameplaySupporterScene(Action<int,int> action = null)
    {
        // Is Need Gameplay Supporter Scene
        if (_state.SceneToLoad.IsNeedGameplaySupporter)
        {
            if (!_state.GameplaySupporter.Scene.isLoaded)
            {
                await _helper.LoadAssetsFromScene(_gameplaySupporter, action);
                _state.GameplaySupporter = await _helper.LoadSceneAsync(_gameplaySupporter.SceneReference, LoadSceneMode.Additive);
            }
        }
        else
        {
            if (_state.GameplaySupporter.Scene.isLoaded)
            {
                _helper.UnloadAssetsFromPrevScene(_gameplaySupporter);
                await _helper.UnloadSceneAsync(_state.GameplaySupporter);
            }
        }
    }

    #endregion



    #region Scene Load (Modular - Splash Scene)

    private async UniTask LoadSplashScene(GameSceneSO splashScene, bool showLoadScreen,Action<int, int> action = null, bool fadeScreen = false)
    {
        if (_state.IsLoadingFlag) return;

        SetSceneState(splashScene, showLoadScreen);

        await LoadSceneSequence();
    }

    #endregion



    #region Scene Load Internal (Sequence)

    private async UniTask LoadSceneSequence()
    {
        if (_state.CurrentLoadScene != null)
        {
            if (_state.CurrentLoadScene.SceneReference.OperationHandle.IsValid())
            {
                _helper.UnloadAssetsFromPrevScene(_state.CurrentLoadScene);
                await _state.CurrentLoadScene.SceneReference.UnLoadScene();
            }
#if UNITY_EDITOR
            else
            {
                SceneManager.UnloadSceneAsync(_state.CurrentLoadScene.SceneReference.editorAsset.name);
            }
#endif
        }

        await LoadToNewScene();
    }

    private async UniTask LoadToNewScene()
    {
        var loadSceneInstance = await _helper.LoadSceneAsync(_state.SceneToLoad.SceneReference, LoadSceneMode.Additive);
        CompleteNewSceneLoaded(loadSceneInstance);
    }

    private void CompleteNewSceneLoaded(SceneInstance loadSceneInstance)
    {
        _state.CurrentLoadScene = _state.SceneToLoad;

        // Activate Loaded Scene
        SceneManager.SetActiveScene(loadSceneInstance.Scene);

        _state.IsLoadingFlag = false;

        if (_state.ShowLoadingCover) _loadingInterfaceEvent.RaiseEvent(false);
    }

    #endregion



    #region Utils

    private void SetSceneState(GameSceneSO sceneToLoad, bool showLoadScreen)
    {
        _state.SceneToLoad = sceneToLoad;
        _state.ShowLoadingCover = showLoadScreen;
        _state.IsLoadingFlag = true;
    }

    #endregion
}