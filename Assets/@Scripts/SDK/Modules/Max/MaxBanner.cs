
using Cysharp.Threading.Tasks;

public class MaxBanner : AbstractMaxAds
{
    #region Fields

    private bool _isShowBanner;
    private bool _hasBanner = true;

    #endregion
    
    
    
    #region Constructor & Destructor

    public MaxBanner()
    {
        Debugger.Log("Create banner video ad.");

        AdTypeString = "Banner";
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
        Show();
    }
    
    public override void Show()
    {
        MaxSdk.ShowBanner(MaxDataKey.MaxBannerKey);
        _isShowBanner = true;
    }

    public void Hide()
    {
        MaxSdk.HideBanner(MaxDataKey.MaxBannerKey);
        _isShowBanner = false;
    }

    public void Destroy() => DestroyInternal().Forget();

    private async UniTask DestroyInternal()
    {
        if (!_hasBanner)
        {
            return;
        }

        UniTask.WaitUntil(() => _isShowBanner);
        MaxSdk.DestroyBanner(MaxDataKey.MaxBannerKey);
        _hasBanner = false;
    }

    #endregion
    
    
    
    #region Regist & Unregist Events

    private void RegisterEvent()
    {
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += Revenue;
    }

    private void UnregisterEvent()
    {
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= Revenue;
    }

    #endregion



    #region Binding Events & Load

    protected override void Load()
    {
        MaxSdk.CreateBanner(MaxDataKey.MaxBannerKey, MaxSdkBase.BannerPosition.BottomCenter);
    }

    #endregion
}
