
using System;

namespace IziHardGames.UserControl.Abstractions.NetStd21.Environments
{
    public enum EEnvironmentStateAsignType
    {
        None,
        Permanent,
        CollectAtUiLoop,
        Push,
    }

    /// <summary>
    /// <see cref="IEnvironmentState"/>
    /// </summary>
    public class EnvironmentState
    {
        public bool StateImmediate { get { state = collector.GetState(); return state; } }
        /// <summary>
        /// Это состояния после UserControl Loop которые вызывает <see cref="Collect"/>. 
        /// Доступ к значению до выполненияя User Contol loop может быть не актуальным. см. <see cref="UserEnvironmentAbstract.CollectStates"/>
        /// </summary>
        public bool State => state;
        private bool state;
        private bool isPermanent;
        private EnvironmentStateCollector? collector;

        internal EnvironmentState()
        {

        }

        public EnvironmentState(bool permanentValue)
        {
            state = permanentValue;
            isPermanent = true;
        }
        public EnvironmentState(Func<bool> collector)
        {
            this.collector = new EnvironmentStateCollectorWithFunc(collector);
        }
        public void Collect()
        {
            if (isPermanent) return;
            state = collector.GetState();
        }

        public void SetCollectorState(EnvironmentStateCollector collector)
        {
            this.collector = collector;
        }

        public static EnvironmentState FromStateSource<T>(T source) where T : IEnvironmentStateSource
        {
            EnvironmentState state = new EnvironmentState();
            state.SetCollectorState(new EnvironmentStateCollectorGeneric<T>(source));
            return state;
        }
    }

    public class EnvironmentStateCollectorGeneric<T> : EnvironmentStateCollector where T : IEnvironmentStateSource
    {
        private T source;

        public EnvironmentStateCollectorGeneric(T source) : base()
        {
            this.source = source;
        }
        public override bool GetState()
        {
            return source.EnvironmentStateValue;
        }
    }

    internal class EnvironmentStateCollectorWithFunc : EnvironmentStateCollector
    {
        private Func<bool> func;

        public EnvironmentStateCollectorWithFunc(Func<bool> func) : base()
        {
            this.func = func;
        }
        public override bool GetState()
        {
            return func();
        }
    }

    public abstract class EnvironmentStateCollector
    {
        public abstract bool GetState();
    }
}