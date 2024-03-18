using IziHardGames.UserControl.Abstractions.Lib;
using IziHardGames.UserControl.Abstractions.Lib.Environments;
using IziHardGames.UserControl.Abstractions.Lib.UserMods;

namespace IziHardGames.ForEcs.Avatar2d.ForUnity
{
	public class UserModeAvatar2dEcs : UserMode
	{
		public UserModeAvatar2dEcs(UserEnvironmentAbstract userEnvironment) : base(userEnvironment)
		{
		}
	}
    public class MarkerMoveDown : UserActionMove { public override void Execute() { } }
    public class MarkerMoveUp : UserActionMove { public override void Execute() { } }
    public class MarkerMoveLeft : UserActionMove { public override void Execute() { } }
    public class MarkerMoveRight : UserActionMove { public override void Execute() { } }
}
