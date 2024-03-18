using System;
using System.Collections.Generic;
using System.Linq;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;
using IziHardGames.UserControl.Abstractions.NetStd21.UserMods;
using IziHardGames.UserControl.NetStd21.Metas;

namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    public class UserControlMapAuthoring
    {
        private UserEnvironmentAbstract userEnv;
        private List<UserMode> userModes = new List<UserMode>();
        private List<UserAction> userActions = new List<UserAction>();
        private Dictionary<UserMode, UserModeControlAuthoring> controlsForUserModes = new Dictionary<UserMode, UserModeControlAuthoring>();
        private Dictionary<UserAction, UserActionControlAuthoring> controlsForUserActions = new Dictionary<UserAction, UserActionControlAuthoring>();
        private Dictionary<Type, EnvironmentState> states = new Dictionary<Type, EnvironmentState>();

        public IEnumerable<UserMode> UserModes => userModes;
        public IEnumerable<UserAction> UserActions => userActions;

        public UserControlMapAuthoring(UserEnvironmentAbstract userEnv)
        {
            this.userEnv = userEnv;
        }

        public UserActionControlAuthoring AddUserAction<T>() where T : UserAction, new()
        {
            T ua = UserActionDefaultFactory.Create<T>();
            userActions.Add(ua);
            return GetControlForUserAction<T>();
        }

        public UserModeControlAuthoring AddUserMode<T>() where T : UserMode, new()
        {
            T um = UserModeDefaultFactory.Create<T>(userEnv);
            userModes.Add(um);
            return GetControlForUserMode<T>();
        }

        public UserActionControlAuthoring GetControlForUserAction<T>() where T : UserAction
        {
            var action = userActions.First(x => x is T);
            if (!controlsForUserActions.TryGetValue(action, out var result))
            {
                result = new UserActionControlAuthoring(action);
                controlsForUserActions.Add(action, result);
            }
            return result;
        }
        public UserModeControlAuthoring GetControlForUserMode<T>() where T : UserMode
        {
            var mode = userModes.First(x => x is T);

            if (!controlsForUserModes.TryGetValue(mode, out var result))
            {
                result = new UserModeControlAuthoring(mode);
                controlsForUserModes.Add(mode, result);
            }
            return result;
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }

        public ActionMap GetActionMap()
        {
            ActionMap map = new ActionMap();

            foreach (var action in userActions)
            {
                map.AddUserAction(action);
            }
            return map;
        }

        public void AddState<T>() where T : IEnvironmentState
        {
            states.Add(typeof(T), new EnvironmentState());
        }
        public void AddState(int key)
        {
            throw new NotImplementedException();
        }

        public void AddAsSingletons(IIziAppBuilder builder)
        {
            foreach (var um in userModes)
            {
                builder.AddSingleton(um);
            }
        }
    }
}
