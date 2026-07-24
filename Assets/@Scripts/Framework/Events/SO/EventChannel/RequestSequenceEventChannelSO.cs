
using System;
using Cysharp.Threading.Tasks;
using Scripts.Framework.Scenes.SO;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "RequestSequenceEventChannel", menuName = "Events/Request Sequence Event Channel")]
public class RequestSequenceEventChannelSO : DescriptionSO
{
    #region Fields

    public Func<GameSceneSO, bool,Action<int,int>, bool, UniTask> OnSequenceRequested;

    #endregion



    #region Raise

    public async UniTask RaiseEvent(
        GameSceneSO sceneToLoad, bool showInitializeLoadingCover, Action<int,int> action = null, bool fadeScreen = false)
    {
        if (OnSequenceRequested != null)
        {
            await OnSequenceRequested.Invoke(sceneToLoad, showInitializeLoadingCover, action, fadeScreen);
        }
        else
        {
            Debug.LogWarning("OnSequenceRequested has no subscribers.");
        }
    }

    #endregion
}
