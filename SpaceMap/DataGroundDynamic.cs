using IziHardGames.Libs.NonEngine.Vectors;
using System;

namespace IziHardGames.Libs.NonEngine.Graphs.QuadTile.Ground
{
	[Serializable]
	public class DataGroundDynamic
	{
		/// <summary>
		/// <see langword="true"/> - cell by index map is detectable
		/// </summary>
		public bool[] flagsDetectionByIndexMap;
		/// <summary>
		/// <see cref="ManagerTilemap.TakenFlags"/>
		/// </summary>
		public bool[] flagsTakenPerIndexMap;
		/// <summary>
		/// <see langword="true"/> - cell is preserved, but not taken <br/>
		/// <see langword="false"/> - cell is not preserved. taken status any 
		/// </summary>
		public bool[] flagsPreservedPositionsPerInedexMap;

		protected DataGroundStatic dataGroundStatic;

		public void Initilize(DataGroundStatic dataGroundStatic)
		{
			this.dataGroundStatic = dataGroundStatic;
		}

		public virtual void Clean()
		{
			flagsPreservedPositionsPerInedexMap.Clear();
			flagsDetectionByIndexMap.Clear();
			flagsTakenPerIndexMap.Clear();
		}
		/// <summary>
		/// Cell is treated as "Active" and "Usable"
		/// </summary>
		/// <param name="indexMap"></param>
		/// <returns>
		/// <see langword="true"/>- information can be recived from this cell
		/// </returns>
		public bool IsDetectable(int indexMap)
		{
			return !flagsDetectionByIndexMap[indexMap];
		}

		public bool IsAwailableToSet(Point3Int positionAtTilemap, out int indexMap)
		{
			if (dataGroundStatic.IsPartOfMap(positionAtTilemap, out indexMap))
			{
				return !flagsTakenPerIndexMap[indexMap] && !flagsPreservedPositionsPerInedexMap[indexMap];
			}
			return false;
		}

		public virtual void PreAlloc(DataGroundStatic dataGroundStatic)
		{
			int countIndexMap = dataGroundStatic.countCellsUsed;

			flagsTakenPerIndexMap = new bool[countIndexMap];
			flagsPreservedPositionsPerInedexMap = new bool[countIndexMap];
			flagsDetectionByIndexMap = new bool[countIndexMap];
		}
	}
}//namespace
