namespace IziHardGames.Ticking.Lib
{
	/// <summary>
	/// Объект который передает <see cref="UpdateService{TDataProvider}"/> объекты для регистрации
	/// </summary>
	public interface IUpdateRegistrator
	{
		UpdateStep Push();
	}

}