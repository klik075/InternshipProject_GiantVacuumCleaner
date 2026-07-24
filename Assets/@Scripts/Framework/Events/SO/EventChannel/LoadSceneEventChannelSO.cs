
using System;
using Cysharp.Threading.Tasks;
using Scripts.Framework.Scenes.SO;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadSceneEventChannel", menuName = "Events/Load Scene Event Channel")]
public class LoadSceneEventChannelSO : DescriptionSO
{
    #region Fields

    public Func<GameSceneSO, bool, Action<int, int>, bool, UniTask> OnSceneLoadRequest;

    #endregion



    #region Raise

    public async UniTask RaiseEvent(
        GameSceneSO sceneToLoad, bool showLoadingScreen, Action<int, int> action = null, bool fadeScreen = false)
    {
        if (OnSceneLoadRequest != null)
        {
            await OnSceneLoadRequest.Invoke(sceneToLoad,showLoadingScreen, action, fadeScreen);
        }
        else
        {
            Debug.LogWarning("OnSceneLoadRequest has no subscribers.");
        }
    }

    #endregion
}
