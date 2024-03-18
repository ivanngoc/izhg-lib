using System;
using System.Collections.Generic;

namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    public class ActionMapScheme
    {

    }

    public class ActionMap
    {
        private readonly List<UserAction> userActions = new List<UserAction>();
        public IEnumerable<UserAction> UserActions => userActions;
        public void AddUserAction(UserAction userAction)
        {
            userActions.Add(userAction);
        }
        public void Validate()
        {
#if DEBUG
            foreach (var item in userActions)
            {
                if (item.activator == null) throw new InvalidProgramException($"Activator for userAction is Empte. {item.ToStringInfo()}");
            }
#endif
        }
    }
}