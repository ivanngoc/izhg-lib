using IziHardGames.Libs.Engine.Memory;
using IziHardGames.Libs.NonEngine.Helpers;
using IziHardGames.Libs.NonEngine.SpaceMap.Searching.ForQuadTilemap;
using IziHardGames.Libs.NonEngine.Vectors;
using System;
using System.Buffers;
using System.Linq;


namespace IziHardGames.Libs.NonEngine.Graphs.QuadTile.Ground
{
	/// <summary>
	/// Predefined datas about space devided into cells.<br/>
	/// Bound - area in world space between <see cref="boundsMin"/> and <see cref="boundsMax"/>.<br/>
	/// indexBound - is index of specific position of that bound.<br/>
	/// Map - is area including only cells that treated as "USED" or "ACTIVE" inside Bound. Every cell outside Map area is not operated with.<br/>
	/// indexMap - is index of specific coordinate in array that represens map area (<see cref="pointsAtTileMapByIndexMap"/>).
	/// </summary>
	[Serializable]
	public class DataGroundStatic
	{
		/// <summary>
		/// Пример: Если в боксе 10*10 используется лишь 27 ячеек из 100 то значение будет = 27
		/// </summary>
		public int countCellsUsed;
		/// <summary>
		/// Равно размеру бокса. Если поле 10*10 то значение будет 100
		/// </summary>
		public int countCellsInRect;

		/// <summary>
		/// (Length) How many rows in table
		/// </summary>
		public int CountRows => sizeTable2d.x;
		/// <summary>
		/// (Hieght) How many columns in table 
		/// </summary>
		public int CountColumns => sizeTable2d.y;
		/// <summary>
		/// (Depth) How many elements by Z axis in table. 
		/// </summary>
		public int CountZ => sizeTable3d.z;

		public int BiggerSideCount => sizeTable2d.x >= sizeTable2d.y ? sizeTable2d.x : sizeTable2d.y;

		/// <summary>
		/// Size of playground (width/height/depth).
		/// </summary>
		public Point3Int sizeTable3d;
		public Point2Int sizeTable2d;

		#region Bounds		
		/// <summary>
		/// <see cref="ManagerTilemap.correspondMapIndex"/>
		/// if use indexBound as index of array than gained value will be indexMap 
		/// </summary>
		public int[] indexMapByIndexBound;
		/// <summary>
		/// if use indexMap as index of array than gaine value will be indexBound
		/// </summary>
		public int[] indexBoundByIndexMap;
		/// <summary>
		/// <see cref="ManagerTilemap.flagsIsPartOfMap"/>
		/// Per Index Bound
		/// </summary>
		public bool[] flagsIsPartOfMapPerIndexBound;
		#endregion

		/// <summary>
		/// Value to add (+) to another value to gain (0,0,0) from <see cref="boundsOrigin3d"/>
		/// </summary>
		public Point3Int offsetToZero;

		/// <summary>
		/// World Space Coordinate of (0,0)-Left,Bot cornter of tilemap. 
		/// usualy equal to <see cref="boundsMin"/>
		/// </summary>
		[CenterPos] public Point2Int boundsOrigin2d;
		/// <summary>
		/// World Space Coordinate of (0,0,0)-Left,Bot,Front cornter of tilemap. 
		/// usualy equal to <see cref="boundsMin"/>
		/// </summary>
		[CenterPos] public Point3Int boundsOrigin3d;
		/// <summary>
		/// Left,Bot,Front Corner of Bound calculated from <see cref="pointsAtTileMapByIndexMap"/><br/>
		/// </summary>
		[GridPos] public Point3Int boundsMin;
		/// <summary>
		/// Right,Top,Back Corner of Bound calculated from <see cref="pointsAtTileMapByIndexMap"/><br/>
		/// Grid-position (intersect-position)
		/// </summary>
		[GridPos] public Point3Int boundsMax;
		/// <summary>
		/// <see cref="boundsMax"/> - <see cref="boundsMin"/><br/>
		/// Grid-position (intersect-position)
		/// </summary>
		[GridPos] public Point3Int boundsDelta;

		/// <summary>
		/// Cell With minimum coordinated in bound.
		/// Adress-Position of minimum in cell coordinate. Center-positon (unlike grid-position which position represents intersect).
		/// </summary>
		[CenterPos] public Point3Int tableMin;
		/// <summary>
		/// Cell With maximum coordinated in bound.
		/// Adress-Position of maximum in cell coordinate. Center-position (unlike grid-position which position represents intersect).
		/// </summary>
		[CenterPos] public Point3Int tableMax;


		#region Active Tiles (Free Size)
		/// <summary>
		/// Represents of area in bound between <see cref="boundsMin"/> and <see cref="boundsMax"/>. 
		/// Each cell is treated as "active"/"in use". Rest cells in bound is not operated with. <br/>
		/// Center-position
		/// </summary>
		public Point3Int[] pointsAtTileMapByIndexMap;
		/// <summary>
		/// Correspond position of each cell's center of <see cref="pointsAtTileMapByIndexMap"/> in space (world/local as wished).<br/>
		/// Center-position
		/// </summary>
		public Point3[] pointsAtWorldByIndexMap;

