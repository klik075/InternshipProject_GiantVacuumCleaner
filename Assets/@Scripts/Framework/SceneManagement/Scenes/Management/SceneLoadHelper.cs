
using Cysharp.Threading.Tasks;
using Scripts.Framework.Managers.Asset.Core;
using Scripts.Framework.Scenes.SO;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneLoadSystem Helper
/// </summary>
public class SceneLoadHelper
{
    #region Extensions

    public async UniTask<SceneInstance> LoadSceneAsync(AssetReference sceneReference, LoadSceneMode loadSceneMode)
    {
        return await sceneReference.LoadSceneAsync(loadSceneMode).ToUniTask();
    }

    public async UniTask UnloadSceneAsync(SceneInstance sceneInstance)
    {
        await Addressables.UnloadSceneAsync(sceneInstance).ToUniTask();
    }

    public async UniTask LoadAssetsFromScene(GameSceneSO gameSceneSO,Action<int, int> action = null)
    {
        foreach (var label in gameSceneSO.AssetLabels)
            await AMS.LoadAssets<UnityEngine.Object>(label, action);
        
    }

    public void UnloadAssetsFromPrevScene(GameSceneSO gameSceneSO)
    {
        foreach(var label in gameSceneSO.AssetLabels)
            AMS.ReleaseAssetLabel(label);
    }

    #endregion
}
