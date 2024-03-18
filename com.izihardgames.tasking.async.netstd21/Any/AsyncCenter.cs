using System.Runtime.CompilerServices;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using IziHardGames.Attributes;

namespace IziHardGames.Libs.Async
{
    /// <see cref="System.Threading.Tasks.Task"/>
    /// <see cref="System.Threading.Tasks.ValueTask"/>

    /// <summary>
    /// Централизизованное отслеживание, запуск и управление асинхронных задач.
    /// Объединяет различные API await/async. Центральным место является поддержка синаксиса async/await на уровне библиотеки .NetStandard21
    /// Заварачивает любой awaitable объект и дает новый awaitable объект, чтобы вызывающая сторона могла использовать await.
    /// Может работать как <see cref="AsyncSignaler"/> - создавать ожидание и вручную завершать ожидание.
    /// </summary>
    [Guid("5baf2acd-5028-4f8e-853b-aa3530c15008")]
    public static class AsyncCenter
    {
        internal const int TYPE_CUSTOM = 1;
        private static readonly Dictionary<int, AsyncInfo> infos = new Dictionary<int, AsyncInfo>(128);
        /// <summary>
        /// Action из объекта state-machine, который CLR сгенерировал для асинхронной операции. Передается как аргумент в метод <see cref="AsyncCenterAwaiter.OnCompleted(Action)"/> 
        /// в случае когда операция не может быть завершена синхронно в момент await
        /// </summary>
        private static readonly Dictionary<int, Action> continuations = new Dictionary<int, Action>(128);
        private static int counter;
        public static readonly AsyncCenterTask CompletedTask;

        static AsyncCenter()
        {
            CompletedTask = new AsyncCenterTask(default);
            infos.Add(default, new AsyncInfo(default, (int)TaskStatus.RanToCompletion, true));
        }

        /// <summary>
        /// завершает операцию которая ожидается. 
        /// Если адаптер оборачивал <see cref="Task"/> или <see cref="ValueTask"/> то их выполнение не прерывается???
        /// </summary>
        /// <param name="id"></param>
        public static void ManualComplete(int id)
        {
#if DEBUG || UNITY_EDITOR
            if (infos[id].IsComplete) throw new InvalidOperationException("Задача уже завершена. Возможно синхронно потому что после выполненя AsyncInfo удаляется");
#endif
            /// если <see cref="continuations"/> еще не задан (awaiter не затребован) но уже устанавливается результат?
            if (continuations.TryGetValue(id, out var result))
            {
                result.Invoke();
                // do not remove from continuations. it removed in SetComplete()
            }
            else
            {
                lock (infos)
                {
                    var info = new AsyncInfo(id, (int)TaskStatus.WaitingForActivation, false);
                    if (infos.TryGetValue(id, out var existedInfo))
                    {
                        infos[id] = info;
                    }
                    else
                    {
                        infos.Add(id, info);
                    }
                }
            }
        }
        /// <summary>
        /// Create Awaiter for custom set completion. 
        /// Для завершения этого ожидания необходимо вызвать <see cref="ManualComplete(int)"/> и передать <see cref="AsyncCenterTask.id"/> из созданной задачи.
        /// Не может завершить задачу до ее создания как в случае с сигналером
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static AsyncCenterTask CreateAsyncPoint()
        {
            int id = Interlocked.Increment(ref counter);
            lock (infos)
            {
                infos.Add(id, new AsyncInfo(id, (int)TaskStatus.Running, false));
            }
            return new AsyncCenterTask(id);
        }

        public static AsyncCenterTask AwaitAsync<T>(T task) where T : ITaskAdapter
        {
            throw new System.NotImplementedException();
        }
        public static AsyncCenterTask AwaitAsync(ValueTask task)
        {
            throw new System.NotImplementedException();
        }
        public static AsyncCenterTask AwaitAsync(Task task)
        {
            throw new System.NotImplementedException();
        }
        internal static bool IsCompleted(int id)
        {
            if (infos.TryGetValue(id, out var info))
            {
                return info.IsComplete;
            }
            return true;
        }

        internal static void RegistAsync(int id, Action continuation)
        {
            continuations.Add(id, continuation);
        }

        internal static void SetComplete(int id)
        {
            lock (infos)
            {
                AsyncInfo info = infos[id];
                if (info.status == (int)TaskStatus.WaitingForActivation)
                {

                }

                if (continuations.TryGetValue(id, out var continuation))
                {
                    continuations.Remove(id);
                }
                if (id != 0)
                {
                    infos.Remove(id);
                }
            }
        }
        [UnityHotReloadEditor]
        public static void CleanupStatics()
        {
            infos.Clear();
            infos.Add(default, new AsyncInfo(default, (int)TaskStatus.RanToCompletion, true));
            continuations.Clear();
            counter = default;
        }
    }

    internal readonly struct AsyncInfo
    {
        public readonly int id;
        /// <summary>
        /// <see cref="System.Threading.Tasks.TaskStatus"/>
        /// </summary>
        public readonly int status;
        public readonly bool IsComplete;
        public TaskStatus Status => (TaskStatus)status;

        public AsyncInfo(int type, int status, bool isComplete)
        {
            this.id = type;
            this.status = status;
            this.IsComplete = isComplete;
        }
    }

    public interface ITaskAdapter
    {

    }

    [Guid("2bf2eefb-b264-496e-bb7e-a71075116f58")]
    public readonly struct AsyncCenterTask
    {
        public readonly int id;
        public AsyncCenterTask(int id)
        {
            this.id = id;
        }

        // TODO: status; Cancellation Token; error
        public AsyncCenterAwaiter GetAwaiter()
        {
            return new AsyncCenterAwaiter(id);
        }
        public AsyncCenterTask ContinueWith(Action<AsyncCenterTask> action)
        {
            throw new System.NotImplementedException();
        }
        public AsyncCenterTask ContinueWith(Action action)
        {
            throw new System.NotImplementedException();
        }
        public AsyncCenterTask ContinueWith(Task task)
        {
            throw new System.NotImplementedException();
        }
    }

    [Guid("702cc992-736a-42cd-8699-54d3067ba89d")]
    public readonly struct AsyncCenterAwaiter : INotifyCompletion
    {
        public readonly int id;
        public bool IsCompleted => AsyncCenter.IsCompleted(id);
        public AsyncCenterAwaiter(int id)
        {
            this.id = id;
        }
        public void OnCompleted(Action continuation)
        {
            AsyncCenter.RegistAsync(id, continuation);
        }
        public void GetResult()
        {
            AsyncCenter.SetComplete(id);
        }
    }
}
