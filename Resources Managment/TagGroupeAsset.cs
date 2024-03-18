using IziHardGames.Core;


namespace IziHardGames.ProjectResourceManagment
{
	/// <summary>
	/// Таг для гурппировки ассетов
	/// </summary>
	public class TagGroupeAsset : IUnique
	{
		public int Id { get => idGroupe; set => idGroupe = value; }
		public int idGroupe;
		/// <summary>
		/// <see cref="System.Type"/>
		/// </summary>
		public int idType;
		/// <summary>
		/// 0 - none
		/// 1 - sprit
		/// ...
		/// </summary>
		public int idTGroupAsset;
	}
}