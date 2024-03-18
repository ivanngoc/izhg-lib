using System;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;

namespace IziHardGames.UserControl.Abstractions.NetStd21.Builders
{
    public static class UserActionBuilder
    {
        public static readonly UserActionBuilderSelector Select = new UserActionBuilderSelector();
        public static readonly UserActionBuilderByDefault Default = new UserActionBuilderByDefault();
    }

    public sealed class UserActionBuildMonada
    {
        internal UserAction userAction;
        internal ActionMap actionMap;

        public UserActionBuildMonada TriggerBy(TriggersBuilderMonada triggersBuilder)
        {
            var sets = triggersBuilder.sets;
            foreach (var set in sets)
            {
                userAction.activator.AddSet(set);
            }
            return this;
        }
    }

    public class UserActionBuilderByDefault : IUserActionBuilder
    {
        private ConditionBuilderMonada currentCondition;
        public UserActionBuildMonada Create<T>(ActionMap actionMap, Func<T> factory) where T : UserAction
        {
            throw new System.NotImplementedException();
        }
        public UserActionBuildMonada Create<T>(ActionMap actionMap) where T : UserAction, new()
        {
            var environment = IziEnvironment.Environments.Current;
            UserActionBuildMonada monada = new UserActionBuildMonada();
            var userAction = new T();
            userAction.SetUserEnvironment(environment);
            monada.userAction = userAction;
            monada.actionMap = actionMap;
            if (currentCondition != null) currentCondition.ImplementConditions(userAction);
            actionMap.AddUserAction(userAction);
            return monada;
        }

        public void Do(Action<ConditionBuilderMonada> value)
        {
            value.Invoke(currentCondition);
            Reset();
        }

        private void Reset()
        {
            this.currentCondition = null;
        }

        public UserActionBuilderByDefault When(ConditionBuilderMonada condition)
        {
            this.currentCondition = condition;
            return this;
        }
    }
    public class UserActionBuilderSelector
    {
        public IUserActionBuilder this[Type type] => throw new System.NotImplementedException();
    }
}
