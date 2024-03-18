using System;
using System.Collections.Generic;

namespace IziHardGames.Apps.Abstractions.Lib
{
    /// <summary>
    /// Предоставляет обхекты которые позволяет зарегестрировать делегат для интервального выполнения.<br/>
    /// Если задача для выполнения имеет переменчивый интервал то??? лучше "выполнить-пересчитать-выполнить-пересчитать". То есть не пользоваться интервалом. <see cref="IziDelay"/>
    /// </summary>
    [StaticAbstraction]
    public static class IziInterval
    {
        public static readonly SelectorForProvider providers = new SelectorForProvider();
        public static IntervalProvider Default { get; set; }
        public static void SetDefault(IntervalProvider provider)
        {
            Default = provider;
        }
    }


    public class SelectorForProvider
    {
        private readonly Dictionary<Type, IntervalProvider> pairs = new Dictionary<Type, IntervalProvider>();
        public IntervalProvider this[Type type] { get => pairs[type]; set => AddOrUpdate(type, value); }
        public IEnumerable<IntervalProvider> All => pairs.Values;
        private void AddOrUpdate(Type type, IntervalProvider value)
        {
            if (pairs.TryGetValue(type, out var existed))
            {
                pairs[type] = value;
            }
            else
            {
                pairs.Add(type, value);
            }
        }
    }

    public abstract class IntervalProvider
    {
        protected readonly Dictionary<uint, IntervalGroupe> pairs = new Dictionary<uint, IntervalGroupe>();
        public IEnumerable<IntervalGroupe> All => pairs.Values;
        public void Set(uint startValue, uint value, Action action)
        {
            if (!pairs.TryGetValue(value, out IntervalGroupe groupe))
            {
                groupe = new IntervalGroupe()
                {
                    value = value,
                };
                pairs.Add(value, groupe);
            }
            groupe.Regist(startValue, value, action);
        }
        public T As<T>() where T : IntervalProvider
        {
            return this as T ?? throw new InvalidCastException($"Can't cast. CurrentType:{GetType().FullName}. Target:{typeof(T).FullName}");
        }
        public abstract void Execute();

        public void Unset(uint v1, uint v2, Action spawn)
        {
            throw new NotImplementedException();
        }
    }

    public class IntervalGroupe
    {
        public uint value;
        public uint ticks;
        private readonly List<IntervalData> intervalDatas = new List<IntervalData>();
        public IEnumerable<IntervalData> All => intervalDatas;
        internal void Regist(uint startValue, uint value, Action action)
        {
            IntervalData data = new IntervalData()
            {
                milliseconds = (int)value,
                millisecondsLeft = (int)startValue,
                action = action,
            };
            intervalDatas.Add(data);
        }
        public void SynchronizeAll(uint startValue)
        {
            throw new System.NotImplementedException();
        }
        public void SynchronizeAll()
        {
            foreach (var item in intervalDatas)
            {
                item.ResetLeftTime();
            }
        }
    }

    public class IntervalData
    {
        public int milliseconds;
        public int millisecondsLeft;
        public int triggerCount;
        public int ticksCount;
        public Action action;

        public bool Decrease(int milliseconds)
        {
            millisecondsLeft -= milliseconds;
            ticksCount++;
            return millisecondsLeft <= 0;
        }

        public void Fire()
        {
            action.Invoke();
            triggerCount++;
            millisecondsLeft = milliseconds;
        }

        internal void ResetLeftTime()
        {
            millisecondsLeft = milliseconds;
        }
    }
}
