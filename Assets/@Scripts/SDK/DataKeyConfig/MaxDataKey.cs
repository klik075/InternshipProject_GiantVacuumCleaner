
public static class MaxDataKey
{
    public const string MaxSdkKey = "EG4dCO2mV2THPcolJ7UkHmIGIfTqtwfpRaimZ-lyk-OV5RSBpi4KMT6P3FnnemsgdzXD-3swcClOldu3paIfqG";

#if UNITY_ANDROID
    public const string MaxBannerKey = "b8f3ca472cb5071d";
    public const string MaxInterstitialKey = "1cc1877faf5adf7f";
    public const string MaxRewardKey = "49f8b5595e81b470";
#endif

#if UNITY_IOS
    public const string MaxBannerKey = "cc7f6ef68f6f98eb";
    public const string MaxInterstitialKey = "2102a9969415fbaf";
    public const string MaxRewardKey = "199cad244b20fdc6";
#endif

    /// <summary>
    /// 실제 테스트키를 추가하십쇼
    /// </summary>
    public static readonly string[] MaxTestDeviceKey = 
    {
        "2e2a829a-5aff-4532-bdeb-2ba792250a95",
    };
}