		#endregion

		public void MakePreset(Point3Int[] array)
		{
			pointsAtTileMapByIndexMap = array.ToArray();
		}

		public void SetBounds(Point3Int min, Point3Int max)
		{
			boundsMin = min;
			boundsMax = max;
			boundsDelta = boundsMax - boundsMin;
		}

		public void SetOrigins(Point3Int origin)
		{
			boundsOrigin3d = origin;
			boundsOrigin2d = boundsOrigin3d;
		}

		public void SetTableSize(int x, int y, int z)
		{
			sizeTable3d = new Point3Int(x, y, z);
			sizeTable2d = sizeTable3d;
		}
		public void SetTableBounds(Point3Int tableMin, Point3Int tableMax)
		{
			this.tableMin = tableMin;
			this.tableMax = tableMax;
		}

		#region MyRegion
		public int FindIndexMap(Point3Int position)
		{
			for (int i = 0; i < pointsAtTileMapByIndexMap.Length; i++)
			{
				if (pointsAtTileMapByIndexMap[i] == position) return i;
			}
			throw new Exception($"Не удалост получить индекс координаты {position}.");
		}
		#endregion

		#region Converters
		public Point3 GetPositionAtWorldByIndexMap(int indexMap)
		{
			return pointsAtWorldByIndexMap[indexMap];
		}
		public Point3Int GetPositionAtTilemapByIndexMap(int indexMap)
		{
			return pointsAtTileMapByIndexMap[indexMap];
		}
		public bool TryGetIndexMapByPos(Point3Int pos, out int indexMap)
		{
			if (IsPartOfMapOrigin(pos))
			{
				indexMap = GetIndexMapByPos(pos);
				return true;
			}
			indexMap = -1;
			return false;
		}
		public int GetIndexMapByPos(Point3Int pos)
		{
			int indexBound = GetIndexBoundByPos(pos);
			return indexMapByIndexBound[indexBound];
		}
		public int GetIndexBoundByPos(Point3Int pos)
		{
			return pos.ToCellIndexOrigin(sizeTable2d, boundsOrigin2d);
		}

		public int PosToIndexMap(Point3Int pos)
		{
			return indexMapByIndexBound[PosToIndexBound(pos)];
		}
		public int PosToIndexBound(Point3Int pos)
		{
			return pos.ToCellIndex(sizeTable2d, boundsOrigin2d);
		}
		public Point3 PositionInTilemapToPositionInWorld(Point3Int pos)
		{
			return pointsAtWorldByIndexMap[PosToIndexMap(pos)];
		}

		public int IndexMapToIndexBound(int indexMap)
		{
			return indexBoundByIndexMap[indexMap];
		}
		public int IndexBoundToIndexMap(int indexBound)
		{
			return indexMapByIndexBound[indexBound];
		}
		#endregion

		#region Checkers
		/// <summary>
		/// Позиция является частью карты и используется в тайлмапе.
		/// </summary>
		/// <param name="posTemp"></param>
		/// <returns></returns>
		public bool IsPartOfMapOrigin(Point3Int posTemp)
		{
			if (IsInsideTableOrigin(posTemp))
			{
				int indexBound = GetIndexBoundByPos(posTemp);

				return flagsIsPartOfMapPerIndexBound[indexBound];
			}
			return false;
		}
		public bool IsInsideTableOrigin(Point3Int posTemp)
		{
			return HelpFuncsBoundsNonEngine.IsPointInsideRect(posTemp, (Point2Int)tableMin, (Point2Int)tableMax);
		}

		/// <summary>
		/// Позиция является частью карты и используется в тайлмапе.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool IsPartOfMap(Point2Int pos)
		{
			if (IsInsideTable(pos))
			{
				int indexBound = PosToIndexBound(pos);
				return flagsIsPartOfMapPerIndexBound[indexBound];
			}
			return false;
		}
		/// <summary>
		/// Позиция является частью карты и используется в тайлмапе.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool IsPartOfMap(Point3Int pos)
		{
			if (IsInsideTable(pos))
			{
				int indexBound = PosToIndexBound(pos);
				return flagsIsPartOfMapPerIndexBound[indexBound];
			}
			return false;
		}
		public bool IsPartOfMap(Point3Int pos, out int indexMap)
		{
			if (IsInsideTable(pos))
			{
				int indexBound = PosToIndexBound(pos);
				if (flagsIsPartOfMapPerIndexBound[indexBound])
				{
					indexMap = IndexBoundToIndexMap(indexBound);
					return true;
				}
			}
			indexMap = -1;
			return false;
		}
		public bool IsInsideTable(Point3Int point)
		{
			return HelpFuncsBoundsNonEngine.IsPointInsideRect(point, tableMin, tableMax);
		}
		#endregion
	}
}//namespace
