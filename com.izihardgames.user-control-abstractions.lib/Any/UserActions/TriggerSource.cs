using System;
using System.Collections.Generic;
using System.Linq;
using IziHardGames.UserControl.Abstractions.NetStd21.Attributes;


namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    public abstract class TriggerSource : ITriggerSource
    {
        public virtual bool IsTriggered { get; set; }
        public abstract void Collect();
        public abstract void Calculate();
        public void Clean()
        {
            IsTriggered = false;
        }
    }

    /// <summary>
    /// Для числовых значений. Например для диапазона значений от -1 до 1. 0 Значит выключено, остальные значения - включено но с индекацией направления
    /// </summary>
    public abstract class TriggerByDelta : TriggerSource
    {
        internal InputCollectTask task;
        internal PairContainerAndDevice pair;
        protected bool isDelta;

        public override void Collect()
        {
            this.isDelta = pair.container.data.IsChanged;
        }
        public override void Calculate()
        {
            IsTriggered = isDelta;
        }
        internal void SetTask(InputCollectTask task)
        {
            this.task = task;
        }

        internal void SetPair(PairContainerAndDevice pair)
        {
            this.pair = pair;
        }
    }

    /// <summary>
    /// Axis-based trigger.
    /// Например когда устройство посылает значение от -1 до 1 (мышь передает вектор смещения. Если длина вектора 0 то движения не было)
    /// </summary>
    public class TriggerByAxis : TriggerByDelta
    {
        public override void Collect()
        {
            throw new NotImplementedException();
        }
        public override void Calculate()
        {
            throw new NotImplementedException();
        }
    }

    public class TriggerByPointerMove : TriggerByDelta
    {

    }

    public class TriggerByKeyPress : TriggerSource, ITriggeredByKey
    {
        public int keyCode;
        internal PairContainerAndDevice pair;
        private InputCollectTask collectTask;
        internal void SetTask(InputCollectTask task)
        {
            this.collectTask = task;
        }
        internal void SetPair(PairContainerAndDevice pair)
        {
            this.pair = pair;
        }
        internal void SetKey(int code)
        {
            this.keyCode = code;
        }
        public override void Collect()
        {

        }

        public override void Calculate()
        {
            IsTriggered = pair.container.data.isActive;
        }
    }

    [Registry]
    public static class RegistryForTriggers
    {
        private readonly static List<TriggerSource> triggers = new List<TriggerSource>();
        public static List<TriggerSource> Triggersss => triggers;
        public static T GetOrCreate<T>(Func<T, bool> predictate, Func<T> factory) where T : TriggerSource
        {
            var existed = triggers.FirstOrDefault(x =>
            {
                if (x is T item)
                {
                    if (predictate(item)) return true;
                }
                return false;
            });
            if (existed == null)
            {
                var newItem = factory();
                triggers.Add(newItem);
                return newItem;
            }
            return existed as T;
        }
    }


}
