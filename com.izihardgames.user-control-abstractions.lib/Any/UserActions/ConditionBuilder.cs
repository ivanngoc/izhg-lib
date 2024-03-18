using System;
using System.Collections.Generic;
using System.Linq;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;
using IziHardGames.UserControl.Abstractions.NetStd21.UserMods;

namespace IziHardGames.UserControl.Abstractions.NetStd21.Builders
{
    public static class ConditionBuilder
    {
        public static ConditionBuilderMonada CreateFor(UserEnvironmentAbstract env)
        {
            ConditionBuilderMonada monada = new ConditionBuilderMonada();
            monada.Set = new SetOfEnvironmentConditions(env);
            return monada;
        }
    }

    public class ConditionBuilderMonada
    {
        public SetOfEnvironmentConditions Set { get => set; set => set = value; }
        private SetOfEnvironmentConditions set;

        internal ConditionBuilderMonada()
        {
        }
        public ConditionBuilderMonada IsActive<T>() where T : UserMode
        {
            EnvironmentCondition condition = new EnvironmentCondition(typeof(T), true);
            set.AddCondition(condition);
            return this;
        }

        internal void ImplementConditions<T>(T userAction) where T : UserAction, new()
        {
            userAction.activator.AddCondition(Set);
        }
    }

    public class SetOfEnvironmentConditions
    {
        private UserEnvironmentAbstract userEnvironment;
        private readonly List<EnvironmentCondition> conditions = new List<EnvironmentCondition>();
        private bool isFullfield;

        public UserEnvironmentAbstract Environment => userEnvironment;
        public bool IsFullfield => isFullfield;
        public SetOfEnvironmentConditions(UserEnvironmentAbstract userEnvironment)
        {
            this.userEnvironment = userEnvironment;
        }

        public void Collect()
        {
            foreach (var condition in conditions)
            {
                condition.Check(userEnvironment);
            }
        }
        public void Calculate()
        {
            isFullfield = conditions.All(x => x.isPass);
        }
        public void AddCondition(EnvironmentCondition condition)
        {
            conditions.Add(condition);
        }
    }
    public class EnvironmentCondition
    {
        public Type key;
        public bool value;
        public bool isPass;

        public EnvironmentCondition(Type key, bool value)
        {
            this.key = key;
            this.value = value;
        }
        internal void Check(UserEnvironmentAbstract environment)
        {
            isPass = environment[key].State == value;
        }
    }
}
