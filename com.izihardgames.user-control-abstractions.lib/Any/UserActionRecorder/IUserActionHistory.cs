using System.Collections.Generic;
using IziHardGames.Collections.Abstractions.NetStd21;

namespace IziHardGames.UserControl.Abstractions.NetStd21
{   
    public interface IUserActionHistory
    {
      
    }

    public struct Activation
    {
        public uint tick;
        public EUserActionCompletionStatus status;
    }
}