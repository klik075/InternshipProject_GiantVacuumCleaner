using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Scripts.Framework.Modules.TaskModules
{
    public sealed class Sequencer
    {
        private readonly Queue<Func<UniTask>> _sequenceQueue = new();
        private bool _isExecuting;
        public Action EndSequence { get; set; }

        /// TODO : 시퀀스에 작업 추가
        public void AddToSequence(Func<UniTask> action)
        {
            _sequenceQueue.Enqueue(action);
            if (!_isExecuting) ExecuteNext().Forget();
        }
        
        /// TODO : 시퀀스를 실행하는 메서드
        private async UniTask ExecuteNext()
        {
            _isExecuting = true;
            while (_sequenceQueue.Count > 0)
            {
                Func<UniTask> action = _sequenceQueue.Dequeue();
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    Debugger.LogError($"Sequence execution error: {ex}");
                }
            }
            _isExecuting = false;
            // TODO : End Sequence Callback
            EndSequence?.Invoke();
        }
    }
}