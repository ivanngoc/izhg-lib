using IziHardGames.Libs.NonEngine.Game.Abstractions;
using IziHardGames.Ticking.Abstractions.Lib;

namespace IziHardGames.Ticking.Lib
{
	public interface IUpdateGroupe
	{
		ITickChannel UpdateChannel { get; set; }
		//public void Regist(Action action);
		//public void RegistReverse(Action action);
	}
}