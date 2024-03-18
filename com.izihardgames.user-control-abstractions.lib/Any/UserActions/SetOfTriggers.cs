using System;
using System.Collections.Generic;
using System.Linq;

namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    public sealed class SetOfTriggers
    {
        protected List<TriggerSource> triggers;
        public IEnumerable<TriggerSource> Triggers => triggers;
        private bool isTriggered;
        public bool IsTriggered => isTriggered;

        internal SetOfTriggers()
        {

        }

        public void AddTrigger(TriggerSource source)
        {
            triggers.Add(source);
        }
        public void Calculate()
        {
            isTriggered = triggers.All(x => x.IsTriggered);
        }

        internal void Clean()
        {
            isTriggered = false;
            triggers.ForEach(x => x.Clean());
        }

        internal bool ContainsAll(IEnumerator<TriggerSource> triggers)
        {
            while (triggers.MoveNext())
            {
                var trigger = triggers.Current;
                if (!this.triggers.Contains(trigger)) return false;
            }
            return true;
        }

        internal void SetTrigger(TriggerSource trigger)
        {
            if (triggers != null) throw new InvalidOperationException($"This method can be used only if [field:{nameof(triggers)}] is null");
            triggers = new List<TriggerSource>() { trigger };
        }
        internal void SetTriggers(ref List<TriggerSource> triggers)
        {
            this.triggers = triggers;
            triggers = null;
        }
    }
}
