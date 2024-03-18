using IziHardGames.Libs.Engine.SpaceMap.Helpers;
using IziHardGames.Libs.NonEngine.Graphs.QuadTile.Ground;
using IziHardGames.Libs.NonEngine.Helpers;
using IziHardGames.Libs.NonEngine.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IziHardGames.Libs.Engine.SpaceMap
{

	[Serializable]
	public class DataGroundStaticEngine : DataGroundStatic
	{
		/// <summary>
		/// TODO: replace <see cref="ManagerTilemap.bounds"/> with this
		/// </summary>
		public Bounds mapBounds;
		public Camera camera;
		public Tilemap tilemap;

		/// <summary>
		/// <see cref="DataGroundStatic.pointsAtWorldByIndexMap"/> as <see cref="Vector3"/>
		/// </summary>
		[CenterPos] public Vector3[] vector3ByIndexMap;
		/// <summary>
		/// <see cref="DataGroundStatic.pointsAtTileMapByIndexMap"/> as <see cref="Vector3Int"/>
		/// </summary>
		public Vector3Int[] vector3IntsByIndexMap;


		/// <summary>
		/// <see cref="ManagerTilemap"/>
		/// </summary>
		/// <param name="tilemap"></param>
		public void MakePreset(UnityEngine.Tilemaps.Tilemap tilemap)
		{
			List<Vector3Int> vector3Ints = new List<Vector3Int>();
			List<int> indexes = new List<int>();

			tilemap.GetUsedCoordsXYZ(vector3Ints, indexes);

			Point3Int[] coords = vector3Ints.Select(x => x.ToPoint3Int()).ToArray();
			int countOfTilesInGameMap = coords.Length;

			base.MakePreset(coords);

			this.pointsAtWorldByIndexMap = new Point3[countOfTilesInGameMap];
			this.vector3ByIndexMap = new Vector3[countOfTilesInGameMap];
			this.vector3IntsByIndexMap = new Vector3Int[countOfTilesInGameMap];

			for (int i = 0; i < countOfTilesInGameMap; i++)
			{
				vector3IntsByIndexMap[i] = pointsAtTileMapByIndexMap[i].ToPoint3Int();
				vector3ByIndexMap[i] = tilemap.GetCellCenterWorld(vector3IntsByIndexMap[i]);
				pointsAtWorldByIndexMap[i] = vector3ByIndexMap[i].ToPoint3();
			}

			SetBounds(tilemap.cellBounds.min.ToPoint3Int(), tilemap.cellBounds.max.ToPoint3Int());
			SetOrigins(tilemap.origin.ToPoint3Int());
			SetTableSize(boundsDelta.x, boundsDelta.y, boundsDelta.z);

			countCellsUsed = pointsAtTileMapByIndexMap.Length;
			countCellsInRect = CountRows * CountColumns;

			var tableMin = boundsMin;
			var tableMax = new Point3Int(
					Mathf.Clamp(boundsMax.x - 1, tableMin.x, int.MaxValue),
					Mathf.Clamp(boundsMax.y - 1, tableMin.y, int.MaxValue),
					Mathf.Clamp(boundsMax.z - 1, tableMin.z, int.MaxValue));
			SetTableBounds(tableMin, tableMax);

			mapBounds = tilemap.localBounds;
			offsetToZero = Point3Int.zero - boundsMin;

			indexBoundByIndexMap = new int[countCellsUsed];
			flagsIsPartOfMapPerIndexBound = new bool[countCellsInRect];
			indexMapByIndexBound = new int[countCellsInRect];

			for (int i = 0; i < flagsIsPartOfMapPerIndexBound.Length; i++)
			{
				flagsIsPartOfMapPerIndexBound[i] = pointsAtTileMapByIndexMap.Contains(HelperVector3IntNonEngine.IndexToCoord2D(i, sizeTable3d, boundsMin), EqualityComparer<Point3Int>.Default);
			}
			for (int i = 0; i < indexBoundByIndexMap.Length; i++)
			{
				indexBoundByIndexMap[i] = PosToIndexBound(pointsAtTileMapByIndexMap[i]);
			}

			for (int i = 0; i < indexMapByIndexBound.Length; i++)
			{
				indexMapByIndexBound[i] = -1;
			}
			for (int i = 0; i < indexBoundByIndexMap.Length; i++)
			{
				indexMapByIndexBound[indexBoundByIndexMap[i]] = i;
			}
		}


		/// <summary>
		/// Позиция является частью карты и используется в тайлмапе.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool IsPartOfMap(Vector3Int pos)
		{
			if (IsInsideTable(pos))
			{
				int indexBound = PosToIndexBound(pos);
				return flagsIsPartOfMapPerIndexBound[indexBound];
			}
			return false;
		}
		public bool IsPartOfMap(Vector3Int pos, out int indexMap)
		{
			if (IsInsideTable(pos))
			{
				indexMap = PosToIndexMap(pos);
				return true;
			}
			indexMap = -1;
			return false;
		}
		public bool IsInsideTable(Vector3Int posTemp)
		{
			return HelpFuncsBoundsEngineForSurrogates.IsPointInsideRect3d(posTemp, tableMin, tableMax);
		}
		public bool IsSearchCanBeProceeded(Vector2Int searchAreaMin, Vector2Int searchAreaMax)
		{
			return HelpFuncsBoundsEngineForSurrogates.IsEdgeOfLeftRectIntersectWithRightRect(searchAreaMin, searchAreaMax, tableMin, tableMax);
		}





		public int PosToIndexMap(Vector3Int pos)
		{
			return indexMapByIndexBound[PosToIndexBound(pos)];
		}
		public int PosToIndexBound(Vector3Int pos)
		{
			return pos.ToCellIndex(sizeTable2d, boundsOrigin2d);
		}
		public bool TryPosToIndexMap(Vector3Int pos, out int index)
		{
			if (IsPartOfMap(pos))
			{
				index = PosToIndexMap(pos);

				return true;
			}

			index = -1;

			return false;
		}



		public Vector2 GetScreenPos(Vector3Int vector3Int)
		{
			return camera.WorldToScreenPoint(tilemap.GetCellCenterWorld(vector3Int));
		}


		/// https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/proposals/csharp-7.2/readonly-ref#declaring-ref-readonly-returning-members
		public ref readonly Vector3 GetVector3ByIndexMap(int indexMap)
		{
			return ref vector3ByIndexMap[indexMap];
		}
	}
}