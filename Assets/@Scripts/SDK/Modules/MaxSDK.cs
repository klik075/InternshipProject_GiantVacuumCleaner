
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MaxSDK : MonoBehaviour, ISDKSystem
{
    #region Fields

    public MaxInterstitial Interstitial { get; private set; }
    public MaxReward Reward { get; private set; }
    public MaxBanner Banner { get; private set; }
    
    public bool IsInitialize { get; set; }

    #endregion



    #region Initiialize

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
            Interstitial = new MaxInterstitial();
            Reward = new MaxReward();
            Banner = new MaxBanner();

            MaxSdkCallbacks.OnSdkInitializedEvent += _ =>
            {
                Interstitial.Initialize();
                Reward.Initialize();
                Banner.Initialize();
            };

            MaxSdk.SetSdkKey(MaxDataKey.MaxSdkKey);
            MaxSdk.SetTestDeviceAdvertisingIdentifiers(MaxDataKey.MaxTestDeviceKey);
            MaxSdk.InitializeSdk();
            InitializeGdpr();
            IsInitialize = true;
        }
        catch (Exception exception)
        {
            Debugger.LogError($"Initialize Failed to MAX SDK : {exception.Message}");
        }
    }

    private void InitializeGdpr()
    {
        var sdkConfiguration = MaxSdk.GetSdkConfiguration();
        if (sdkConfiguration.ConsentFlowUserGeography == MaxSdkBase.ConsentFlowUserGeography.Gdpr)
        {
            LoadAndShowCmpFlow();
        }
    }

    private void LoadAndShowCmpFlow()
    {
        var cmpService = MaxSdk.CmpService;
        cmpService.ShowCmpForExistingUser(error =>
        {
            if (error == null)
            {
                Debugger.Log("CMP dialog shown successfully.");
            }
            else
            {
                Debugger.LogError("Failed to show CMP dialog: " + error.Message);
            }
        });
    }

    #endregion
}
