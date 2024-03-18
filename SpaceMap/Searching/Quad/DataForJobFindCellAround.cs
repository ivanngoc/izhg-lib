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
		/// ������� ������ ������
		/// </summary>
		public int radius;
		/// <summary>
		/// ������� ������ �� ������ ��������� �������
		/// </summary>
		public int indexAtRadius;
		/// <summary>
		/// ���������� ��������� � ������ ��������� �������
		/// </summary>
		public int countOfElementsAtRadius;
		/// <summary>
		/// ���������� ����������� ����� ������� ������
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
