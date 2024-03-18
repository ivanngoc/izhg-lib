using System.Collections.Generic;
using System.Linq;
using IziHardGames.UserControl.Abstractions.NetStd21.Builders;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;


namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    /// <summary>
    /// Активатор для <see cref="IUserAction"/>. Может быть закреплен за несколькимим <see cref="IUserAction"/> для одновременного срабатывания
    /// </summary>
    public sealed class UserActionActivator
    {
        private readonly List<SetOfTriggers> sets = new List<SetOfTriggers>();
        private readonly List<SetOfEnvironmentConditions> conditions = new List<SetOfEnvironmentConditions>();

        private UserEnvironmentAbstract environment;

        /// <summary>
        /// Сколько источников было активировано в этом кадре
        /// </summary>
        private int countTriggeredSourceAtFrame;
        /// <summary>
        /// Сколько кадров подряд была активация хотя бы одного триггера
        /// </summary>
        private int streakActive;
        /// <summary>
        /// Сколько кадров подряд не было активаций ниодного триггера
        /// </summary>
        private int streakInactive;
        public bool IsFired { get => isTriggered && isConditionFullfiled; }
        public bool IsTriggered { get => isTriggered; }

        private bool isTriggered;
        private bool isConditionFullfiled;

        internal void BindEnvironemnt(UserEnvironmentAbstract environment)
        {
            this.environment = environment;
        }

        public void ChekConditions()
        {
            conditions.ForEach(x => x.Collect());
            conditions.ForEach(x => x.Calculate());

            if (this.conditions.Count > 0)
            {
                this.isConditionFullfiled = conditions.Any(x => x.IsFullfield);
            }
            else
            {
                isConditionFullfiled = true;
            }
        }
        public void ChekTriggers()
        {

            isTriggered = sets.Any(x => x.IsTriggered);
        }

        public void Clean()
        {
            isTriggered = false;
            sets.ForEach(x => x.Clean());
        }

        internal void AddCondition(SetOfEnvironmentConditions set)
        {
            conditions.Add(set);
        }
        internal void AddSet(SetOfTriggers set)
        {
            sets.Add(set);
        }
        internal void AddSet(ref List<TriggerSource> triggers)
        {
            var set = RegistryForSetOfTriggers.Sets.GetOrCreate(ref triggers);
            sets.Add(set);
        }
    }
}
