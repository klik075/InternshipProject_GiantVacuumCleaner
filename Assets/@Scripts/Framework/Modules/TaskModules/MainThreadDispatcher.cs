using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scripts.Framework.Modules.SingletonModule.IndividualSingleton;

namespace Scripts.Framework.Modules.TaskModules
{
    public class MainThreadDispatcher : IndividualSingletonPersist<MainThreadDispatcher>
    {
        private static readonly Queue<Func<UniTask>> ExecutionQueue = new();
        
        private void Update()
        {
            lock (ExecutionQueue)
            {
                while (ExecutionQueue.Count > 0)
                {
                    ExecutionQueue.Dequeue().Invoke().Forget();
                }
            }
        }

        /// <summary> 메인 스레드에 작업 요청(코루틴) </summary>
        public void Request(IEnumerator coroutine)
        {
            lock (ExecutionQueue)
            {
                ExecutionQueue.Enqueue(async () => 
                {
                    await UniTask.SwitchToMainThread();
                    StartCoroutine(coroutine);
                });
            }
        }

        /// <summary> 메인 스레드에 작업 요청(메소드) </summary>
        public void Request(Action action)
        {
            lock (ExecutionQueue)
            {
                ExecutionQueue.Enqueue(async () => 
                {
                    await UniTask.SwitchToMainThread();
                    action();
                });
            }
        }

        /// <summary> 메인 스레드에 작업 요청 및 대기(await) </summary>
        public UniTask RequestAsync(Action action)
        {
            var tcs = new UniTaskCompletionSource();

            Request(WrappedAction);
            return tcs.Task;

            void WrappedAction()
            {
                try
                {
                    action();
                    tcs.TrySetResult();
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }
        }

        /// <summary> 메인 스레드에 작업 요청 및 대기(await) + 값 받아오기 </summary>
        public UniTask<T> RequestAsync<T>(Func<T> action)
        {
            var tcs = new UniTaskCompletionSource<T>();

            Request(WrappedAction);
            return tcs.Task;

            void WrappedAction()
            {
                try
                {
                    T result = action();
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }
        }
    }
}
