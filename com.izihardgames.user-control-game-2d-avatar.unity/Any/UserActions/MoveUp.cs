
using IziHardGames.UserControl.Abstractions.NetStd21;

// using IziHardGames.UserControl.InputSystem.Unity;

namespace IziHardGames.UserControl.Game2d.Avatar.Unity.Moving
{
	/// <summary>
	/// North-East
	/// </summary>
	public class MoveRightUp : UserActionMove
    {
        public override void Execute()
        {
            ComponentAvatar2d.Current.MoveRight();
        }
    }
    /// <summary>
    /// North-West
    /// </summary>
    public class MoveLeftUp : UserActionMove
    {
        public override void Execute()
        {
            ComponentAvatar2d.Current.MoveLeftUp();
        }
    }
    /// <summary>
    /// North
    /// </summary>
    public class MoveUp : UserActionMove
    {

        public override void Execute()
        {
            ComponentAvatar2d.Current.MoveUp();
        }
    }
    /// <summary>
    /// South
    /// </summary>
    public class MoveDown : UserActionMove
    {
        public override void Execute()
        {
            ComponentAvatar2d.Current.MoveDown();
        }
    }
    /// <summary>
    /// South-West
    /// </summary>
    public class MoveLeftDown : UserActionMove
    {
        public override void Execute()
        {
            ComponentAvatar2d.Current.MoveLeftDown();
        }
    }
    /// <summary>
    /// South-East
    /// </summary>
    public class MoveRightDown : UserActionMove
    {
        public override void Execute()
        {
            ComponentAvatar2d.Current.MoveRightDown();
        }
    }

    /// <summary>
    /// East
    /// </summary>
    public class MoveLeft : UserActionMove
    {
        public override void Execute()
        {
            ComponentAvatar2d.Current.MoveLeft();

        }
    }
    /// <summary>
    /// West
    /// </summary>
    public class MoveRight : UserActionMove
    {
        public override void Execute()
        {
            ComponentAvatar2d.Current.MoveRight();
        }
    }
}
