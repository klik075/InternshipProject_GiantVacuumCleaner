
using System;
using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;

public class FirebaseSDK : MonoBehaviour, ISDKSystem
{
    #region Field

    private FirebaseApp _defaultInstance; 
    
    public bool IsInitialize { get; set; }

    #endregion
    
    
    
    #region Initialize

    public async UniTask Initialize()
    {
        if (IsInitialize) return;

        await InitializeInternal();
    }

    private async UniTask InitializeInternal()
    {
        await UniTask.SwitchToMainThread();
        
        try
        {
            await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
            
            // Async Complete
            _defaultInstance = FirebaseApp.DefaultInstance;
            IsInitialize = true;
        }
        catch (Exception exception)
        {
            Debugger.LogError($"Error! firebase Initialize failed. {exception.Message}");
        }
    }

    #endregion
}
