using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Scripts.SDK.RewardCommand.Interface;
using UnityEngine;

namespace Scripts.SDK.RewardCommand
{
    public class CommandAds : ICommand
    {
        private static Action _onReward;
        private static Action _onFail;
        private static Action _onClose;
            
        
        public CommandAds(Action onReward, Action onFail = null)
        {
            _onReward = onReward;
            _onFail = onFail;
        }

        private async UniTaskVoid InvokeReward()
        {
            await UniTask.WaitUntil( ()=> SDK_Module.Instance.AppState == AppState.Foreground);
            await UniTask.WaitForSeconds(300);
            _onReward?.Invoke();
            _onReward = null;
            _onFail = null;
            _onClose = null;
        }
        
        public void Execute()
        {
            void AdView()
            {
                
            }
        }
    }
}