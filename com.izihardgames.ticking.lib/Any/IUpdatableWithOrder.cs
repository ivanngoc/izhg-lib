using IziHardGames.Ticking.Lib;
using IziHardGames.Ticking.Lib;

namespace IziHardGames.Core
{
	public interface IUpdatableWithOrder : IUpdatable
	{
		UpdateControlToken UpdateOrder { get; set; }
	}
}