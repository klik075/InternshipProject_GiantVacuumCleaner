using Cysharp.Threading.Tasks;
using Scripts.Framework.Events.SO;
using Scripts.Framework.Scenes.SO;
using UnityEngine;

namespace Scripts.Framework.Scenes.Initialization
{
    public class GameLoadSequence : MonoBehaviour
    {
        #region Fields

        [Header("Game Sequence Loading Use Cover Setting")]
        [SerializeField] private bool _showInitLoadingCover;

        [Header("Broadcasting on")] 
        [SerializeField] private GameEventSO<bool> _toggleLoadingInitEvent;

        [Header("You want GameSceneSO")] 
        [SerializeField] private GameSceneSO _wantMoveScene;
        [SerializeField] private LoadSceneEventChannelSO _wantMoveSceneLoadEventChannel;

        #endregion



        #region Unity Behavior

        private async void Start()
        {
            if (_showInitLoadingCover)
            {
                _toggleLoadingInitEvent.RaiseEvent(true);
            }
        
            await GameLoadSequenceProcess();
        }

        #endregion



        #region Load Sequence

        private async UniTask GameLoadSequenceProcess()
        {
            await UniTask.Delay(300);
            // Addressables Init
            await Managers.Asset.Core.AMS.InitializeAssets();
        
            // SDK Init
        
            // Scene Load & Asset Load
            await _wantMoveSceneLoadEventChannel.RaiseEvent(_wantMoveScene, false);

            // Turn Off Cover
            if (_showInitLoadingCover)
            {
                _toggleLoadingInitEvent.RaiseEvent(false);
            }
        }

        #endregion
    }
}
