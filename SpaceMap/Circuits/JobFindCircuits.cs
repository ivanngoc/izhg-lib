using IziHardGames.Libs.NonEngine.Vectors;
using System;
using System.Buffers;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Graphs.QuadTile.Ground
{

	public class JobFindCircuits
	{
		public static DataGroundStatic dataGroundStatic;

		public bool isComplete;
		public bool isSolidCircuit;

		/// <summary>
		/// YXZ order. Sorted from left botton to right up
		/// </summary>
		public static Point3Int[] offsetsDLRU => SetOfCircuits.offsetsDLRU;

		private Point3Int[] positions;

		public void Initilize(Point3Int[] positions)
		{
			this.positions = positions;
		}

		public void Execute()
		{
			int newGroupeIndex = 1;
			int circuitsCount = default;
			/// associated with <see cref="positions"/>
			int[] rentGroupeIndexed = ArrayPool<int>.Shared.Rent(positions.Length);
			Span<int> spanGroupIndexes = new Span<int>(rentGroupeIndexed, 0, positions.Length);

			int[] rentFlagsNeighbour = ArrayPool<int>.Shared.Rent(positions.Length);
			Span<int> spanFlagsNeighbour = new Span<int>(rentFlagsNeighbour, 0, positions.Length);
			/// value is the index in <see cref="positions"/>. 
			int[] rentIndexesGroupes = ArrayPool<int>.Shared.Rent(positions.Length);


			//Memory[] rentMemories = ArrayPool<Memory<int>>.Shared.Rent(positions.Length);
			//Span spanMemories = new Span<Memory<int>>(rentMemories, 0, positions.Length);					

			spanGroupIndexes.Fill(default);
			spanFlagsNeighbour.Fill(default);

			for (int i = 0; i < positions.Length; i++)
			{
				for (int j = 0; j < offsetsDLRU.Length; j++)
				{
					Point3Int offset = positions[i] + offsetsDLRU[j];

					int indexPos = positions.IndexOf(offset);
					int indexExistedGroupe = spanGroupIndexes[indexPos];

					if (indexExistedGroupe > 0)
					{
						spanGroupIndexes[i] = indexExistedGroupe;
						spanFlagsNeighbour[i] = spanFlagsNeighbour[i] | (1 << j);
					}
					else
					{
						spanGroupIndexes[i] = newGroupeIndex;
						newGroupeIndex++;
					}
				}
			}

			for (int i = 0; i < positions.Length; i++)
			{
				///0000
				///URLD
				switch (spanFlagsNeighbour[i])
				{
					case 0b_0000: break;
					case 0b_0001: break;
					case 0b_0010: break;
					case 0b_0011: break;
					case 0b_0100: break;
					case 0b_0101: break;
					case 0b_0110: break;
					case 0b_0111: break;
					case 0b_1000: break;
					case 0b_1001: break;
					case 0b_1010: break;
					case 0b_1011: break;
					case 0b_1100: break;
					case 0b_1101: break;
					case 0b_1110: break;
					case 0b_1111: break;
					default: throw new ArgumentOutOfRangeException();
				}
			}

			ArrayPool<int>.Shared.Return(rentGroupeIndexed);
			ArrayPool<int>.Shared.Return(rentFlagsNeighbour);
			ArrayPool<int>.Shared.Return(rentIndexesGroupes);

			throw new NotImplementedException();
		}

		public static void FindForSetAndMerge(SetOfCircuits setOfCircuits, List<int> indexMaps)
		{
			var circuitsPerIndexMap = setOfCircuits.circuitsPerIndexMap;

			//bool[] rentFlags = ArrayPool<bool>.Shared.Rent(setOfCircuits.circuitsPerIndexMap.Length);
			//Span<bool> flagsSkipPerIndexMap = new Span<bool>(rentFlags, 0, setOfCircuits.circuitsPerIndexMap.Length);
			//flagsSkipPerIndexMap.Clear();

			for (int i = 0; i < indexMaps.Count; i++)
			{
				int indexMap = indexMaps[i];

				//if (flagsSkipPerIndexMap[indexMap]) continue;

				Circuit current = circuitsPerIndexMap[indexMap];

				for (int j = 0; j < offsetsDLRU.Length; j++)
				{
					Point3Int pos = offsetsDLRU[j] + dataGroundStatic.GetPositionAtTilemapByIndexMap(indexMap);

					if (dataGroundStatic.TryGetIndexMapByPos(pos, out int indexMapTemp))
					{
						Circuit neighbour = circuitsPerIndexMap[indexMapTemp];

						if (neighbour != null && current != neighbour && current.value == neighbour.value)
						{
							Circuit merged = setOfCircuits.Merge(current, neighbour);

							//foreach (var indexMapMerged in merged.associatedTileIndexes)
							//{
							//	flagsSkipPerIndexMap[indexMapMerged] = true;
							//}
						}
					}
				}
			}
			//ArrayPool<bool>.Shared.Return(rentFlags);
		}
	}//class
}//namespace
