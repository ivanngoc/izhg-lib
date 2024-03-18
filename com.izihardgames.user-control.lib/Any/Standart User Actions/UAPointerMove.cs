using System;
using IziHardGames.UserControl.Abstractions.NetStd21;

namespace IziHardGames.UserControl.NetStd21.UserActions
{
    public class UAPointerMove : UserAction, IUserActionRecordable<PointerDataForHistory>
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public PointerDataForHistory GetRecord()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Normalized or Absolute coordinates
    /// </summary>
    public struct PointerDataForHistory : IUserActionDataForHistory
    {
        public float x;
        public float y;
    }
}
