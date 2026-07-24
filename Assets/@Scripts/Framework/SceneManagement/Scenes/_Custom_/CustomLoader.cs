
using Scripts.Framework.Scenes.SO;
using UnityEngine;
using UnityEngine.UI;

public class CustomLoader : MonoBehaviour
{
    #region Fields

    [Header("Broadcasting on")]
    [SerializeField] private RequestSequenceEventChannelSO _sequenceEventChannel;

    [Header("You need Scenes")]
    [SerializeField] private GameSceneSO _gameScene;
    [SerializeField] private bool useLobby;
    [SerializeField]
    private Slider slider;
    #endregion



    #region Unity Behavior

    private async void Start()
    {
        // 원하는 부분 원하시는 씬으로 조건 넣어서 관리하시면 됩니다.
        // ex) 특정 레벨부터는 _gameScene아닐 경우 _lobbyScene
        GameSceneSO so = _gameScene;
        await _sequenceEventChannel.RaiseEvent(so, false,sliderUpdate);
    }

    public void sliderUpdate(int current, int total)
    {
        slider.value = current / (float)total;
    }
    #endregion
}
