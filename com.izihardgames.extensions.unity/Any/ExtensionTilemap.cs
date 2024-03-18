using System;
using System.Collections.Generic;

namespace UnityEngine.Tilemaps
{
	public static class ExtensionTilemap
	{
		public static List<Vector3Int> GetUsedCoordsXYZ(this Tilemap tilemap)
		{
			var bounds = tilemap.cellBounds;

			List <Vector3Int> result = new List<Vector3Int>();

			for (int z = bounds.zMin; z < bounds.zMax; z++)
			{
				for (int y = bounds.yMin; y < bounds.yMax; y++)
				{
					for (int x = bounds.xMin; x < bounds.xMax; x++)
					{
						Vector3Int coord = new Vector3Int(x, y, z);

						TileBase tileBase = tilemap.GetTile(coord);

						if (tileBase != null)
						{
							result.Add(coord);
						}
					}
				}
			}
			return result;
		}

		/// <summary>
		/// »ндексы формируютс¤ согласно оси XYZ: сначала движение по X (ширина), 
		/// заетем движение по Y (высота), затем по Z (глубина)
		/// точка Origin это минимальна¤ точка баунда
		/// </summary>
		/// <param name="tilemap"></param>
		/// <param name="result"></param>
		/// <param name="resultIndexes"></param>
		/// <returns></returns>
		public static bool GetUsedCoordsXYZ(this Tilemap tilemap, List<Vector3Int> result, List<int> resultIndexes)
		{
			var bounds = tilemap.cellBounds;

			int countStart = result.Count;

			int index = default;

			for (int z = bounds.zMin; z < bounds.zMax; z++)
			{
				for (int y = bounds.yMin; y < bounds.yMax; y++)
				{
					for (int x = bounds.xMin; x < bounds.xMax; x++)
					{
						Vector3Int coord = new Vector3Int(x, y, z);

						TileBase tileBase = tilemap.GetTile(coord);

						if (tileBase != null)
						{
							result.Add(coord);

							resultIndexes.Add(index);

							index++;
						}
					}
				}
			}

			return countStart != result.Count;
		}
		/// <summary>
		/// Функци¤ для иттерации while. как yield но без выделени¤ пам¤ти
		/// </summary>
		/// <param name="tilemap"></param>
		/// <param name="itteratorPos"></param>
		/// <param name="index"></param>
		/// <param name="tileBase"></param>
		/// <returns></returns>
		public static bool GetUsedCoordNextXYZ(this Tilemap tilemap, ref int index, ref Vector3Int itteratorPos, ref Vector3Int tilePos, ref TileBase tileBase)
		{
			var bounds = tilemap.cellBounds;

			bool isBreak = false;

			for (; itteratorPos.z < bounds.zMax; itteratorPos.z++)
			{
				for (; itteratorPos.y < bounds.yMax; itteratorPos.y++)
				{
					for (; itteratorPos.x < bounds.xMax; itteratorPos.x++)
					{
						if (isBreak)
						{
							return true;
						}
						tileBase = tilemap.GetTile(itteratorPos);

						if (tileBase != null)
						{
							tilePos = itteratorPos;

							index++;

							isBreak = true;

							continue;
						}
					}
					itteratorPos.x = tilemap.cellBounds.xMin;
				}
				itteratorPos.y = tilemap.cellBounds.yMin;
			}
			return isBreak;
		}

		public static Vector3 CellToScreen(this Tilemap tilemap, Camera camera, Vector3Int pos)
		{
			return camera.WorldToScreenPoint(tilemap.GetCellCenterWorld(pos));
		}
		public static Vector3 ScreenToWroldCellCenter(this Tilemap tilemap, Camera camera, Vector2 screenPos)
		{
			Vector3 worldPos = camera.ScreenToWorldPoint(screenPos);

			Vector3Int vector3Int = tilemap.ScreenToCell(camera, screenPos);

			return tilemap.GetCellCenterWorld(vector3Int);
		}
		public static Vector3Int ScreenToCell(this Tilemap tilemap, Camera camera, Vector2 screenPos)
		{
			Vector3 worldPos = camera.ScreenToWorldPoint(screenPos);
			return tilemap.WorldToCell(worldPos);
		}

		public static Tilemap CopyTilesFrom(this Tilemap self, Tilemap from)
		{
#if UNITY_EDITOR
			int loopProtect = 1000;
#endif
			int currentIndex = default;

			Vector3Int currentItter = from.cellBounds.min;

			Vector3Int pos = default;

			TileBase foundedTile = default;

			while (from.GetUsedCoordNextXYZ(ref currentIndex, ref currentItter, ref pos, ref foundedTile))
			{
#if UNITY_EDITOR
				if (loopProtect-- < 0) throw new OverflowException();
#endif
				self.SetTile(pos, foundedTile);
			}
			return self;
		}
	}
}