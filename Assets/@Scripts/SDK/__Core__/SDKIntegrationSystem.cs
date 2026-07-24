
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Scripts.Framework.Modules.TimeModules;
using Scripts.Framework.Utility;
using UnityEngine;

public enum InterstitialKey { NextStage, Resume, ReturnLobby }
public enum CommandOrderer { Interstitial, Reward, IAP }

public class SDKIntegrationSystem : IndividualSingleton<SDKIntegrationSystem>
{
    #region Fields
    
    // Components
    private MaxSDK _maxSdk;
    private SingularSDK _singularSdk;
    private FirebaseSDK _firebaseSdk;

    private bool _isInitialize = false;
    private readonly Dictionary<InterstitialKey, int> _interstitialCoolTimes = new()
    {
        { InterstitialKey.NextStage, Level_Inter_Cool },
        { InterstitialKey.Resume, Common_Inter_Cool },
        { InterstitialKey.ReturnLobby, Common_Inter_Cool }
    };

    public static AppState AppState = AppState.Foreground;
    public event Action<CommandOrderer> CommandInvoke;

    #endregion



    #region Properties

    public static string CountryCode { get; set; }
    public static int Level_Inter_Cool { get; set; } = 480;
    public static int Common_Inter_Cool { get; set; } = 10;

    #endregion



    #region Event Register & Unregister

    private void OnEnable()
    {
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        CommandInvoke += OnResetCommand;
    }

    private void OnDisable()
    {
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
        CommandInvoke -= OnResetCommand;
    }

    #endregion



    #region Initializer

    public async UniTask Initialize()
    {
        if (_isInitialize) return;
        
        await InitializeInternal();
        
        _isInitialize = true;
    }

    private async UniTask InitializeInternal()
    {
        _firebaseSdk = InstantiateAndComponent<FirebaseSDK>();
        _maxSdk = InstantiateAndComponent<MaxSDK>();
        _singularSdk = InstantiateAndComponent<SingularSDK>();
        
        await _firebaseSdk.Initialize();
        await _maxSdk.Initialize();
        await _singularSdk.Initialize();

    }

    private T InstantiateAndComponent<T>() where T : Component
    {
        var newGameObject = new GameObject();
        newGameObject.transform.SetParent(transform);
        var component = newGameObject.GetAddComponent<T>();
        newGameObject.gameObject.name = typeof(T).Name;

        return component;
    }

    #endregion



    #region App State Change

    private async void OnAppStateChanged(AppState appState)
    {
        AppState = appState;
        if (AppState is AppState.Foreground)
        {
            await ShowInterstitial(InterstitialKey.Resume,
                new AdsCommand(() => { Debugger.LogError("Show"); }, CommandOrderer.Interstitial));
        }
        else
        {
            
        }
    }

    #endregion



    #region Command

    public void OnCommandInvoke(CommandOrderer orderer) => CommandInvoke?.Invoke(orderer);
    
    public void OnResetCommand(CommandOrderer orderer)
    {
        switch (orderer)
        {
            case CommandOrderer.Interstitial:
                _maxSdk.Interstitial.OnSuccess = null;
                _maxSdk.Interstitial.OnFail = null;
                break;
            case CommandOrderer.Reward:
                _maxSdk.Reward.OnSuccess = null;
                _maxSdk.Reward.OnFail = null;
                break;
        }
    }

    #endregion



    #region Ads Events

    public async UniTask ShowInterstitial(InterstitialKey interstitialKey, AdsCommand adsCommand)
    {
        await UniTask.Delay(100);

        if (!ValidateShowInterstitialAds(interstitialKey))
        {
            adsCommand.Execute();
            OnCommandInvoke(CommandOrderer.Interstitial);
            return;
        }

        _maxSdk.Interstitial.OnSuccess = adsCommand.Execute;
        _maxSdk.Interstitial.OnFail = OnCommandInvoke;
        _maxSdk.Interstitial.Show();
    }
    
    public void ShowReward(AdsCommand command)
    {
        _maxSdk.Reward.OnSuccess = command.Execute;
        _maxSdk.Reward.OnFail = OnCommandInvoke;
        _maxSdk.Reward.Show();
        Debugger.Log("showReward");
    }
    
    private async UniTask ResumeCommand()
    {
        await UniTask.SwitchToMainThread();
        // 팝업 키든지 말든지 알아서 하쇼
        OnResetCommand(CommandOrderer.Interstitial);
    }

    #endregion



    #region Ads Validate

    public bool ValidateShowInterstitialAds(InterstitialKey interstitialKey)
    {
        var handler = new TimeHandler(DateType.SinceSecond);
        var coolTime = _interstitialCoolTimes[interstitialKey];
        
        return handler.IsSince(interstitialKey.ToString(), coolTime, true);
    }

    #endregion
}
