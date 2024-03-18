using System;
using Vector2Int = IziHardGames.Libs.NonEngine.Vectors.Point2Int;
using Vector3Int = IziHardGames.Libs.NonEngine.Vectors.Point3Int;

namespace IziHardGames.Libs.NonEngine.SpaceMap.Searching.ForQuadTilemap
{

	public struct DataForJobFindCellAround
	{
		public int minX;
		public int minY;
		public int maxX;
		public int maxY;

		/// <summary>
		/// текущий радиус поиска
		/// </summary>
		public int radius;
		/// <summary>
		/// текущий индекс на кольце заданного радиуса
		/// </summary>
		public int indexAtRadius;
		/// <summary>
		/// количество элементов в кольце заданного радиуса
		/// </summary>
		public int countOfElementsAtRadius;
		/// <summary>
		/// количество засчитанных €чеек текущей работы
		/// </summary>
		public int scannedMapCellCount;

		public void SetCenter(Vector2Int center)
		{
			minX = center.x;
			minY = center.y;

			maxX = center.x;
			maxY = center.y;
		}

		public bool isSearchProcceed;

		public void ExpandAreaByOne()
		{
			minX--;
			minY--;
			maxX++;
			maxY++;
			radius++;
		}
	}
}
