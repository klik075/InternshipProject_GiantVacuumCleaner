using System;
using Scripts.Framework.Modules.SingletonModule.IndividualSingleton;

namespace Scripts.SDK
{
    public enum AppState {Foreground, Background, Quit}
    public class SDK_Module : IndividualSingletonPersist<SDK_Module>
    {
        public AppState AppState { get; private set; }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) AppState = AppState.Background;
            else AppState = AppState.Foreground;
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            AppState = AppState.Quit;
        }
    }
}