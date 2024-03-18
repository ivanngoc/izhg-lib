using System;
using IziHardGames.Libs.NonEngine.Vectors;

namespace IziHardGames.Libs.Engine.Components.NonMono
{
	/// <summary>
	/// Position of specific cell of the grid (Tilemap)
	/// </summary>
	[Serializable]
	public class DataPositionTile
	{
		public int id;
		/// <summary>
		/// Center worldspace
		/// </summary>
		public Point3 atWorld;
		/// <summary>
		/// Center localSpace
		/// </summary>
		public Point3 atLocal;
		/// <summary>
		/// Coordinate relative to tilemap
		/// </summary>
		public Point3Int atTilemap;

		public DataPositionTile()
		{
			
		}
	}
}