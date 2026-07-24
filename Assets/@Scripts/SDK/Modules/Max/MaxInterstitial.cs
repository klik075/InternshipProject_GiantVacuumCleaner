
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MaxInterstitial : AbstractMaxAds
{
    #region Constructor & Destructor

    public MaxInterstitial()
    {
        Debugger.Log("Create interstitial video ad.");

        AdTypeString = "Interstitial";
        RegisterEvent();
    }

    public override void Disable()
    {
        UnregisterEvent();
    }

    #endregion



    #region Ads Methods (Acceesor)
    
    public override void Initialize()
    {
        Load();
    }

    public override void Show()
    {
        if (!MaxSdk.IsInterstitialReady(MaxDataKey.MaxInterstitialKey))
        {
            OnFail.Invoke(CommandOrderer.Interstitial);
            return;
        }
        
        MaxSdk.SetMuted(true);
        MaxSdk.ShowInterstitial(MaxDataKey.MaxInterstitialKey);
    }

    #endregion



    #region Regist & Unregist Events

    private void RegisterEvent()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnAdLoad;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnAdLoadFailed;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnAdDismissed;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += Revenue;
    }
    
    private void UnregisterEvent()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnAdLoad;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnAdLoadFailed;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnAdDismissed;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= Revenue;
    }

    #endregion



    #region Binding Events & Load
    
    protected override void Load()
    {
        MaxSdk.LoadInterstitial(MaxDataKey.MaxInterstitialKey);
    }

    private void OnAdLoad(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        var countryCode = MaxSdk.GetSdkConfiguration().CountryCode;

        SDKIntegrationSystem.CountryCode = countryCode;
    }
    
    private void OnAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        if(_adsAttempts++ < 3)
            Load();
        Debug.LogError("Interstitial video ad failed to load an ad with error : " + errorInfo.Message);
    }
    
    private void OnAdDismissed(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Load();
        OnSuccess.Invoke();
        _adsAttempts = 0;
    }

    #endregion
}
