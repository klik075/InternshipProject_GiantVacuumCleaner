
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MaxReward : AbstractMaxAds
{
    #region Constructor & Destructor

    public MaxReward()
    {
        Debugger.Log("Create reward video ad.");

        AdTypeString = "Reward";
        RegisterEvent();
    }

    public override void Disable()
    {
        UnregisterEvent();
    }

    #endregion



    #region Ads Method (Accssor)

    public override void Initialize()
    {
        Load();
    }
    
    public override void Show()
    {
        if (!MaxSdk.IsRewardedAdReady(MaxDataKey.MaxRewardKey))
        {
            Debugger.Log("Fail");
            OnFail.Invoke(CommandOrderer.Reward);
            return;
        }
        
        MaxSdk.SetMuted(true);
        MaxSdk.ShowRewardedAd(MaxDataKey.MaxRewardKey);
        Debugger.Log("SHOW RE:");
    }

    #endregion



    #region Regist & Unregist Events

    private void RegisterEvent()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnAdLoad;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnAdLoadFailed;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnAdDisplayFailed;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnAdDismissed;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnAdReceiveRewarded;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += Revenue;
    }

    private void UnregisterEvent()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnAdLoad;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnAdLoadFailed;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnAdDisplayFailed;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnAdDismissed;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnAdReceiveRewarded;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= Revenue;
    }

    #endregion
    
    
    
    #region Binding Events & Load
    
    protected override void Load()
    {
        MaxSdk.LoadRewardedAd(MaxDataKey.MaxRewardKey);
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
        Debugger.LogError("Reward video ad failed to load an ad with error : " + errorInfo.Message);
    }

    /// <summary>
    /// 광고가 로드는 성공했으나, 실제로 화면에 표시되지 못했을 때 호출
    /// 광고가 로드된 후 사용자에게 보여주려 할 때 기술적인 문제로 인해 표시되지 않는 경우가 이에 해당
    /// [!] 그래서 뭐요? 뭐 퍼즈팝업 또는 뭐 리워드 로드가 실패 팝업 같은거 띄워주세요. [!]
    /// </summary>
    private void OnAdDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        Load();
        _adsAttempts = 0;
        // 여기서 뭔가 해줘용.. 알았죠?
    }
    
    private void OnAdDismissed(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Load();
        _adsAttempts = 0;
    }
    
    private void OnAdReceiveRewarded(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        Load();
        OnSuccess.Invoke();
        _adsAttempts = 0;
    }

    #endregion
}
