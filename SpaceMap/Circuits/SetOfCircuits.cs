using IziHardGames.Libs.NonEngine.Vectors;
using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Graphs.QuadTile.Ground
{
	/// <summary>
	/// Набор из контурных групп. 
	/// </summary>
	public class SetOfCircuits
	{
		public DataGroundStatic dataGroundStatic;
		public DataGroundDynamic dataGroundDynamic;
		/// <summary>
		/// YXZ order. Sorted from left botton to right up
		/// </summary>
		public static readonly Point3Int[] offsetsDLRU = new Point3Int[]
		{
			new Point3Int( 0, -1, 0),//down
			new Point3Int(-1, 0,  0),//left
			new Point3Int( 1, 0,  0),//right
			new Point3Int( 0, 1,  0),//up
		};
		public int idSet;
		public readonly Circuit[] circuitsPerIndexMap;
		public readonly Dictionary<int, Circuit> circuitsPerValue = new Dictionary<int, Circuit>(64);

		private Circuit temp0;
		private Circuit temp1;
		private Circuit temp2;
		private Circuit temp3;

		public const int DOWN = 0;
		public const int LEFT = 1;
		public const int RIGHT = 2;
		public const int UP = 3;

		private Circuit this[int index]
		{
			get
			{
				switch (index)
				{
					case DOWN: return temp0;
					case LEFT: return temp1;
					case RIGHT: return temp2;
					case UP: return temp3;
					default: throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (index)
				{
					case DOWN: temp0 = value; break;
					case LEFT: temp1 = value; break;
					case RIGHT: temp2 = value; break;
					case UP: temp3 = value; break;
					default: throw new ArgumentOutOfRangeException();
				}
			}
		}

		public SetOfCircuits(int sizeMap, DataGroundStatic dataGroundStatic, DataGroundDynamic dataGroundDynamic)
		{
			this.dataGroundStatic = dataGroundStatic;
			this.dataGroundDynamic = dataGroundDynamic;
			circuitsPerIndexMap = new Circuit[sizeMap];
		}

		#region Merge 
		public Circuit Merge(int mergeCenter) => throw new NotImplementedException();
		/// <summary>
		/// Inherit by left argument
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public Circuit Merge(Circuit a, Circuit b)
		{
			Circuit circuitNew = Circuit.Rent();
			//circuitNew.idCircuit = a.idCircuit;
			circuitNew.value = a.value;

			circuitNew.associatedTileIndexes.AddRange(a.associatedTileIndexes);
			circuitNew.associatedTileIndexes.AddRange(b.associatedTileIndexes);

			a.ReturnToPool();
			b.ReturnToPool();

			foreach (var indexMap in circuitNew.associatedTileIndexes)
			{
				circuitsPerIndexMap[indexMap] = circuitNew;
			}
			return circuitNew;
		}
		public Circuit Merge(Circuit a, Circuit b, Circuit c) => throw new NotImplementedException();
		public Circuit Merge(Circuit a, Circuit b, Circuit c, Circuit d) => throw new NotImplementedException();
		#endregion

		#region Break
		public void Break(int indexBreakCenter, Circuit circuit)
		{
			throw new NotImplementedException();
		}
		public void Break(Circuit circuit)
		{
			foreach (var indexMap in circuit.associatedTileIndexes)
			{   // fill breaked circuit with new single circuits
				circuitsPerIndexMap[indexMap] = Circuit.Rent(circuit.value, indexMap);
			}
			JobFindCircuits.FindForSetAndMerge(this, circuit.associatedTileIndexes);

			circuit.ReturnToPool();
		}
		#endregion
		/// <summary>
		/// Add to circute with same value, despite gaps. 1 circuit per each unique value 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="indexMap"></param>
		public void Add(int value, int indexMap)
		{
			if (circuitsPerValue.TryGetValue(value, out Circuit circuit))
			{
				circuit.Add(indexMap);
				circuitsPerIndexMap[indexMap] = circuit;
			}
			else
			{
				circuitsPerIndexMap[indexMap] = Circuit.Rent(value, indexMap);
				circuitsPerValue.Add(value, circuitsPerIndexMap[indexMap]);
			}
		}
		/// <summary>
		/// reverse of <see cref="Add(int, int)"/>
		/// </summary>
		/// <param name="indexMap"></param>
		public void Remove(int indexMap)
		{
			var v = circuitsPerIndexMap[indexMap];
			circuitsPerIndexMap[indexMap] = default;
			v.Remove(indexMap);

			int value = v.value;

			if (v.CheckDispose())
			{
				circuitsPerValue.Remove(value);
			}
		}

		/// <summary>
		/// Add to circute with same value sticked to given index map, or create new circuit if there is no neighbour around. 
		/// Multiple circuit on map with samve value if there is gaps beetwen them
		/// </summary>
		/// <param name="value"><see cref="Circuit.value"/></param>
		/// <param name="indexMap">index int <see cref="circuitsPerIndexMap"/></param>
		public void AddWithMerge(int value, int indexMap)
		{
#if UNITY_EDITOR
			//Console.Log($"AddWithMerge value {value} indexMap {indexMap}");
			if (circuitsPerIndexMap[indexMap] != null) throw new ArgumentException($"indexMap [{indexMap}] is taken but call to add");
#endif
			ClearTemps();

			int countFoundedNeighbours = default;

			for (int i = 0; i < offsetsDLRU.Length; i++)
			{
				Point3Int pos = offsetsDLRU[i] + dataGroundStatic.GetPositionAtTilemapByIndexMap(indexMap);

				if (dataGroundStatic.TryGetIndexMapByPos(pos, out int indexMapTemp))
				{
					var circuit = circuitsPerIndexMap[indexMapTemp];

					if (circuit != null && circuit.value == value)
					{
						if (!TempContains(circuit))
						{
							this[countFoundedNeighbours] = circuit;
							countFoundedNeighbours++;
						}
					}
				}
			}

			if (countFoundedNeighbours > 0)
			{
				Circuit circuitToInsert = default;

				switch (countFoundedNeighbours)
				{
					case 1:
						{
							circuitToInsert = this[0];
							this[0].Add(indexMap);
							break;
						}
					case 2:
						{
							circuitToInsert = Merge(this[0], this[1]);
							circuitToInsert.Add(indexMap);
							break;
						}
					case 3:
						{
							circuitToInsert = Merge(this[0], this[1], this[2]);
							circuitToInsert.Add(indexMap);
							break;
						}
					case 4:
						{
							circuitToInsert = Merge(this[0], this[1], this[2], this[3]);
							circuitToInsert.Add(indexMap);
							break;
						}
					default: throw new ArgumentOutOfRangeException($"{countFoundedNeighbours}");
				}

				circuitsPerIndexMap[indexMap] = circuitToInsert;
			}
			else
			{
				Circuit circuit = Circuit.Rent();
				circuit.value = value;
				circuit.Add(indexMap);
				circuitsPerIndexMap[indexMap] = circuit;
			}
		}
		/// <summary>
		/// reverse of <see cref="AddWithMerge(int, int)"/>
		/// </summary>
		/// <param name="indexMap"></param>
		public void RemoveWithBreak(int indexMap)
		{
#if UNITY_EDITOR
			//Console.Log($"RemoveWithBreak indexMap {indexMap}");
			if (circuitsPerIndexMap[indexMap] == null) throw new ArgumentOutOfRangeException($"Try to remove from circuit that doesn't exist in indexMap [{indexMap}]");
#endif
			Circuit circuit = circuitsPerIndexMap[indexMap];

			circuit.Remove(indexMap);

			circuitsPerIndexMap[indexMap] = default;

			if (!circuit.CheckDispose())
			{   // если контур остался на карте прверяем возможен ли разрыв контура после удаления элемента
				Break(circuit);
			}
		}

		#region Get
		/// <summary>
		/// 
		/// </summary>
		/// <example>
		///	<code>
		///	int ret = default;
		///	switch (ret)
		///			{
		///				case 0b_0000: break;
		///				case 0b_0001: break;
		///				case 0b_0010: break;
		///				case 0b_0011: break;
		///				case 0b_0100: break;
		///				case 0b_0101: break;
		///				case 0b_0110: break;
		///				case 0b_0111: break;
		///				case 0b_1000: break;
		///				case 0b_1001: break;
		///				case 0b_1010: break;
		///				case 0b_1011: break;
		///				case 0b_1100: break;
		///				case 0b_1101: break;
		///				case 0b_1110: break;
		///				case 0b_1111: break;
		///		default: throw new ArgumentOutOfRangeException();
		///}
		/// </code>
		/// </example>
		/// <returns>
		/// bit flag total of 4 first from right to left.
		/// Each bit set represent out param that has been set with unique circuite around given center.
		/// Order of bit is URLD for 0b_1111;
		/// </returns>
		public int GetCircuitsSurroundUniques(int indexMapCenter, out Circuit down, out Circuit left, out Circuit right, out Circuit up)
		{
			int flagFormationDLRU = default;

			if (TryGetCircuitsSurround(indexMapCenter, DOWN, out Circuit circuit0))
			{
				down = circuit0;
				flagFormationDLRU |= (1 << DOWN);
			}
			else
			{
				down = default;
			}
			if (TryGetCircuitsSurround(indexMapCenter, LEFT, out Circuit circuit1))
			{
				if (circuit0 != circuit1)
				{
					left = circuit1;
					flagFormationDLRU |= (1 << LEFT);
				}
				else
				{
					left = default;
				}
			}
			else
			{
				left = default;
			}
			if (TryGetCircuitsSurround(indexMapCenter, RIGHT, out Circuit circuit2))
			{
				if (circuit0 != circuit2 && circuit1 != circuit2)
				{
					right = circuit2;
					flagFormationDLRU |= (1 << RIGHT);
				}
				else
				{
					right = default;
				}
			}
			else
			{
				right = default;
			}
			if (TryGetCircuitsSurround(indexMapCenter, UP, out Circuit circuit3))
			{
				if (circuit0 != circuit3 && circuit1 != circuit3 && circuit2 != circuit3)
				{
					up = circuit3;
					flagFormationDLRU |= (1 << UP);
				}
				else
				{
					up = default;
				}
			}
			else
			{
				up = default;
			}
			return flagFormationDLRU;
		}
		/// <inheritdoc cref="GetCircuitsSurroundUniques(int, out Circuit, out Circuit, out Circuit, out Circuit)"/>	
		public int GetCircuitsSurroundUniques(int indexMapCenter, int value, out Circuit down, out Circuit left, out Circuit right, out Circuit up)
		{
			int flagFormationDLRU = default;

			if (TryGetCircuitsSurround(indexMapCenter, DOWN, value, out Circuit circuit0))
			{
				down = circuit0;
				flagFormationDLRU |= (1 << DOWN);
			}
			else
			{
				down = default;
			}
			if (TryGetCircuitsSurround(indexMapCenter, LEFT, value, out Circuit circuit1))
			{
				if (circuit0 != circuit1)
				{
					left = circuit1;
					flagFormationDLRU |= (1 << LEFT);
				}
				else
				{
					left = default;
				}
			}
			else
			{
				left = default;
			}
			if (TryGetCircuitsSurround(indexMapCenter, RIGHT, value, out Circuit circuit2))
			{
				if (circuit0 != circuit2 && circuit1 != circuit2)
				{
					right = circuit2;
					flagFormationDLRU |= (1 << RIGHT);
				}
				else
				{
					right = default;
				}
			}
			else
			{
				right = default;
			}
			if (TryGetCircuitsSurround(indexMapCenter, UP, value, out Circuit circuit3))
			{
				if (circuit0 != circuit3 && circuit1 != circuit3 && circuit2 != circuit3)
				{
					up = circuit3;
					flagFormationDLRU |= (1 << UP);
				}
				else
				{
					up = default;
				}
			}
			else
			{
				up = default;
			}
			return flagFormationDLRU;
		}

		public bool TryGetCircuitsSurround(int indexMapCenter, int indexSideDLRU, int value, out Circuit circuit)
		{
			if (TryGetCircuitsSurround(indexMapCenter, indexSideDLRU, out circuit))
			{
				if (circuit.value == value) return true;
			}
			return false;
		}
		public bool TryGetCircuitsSurround(int indexMapCenter, int indexSideDLRU, out Circuit circuit)
		{
			Point3Int pos = dataGroundStatic.GetPositionAtTilemapByIndexMap(indexMapCenter) + offsetsDLRU[indexSideDLRU];
			circuit = default;
			if (dataGroundStatic.TryGetIndexMapByPos(pos, out int indexMap))
			{
				if (dataGroundDynamic.IsDetectable(indexMap))
				{
					circuit = circuitsPerIndexMap[indexMap];
				}
			}
			else
			{
				return false;
			}
			return circuit != null;
		}
		public Circuit GetCircuitByValue(int value) => circuitsPerValue[value];
		#endregion

		private bool TempContains(Circuit circuit)
		{
			return
				temp0 == circuit ||
				temp1 == circuit ||
				temp2 == circuit ||
				temp3 == circuit;
		}
		private void ClearTemps()
		{
			temp0 = default;
			temp1 = default;
			temp2 = default;
			temp3 = default;
		}
	}
	//public readonly struct CircuiteGroupe
	//{
	//	public readonly int indexStart;
	//	public readonly int indexEnd;
	//	public readonly int idGroupe;
	//}
}//namespace
