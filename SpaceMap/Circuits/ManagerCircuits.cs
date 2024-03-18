using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Graphs.QuadTile.Ground
{
	public class ManagerCircuits
	{
		public DataGroundDynamic dataGroundDynamic;
		public DataGroundStatic dataGroundStatic;

		public readonly Dictionary<int, SetOfCircuits> setOfCircuitsById = new Dictionary<int, SetOfCircuits>(8);

		public void CreateCircuits(int[] ids, int sizeMap)
		{
			for (int i = 0; i < ids.Length; i++)
			{
				setOfCircuitsById.Add(ids[i], new SetOfCircuits(sizeMap, dataGroundStatic, dataGroundDynamic)
				{
					idSet = ids[i],
				});
			}
		}
		/// <summary>
		/// <inheritdoc cref="SetOfCircuits.Add(int, int)"/>
		/// </summary>
		/// <param name="indexSet"></param>
		/// <param name="value"></param>
		/// <param name="indexMap"></param>
		public void Add(int indexSet, int value, int indexMap)
		{
			setOfCircuitsById[indexSet].Add(value, indexMap);
		}
		public void Remove(int indexSet, int indexMap)
		{
			setOfCircuitsById[indexSet].Remove(indexMap);
		}
		/// <summary>
		/// <inheritdoc cref="SetOfCircuits.AddWithMerge(int, int)"/>
		/// </summary>
		/// <param name="indexSet"></param>
		/// <param name="value"></param>
		/// <param name="indexMap"></param>
		public void AddWithMerge(int indexSet, int value, int indexMap)
		{
			setOfCircuitsById[indexSet].AddWithMerge(value, indexMap);
		}
		public void RemoveWithBreak(int indexSet, int indexMap)
		{
			setOfCircuitsById[indexSet].RemoveWithBreak(indexMap);
		}
		public Circuit GetCircuit(int indexSet, int indexMap)
		{
			return setOfCircuitsById[indexSet].circuitsPerIndexMap[indexMap];
		}
	}
	//public readonly struct CircuiteGroupe
	//{
	//	public readonly int indexStart;
	//	public readonly int indexEnd;
	//	public readonly int idGroupe;
	//}
}//namespace
