using System;
using System.Collections.Generic;
using System.Linq;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Attributes;
using IziHardGames.UserControl.Abstractions.NetStd21.Attributes;

namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    public static class RegistryForSetOfTriggers
    {
        public static object Concurrent => throw new System.NotImplementedException();
        public static RegistryForSetOfTriggersAbstract Sets { get; set; }
        public static RegistryForSetOfTriggersDefault Default => Sets.AsDef();
    }

    [Registry]
    public abstract class RegistryForSetOfTriggersAbstract
    {
        internal abstract SetOfTriggers GetOrCreate(ref List<TriggerSource> triggers);
        public RegistryForSetOfTriggersDefault AsDef() => this as RegistryForSetOfTriggersDefault ?? throw new InvalidCastException($"Current type:{GetType().FullName}; targetType:{typeof(RegistryForSetOfTriggersDefault).FullName}");
        public T As<T>() where T : RegistryForSetOfTriggersAbstract
        {
            return this as T ?? throw new InvalidCastException($"Current type:{GetType().FullName}; targetType:{typeof(T).FullName}");
        }

        public abstract IEnumerable<SetOfTriggers> GetSets();
    }
    public class RegistryForSetOfTriggersDefault : RegistryForSetOfTriggersAbstract
    {
        /// <summary>
        /// ? ???????? ????? ????? ??????????????:<br/>
        /// - <br/>
        /// - <br/>
        /// </summary>
        private readonly Dictionary<int, SetOfTriggers> sets = new Dictionary<int, SetOfTriggers>();
        public IEnumerable<SetOfTriggers> Sets => sets.Values;
        private int counter;

        internal SetOfTriggers Get(int id)
        {
            return sets[id];
        }
        internal SetOfTriggers GetOrCreate(TriggerSource trigger)
        {
            var existed = sets.Values.First(x => x.Triggers.Count() == 1 && x.Triggers.First() == trigger);
            if (existed == null)
            {
                return Create(trigger);
            }
            return existed;
        }
        internal SetOfTriggers GetOrCreate(TriggerSource trigger0, TriggerSource trigger1)
        {
            throw new System.NotImplementedException();
        }
        internal override SetOfTriggers GetOrCreate(ref List<TriggerSource> triggers)
        {
            foreach (var set in sets.Values)
            {
                if (set.ContainsAll(triggers.GetEnumerator())) return set;
            }
            return Create(ref triggers);
        }

        internal SetOfTriggers Create(TriggerSource trigger)
        {
            counter++;
            var newSet = new SetOfTriggers();
            newSet.SetTrigger(trigger);
            return newSet;
        }
        internal SetOfTriggers Create(ref List<TriggerSource> triggers)
        {
            counter++;
            var newSet = new SetOfTriggers();
            newSet.SetTriggers(ref triggers);
            sets.Add(counter, newSet);
            return newSet;
        }

		public override IEnumerable<SetOfTriggers> GetSets()
		{
            return sets.Values;
		}
	}

    public static class ExtensionsForIziAppBuilderAsRegistryForSetOfTriggers
    {
        [ExtendApp(EUseType.Independent)]
        public static void UseRegistryForSetOfTriggersDefault(this IIziAppBuilder builder)
        {
            var reg = new RegistryForSetOfTriggersDefault();
            RegistryForSetOfTriggers.Sets = reg;
            builder.AddSingleton<RegistryForSetOfTriggersAbstract>(reg);
        }
    }
}
