
using System;
using Cysharp.Threading.Tasks;
using Firebase.Analytics;

public abstract class AbstractMaxAds : IDisposable
{
    #region Fields

    protected readonly object _lockObject = new();
    protected int _adsAttempts = 0;
    
    public Action OnSuccess;
    public Action<CommandOrderer> OnFail;

    protected string AdTypeString;

    #endregion
    
    

    #region Abstract

    public abstract void Initialize();
    protected abstract void Load();
    public abstract void Show();
    public abstract void Disable();

    protected void Revenue(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        lock (_lockObject)
        {
            var revenue = adInfo.Revenue;
            var countryCode = MaxSdk.GetSdkConfiguration().CountryCode;
            var networkName = adInfo.NetworkName;
            var adUnitIdentifier = adInfo.AdUnitIdentifier;
            
            SingularAdData data = new SingularAdData("AppLovin_" + AdTypeString, "USD", revenue);
            SingularSDK.AdRevenue(data);
            
            Parameter[] impressionParameters = {
                new("ad_UnitId", adUnitId),
                new("ad_impression", AdTypeString),
                new("ad_platform", "AppLovin"),
                new("ad_source", networkName),
                new("ad_country", countryCode),
                new("ad_unit_name", adUnitIdentifier),
                new("ad_format", adInfo.AdFormat),
                new("value", revenue),
                new("currency", "USD")
            };
            
            FirebaseAnalytics.LogEvent("Ad_impression", impressionParameters);
        }
    }

    #endregion
    
    

    #region Dispose

    public void Dispose()
    {
        Disable();
    }

    #endregion
}
