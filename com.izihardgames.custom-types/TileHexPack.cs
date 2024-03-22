using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Tile.Hex.Types
{
	/// <summary>
	/// Что значит индексы см <see cref="EHexTileDirectionClockWise"/>
	/// </summary>
	public struct TileHexPack : IList<Vector3Int>
	{
		public Vector3Int n;
		public Vector3Int ne;
		public Vector3Int se;
		public Vector3Int s;
		public Vector3Int sw;
		public Vector3Int nw;
		public Vector3Int center;
		/// <summary>
		/// <see cref="EHexTileDirectionClockWise"/>
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public Vector3Int this[int i]
		{
			get
			{
				switch (i)
				{
					case 0: return n;
					case 1: return ne;
					case 2: return se;
					case 3: return s;
					case 4: return sw;
					case 5: return nw;
					default:
						throw new Exception($"For this type max index is 5!!!");
				}
			}
			set
			{
				switch (i)
				{
					case 0: n = value; break;
					case 1: ne = value; break;
					case 2: se = value; break;
					case 3: s = value; break;
					case 4: sw = value; break;
					case 5: nw = value; break;
					default:
						throw new Exception($"For this type max index is 5!!!");
				}
			}
		}

		public void Fill(Vector3Int centerIn)
		{
			center = centerIn;
			n = centerIn.GetNorth();
			s = centerIn.GetSouth();
			nw = centerIn.GetNorthWest();
			ne = centerIn.GetNorthEast();
			sw = centerIn.GetSouthWest();
			se = centerIn.GetSouthEast();
		}

		public void DebugShowDistances()
		{
			/*
			 vEven
			Disnatce 0: 1 
			Disnatce 1: 1 
			Disnatce 2: 1 
			Disnatce 3: 1 
			Disnatce 4: 1,414214 
			Disnatce 5: 1,414214 
			vOdd
			Disnatce 0: 1 
			Disnatce 1: 1 
			Disnatce 2: 1,414214 
			Disnatce 3: 1,414214
			Disnatce 4: 1
			Disnatce 5: 1 
			 */
			for (int i = 0; i < 6; i++)
			{
				var d = this[i];
				Debug.Log($"Disnatce {i}: {(center - d).magnitude} ");
			}

			// copy past only
			//Vector3Int vEven = new Vector3Int(0, 0, 0);
			//Vector3Int vOdd = new Vector3Int(0, 1, 0);

			//TileHexPack tileHexPackEven = new TileHexPack();
			//TileHexPack tileHexPackOdd = new TileHexPack();

			//tileHexPackEven.Fill(vEven);
			//tileHexPackOdd.Fill(vOdd);

			//tileHexPackEven.DebugShowDistances();
			//tileHexPackOdd.DebugShowDistances();
		}

		public static bool ECompare(EHexTileDirectionClockWise[] compare, EHexTileDirectionClockWise with)
		{
			for (int i = 0; i < compare.Length; i++)
			{
				if (with == compare[i]) return true;
			}
			return false;
		}

		public static EHexTileDirectionClockWise WindRose6To8(int index)
		{
			switch (index)
			{
				case 0: return EHexTileDirectionClockWise.North;
				case 1: return EHexTileDirectionClockWise.NorthEast;
				case 2: return EHexTileDirectionClockWise.SouthEast;
				case 3: return EHexTileDirectionClockWise.South;
				case 4: return EHexTileDirectionClockWise.SouthWest;
				case 5: return EHexTileDirectionClockWise.NorthWest;
				default:
					throw new ArgumentException($"Unexpected value out of range. Must be [0,6]. Value = {index}");
			}
		}

		public int IndexOf(Vector3Int item)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, Vector3Int item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		public void Add(Vector3Int item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(Vector3Int item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(Vector3Int[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(Vector3Int item)
		{
			throw new NotImplementedException();
		}

		public int Count { get => 6; }
		public bool IsReadOnly { get; }

		public IEnumerator<Vector3Int> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Что значит индексы см <see cref="EHexTileDirectionClockWise"/>
	/// </summary>
	public struct TileHexPackId
	{
		public int n;
		public int ne;
		public int se;
		public int s;
		public int sw;
		public int nw;
		public int center;

		public int countRows;
		public int countColums;
		public int this[int i]
		{
			get
			{
				switch (i)
				{
					case 0: return n;
					case 1: return ne;
					case 2: return se;
					case 3: return s;
					case 4: return sw;
					case 5: return nw;
					default:
						throw new Exception($"For this type max index is 5!!!");
				}
			}
			set
			{
				switch (i)
				{
					case 0: n = value; break;
					case 1: ne = value; break;
					case 2: se = value; break;
					case 3: s = value; break;
					case 4: sw = value; break;
					case 5: nw = value; break;
					default:
						throw new Exception($"For this type max index is 5!!!");
				}
			}
		}
		public void Fill(Vector3Int centerIn, Vector2Int sizeMapIn)
		{
			center = GetIdFromCoordXYZ(centerIn, sizeMapIn);
			n = GetIdFromCoordXYZ(centerIn.GetNorth(), sizeMapIn);
			s = GetIdFromCoordXYZ(centerIn.GetSouth(), sizeMapIn);
			nw = GetIdFromCoordXYZ(centerIn.GetNorthWest(), sizeMapIn);
			ne = GetIdFromCoordXYZ(centerIn.GetNorthEast(), sizeMapIn);
			sw = GetIdFromCoordXYZ(centerIn.GetSouthWest(), sizeMapIn);
			se = GetIdFromCoordXYZ(centerIn.GetSouthEast(), sizeMapIn);
		}

		public static int GetIdFromCoordXYZ(Vector3Int v, Vector2Int sizeMapIn)
		{
			return v.y * sizeMapIn.x + v.x;
		}

		public static Vector3Int GetCoordFromId(int id, Vector2Int sizeMapIn)
		{
			int x = id % sizeMapIn.x;
			int y = id / sizeMapIn.x;

			return new Vector3Int(x, y, 0);
		}
	}
	[Flags]
	public enum ETileHex
	{
		None,
		North,
		NorthEast,
		East,
		SouthEast,
		South,
		SouthWest,
		West,
		NorthWest,
		All = North & NorthEast & East & SouthEast & South & SouthWest & West & NorthWest
	}

}