using Cysharp.Threading.Tasks;
using Scripts.Framework.Events.SO;
using Scripts.Framework.Managers.Interface;
using Scripts.Framework.Scenes.SO;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Scripts.Framework.Managers.Scene
{
    public class GameSceneManager : MonoBehaviour, IManager
    {
        #region Fields

        [Header("GamePlaySupporter")] 
        [SerializeField] private bool _isNeedLobbyToo = true;
        [SerializeField] private GameSceneSO _gamePlaySupporterSO;
        // TODO. Input Readers
    
        [Header("Listening to")] 
        [SerializeField] private LoadSceneEventChannelSO _loadLoadingModularScene;
        [SerializeField] private LoadSceneEventChannelSO _loadLobbyScene;
        [SerializeField] private LoadSceneEventChannelSO _loadGamePlayScene;

        [Header("Broadcasting on")]
        [SerializeField] private GameEventSO<bool> _loadingInterfaceEvent;

        private SceneInstance _gamePlaySupporterSceneInstance;
        private GameSceneSO _sceneToLoad;
        private GameSceneSO _currentLoadedScene;
    
        // new loading request while already loading a new scene.
        private bool _isLoadingFlag;
        private bool _showLoadingCover;

        #endregion



        #region Unity Behavior

        private void OnEnable()
        {
            _loadLoadingModularScene.OnSceneLoadRequest += LoadModularScene;
            _loadLobbyScene.OnSceneLoadRequest += LoadLobbyScene;
            _loadGamePlayScene.OnSceneLoadRequest += LoadGameplayScene;
        }

        private void OnDisable()
        {
            _loadLoadingModularScene.OnSceneLoadRequest -= LoadModularScene;
            _loadLobbyScene.OnSceneLoadRequest -= LoadLobbyScene;
            _loadGamePlayScene.OnSceneLoadRequest -= LoadGameplayScene;
        }

        #endregion



        #region Scenes

        private async UniTask LoadLobbyScene(GameSceneSO loadToLobby, bool showLoadScreen, Action<int, int> action = null, bool fadeScreen = false)
        {
            if (_isLoadingFlag) return;
            if (_showLoadingCover) _loadingInterfaceEvent.RaiseEvent(true);

            _sceneToLoad = loadToLobby;
            _showLoadingCover = showLoadScreen;
            _isLoadingFlag = true;

            await LoadAssets(loadToLobby, action);

            if (_isNeedLobbyToo)
            {
                await LoadOrUnloadSupporterScene(true);
            }
            else
            {
                await LoadOrUnloadSupporterScene(false);
            }
        
            await UnloadPreviousScene(loadToLobby);
        }
    
        private async UniTask LoadOrUnloadSupporterScene(bool needSupporter)
        {
            if (needSupporter)
            {
                if (!_gamePlaySupporterSceneInstance.Scene.isLoaded)
                {
                    _gamePlaySupporterSceneInstance = await _gamePlaySupporterSO.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
                }
            }
            else
            {
                if (_gamePlaySupporterSceneInstance.Scene.isLoaded)
                {
                    await Addressables.UnloadSceneAsync(_gamePlaySupporterSceneInstance);
                }
            }
        }
    
        private async UniTask LoadGameplayScene( GameSceneSO loadToGameplay, bool showLoadScreen, Action<int, int> action = null, bool fadeScreen = false)
        {
            if (_isLoadingFlag) return;
            if (_showLoadingCover) _loadingInterfaceEvent.RaiseEvent(true);
        
            _sceneToLoad = loadToGameplay;
            _showLoadingCover = showLoadScreen;
            _isLoadingFlag = true;
        
            await LoadAssets(loadToGameplay,action);
        
            if(_gamePlaySupporterSceneInstance.Scene is not { isLoaded: true })
            {
                _gamePlaySupporterSceneInstance = await _gamePlaySupporterSO.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
                await UnloadPreviousScene(loadToGameplay);
            }
            else
            {
                await UnloadPreviousScene(loadToGameplay);
            }
        }

        #endregion



        #region Modular Scene

        private async UniTask LoadModularScene(GameSceneSO loadingModularScene, bool showLoadScreen, Action<int, int> action = null,bool fadeScreen = false)
        {
            if (_isLoadingFlag) return;

            _sceneToLoad = loadingModularScene;
            _showLoadingCover = showLoadScreen;
            _isLoadingFlag = true;

            await UnloadPreviousScene(loadingModularScene);
        }

        /// <summary>
        /// Asset Loaded
        /// </summary>
        private async UniTask LoadAssets(GameSceneSO gameSceneSO,Action<int,int> action = null)
        {
            for (int index = 0; index < gameSceneSO.AssetLabels.Count; ++index)
                await Asset.Core.AMS.LoadAssets<UnityEngine.Object>(gameSceneSO.AssetLabels[index], action);
        }

        private void UnloadAssets(GameSceneSO gameSceneSO)
        {
            for (int index = 0; index < gameSceneSO.AssetLabels.Count; ++index)
                Asset.Core.AMS.ReleaseAssetLabel(gameSceneSO.AssetLabels[index]);
        }

        #endregion



        #region Sequence

        private async UniTask UnloadPreviousScene(GameSceneSO gameSceneSO)
        {
            if (_currentLoadedScene != null)
            {
                if (_currentLoadedScene.SceneReference.OperationHandle.IsValid())
                {
                    // UnloadAssets(gameSceneSO);
                    await _currentLoadedScene.SceneReference.UnLoadScene();
                }
#if UNITY_EDITOR
                else
                {
                    SceneManager.UnloadSceneAsync(_currentLoadedScene.SceneReference.editorAsset.name);
                }
#endif
            }

            await LoadNewScene();
        }

        private async UniTask LoadNewScene()
        {
            var loadSceneInstance = await _sceneToLoad.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0).ToUniTask();
            // Complete Methods
            CompleteNewSceneLoaded(loadSceneInstance);
        }

        private void CompleteNewSceneLoaded(SceneInstance loadSceneInstance)
        {
            _currentLoadedScene = _sceneToLoad;

            SceneManager.SetActiveScene(loadSceneInstance.Scene);

            _isLoadingFlag = false;

            if (_showLoadingCover)
            {
                // Cover Off
                _loadingInterfaceEvent.RaiseEvent(false);
            }
        
            StartGamePlay();
        }

        private void StartGamePlay()
        {
            // Raise Events
            Debugger.Log("StartGamePlay");
        }

        #endregion



        #region Clears

        public void Clear()
        {
            // TODO
        }

        #endregion
    }
}
