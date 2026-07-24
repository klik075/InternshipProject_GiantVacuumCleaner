
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Common;
using UnityEngine;

public abstract class AbstractCommand
{
    public virtual void Execute()
    {
        UniTask.SwitchToMainThread();
        UniTask.WaitUntil(()=> SDKIntegrationSystem.AppState == AppState.Foreground);
        UniTask.Delay(300);
    }
}
