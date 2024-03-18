using System;
using System.Collections.Generic;
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.Abstractions.NetStd21.Contexts;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;

namespace IziHardGames.UserControl.Lib.UserActions
{
    public class ContextForUserActionsV2 : UserContext
    {
        private readonly UserActionHistory userActionHistory = new UserActionHistory();
        private readonly List<ActionMap> actionMaps = new List<ActionMap>();

        public void Initilize(UserEnvironmentAbstract userEnvironment)
        {
            userActionHistory.Initilize(userEnvironment);
        }
        protected override void Grab()
        {
            base.Grab();
            var triggers = RegistryForTriggers.Triggersss;

            foreach (var trigger in triggers)
            {
                trigger.Collect();
            }
        }
        protected override void Filter()
        {
            base.Filter();

            var triggers = RegistryForTriggers.Triggersss;

            foreach (var trigger in triggers)
            {
                trigger.Calculate();
            }

            var sets = RegistryForSetOfTriggers.Sets.GetSets();
            foreach (var set in sets)
            {
                set.Calculate();
            }

            foreach (var actionMap in actionMaps)
            {
                foreach (var action in actionMap.UserActions)
                {
                    action.activator.ChekTriggers();
                }
            }
            foreach (var actionMap in actionMaps)
            {
                foreach (var action in actionMap.UserActions)
                {
                    if (action.activator.IsTriggered)
                    {
                        action.activator.ChekConditions();
                    }
                }
            }
        }

        protected override void Execute()
        {
            base.Execute();

            foreach (var map in actionMaps)
            {
                foreach (var userAction in map.UserActions)
                {
                    if (userAction.activator.IsFired)
                    {
                        userAction.Execute();
                        userAction.AddFlag(EUserActionInvokationFlags.ExecutedInUiLoop);
                    }
                }
            }
        }
        public override void Clean()
        {
            base.Clean();

            foreach (var map in actionMaps)
            {
                foreach (var action in map.UserActions)
                {
                    action.Clean();
                }
            }
        }
        public void AddActionMap(ActionMap actionMap)
        {
            if (actionMaps.Contains(actionMap)) throw new ArgumentException("Action map is already presented");
            actionMaps.Add(actionMap);
        }

        public bool IsFired<T>() where T : UserAction
        {
            foreach (var map in actionMaps)
            {
                foreach (var action in map.UserActions)
                {
                    if (action is T target) return target.activator.IsFired;
                }
            }
            throw new System.NullReferenceException($"User Action typeof [{typeof(T)}] is not founded");
        }

        internal void FillHistory()
        {
            foreach (var item in actionMaps)
            {
                foreach (var ua in item.UserActions)
                {
                    if (ua.activator.IsFired)
                    {
                        userActionHistory.Push(ua);
                    }
                }
            }
        }
    }
}