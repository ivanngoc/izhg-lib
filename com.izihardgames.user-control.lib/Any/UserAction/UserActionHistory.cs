using System;
using System.Collections.Generic;
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;

namespace IziHardGames.UserControl.Lib
{
    /// <summary>
    /// Or User Control History?
    /// </summary>
    public class UserActionHistory : IUserActionHistory, IDisposable
    {
        private readonly Queue<Activation>? activations = new Queue<Activation>();
        public static UserActionHistory? Default { get; set; }
        private UserEnvironmentAbstract? userEnvironment;

        public void Initilize(UserEnvironmentAbstract env)
        {
            userEnvironment = env;
        }

        public void Push(UserAction userAction)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}