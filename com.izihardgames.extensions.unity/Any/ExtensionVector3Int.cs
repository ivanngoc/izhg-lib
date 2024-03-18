using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
	public static class ExtensionVector3IntQuad
	{
		/// <summary>
		/// Проверяет находится ли координата внутри прямоугольника образованого из левого нижнего угла в верхний правый угол начиная с 0,0
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static bool IsInRect(this Vector3Int self, Vector2 size)
		{
			var isX = -1 < self.x && self.x < size.x;
			var isY = -1 < self.y && self.y < size.y;

			return isX && isY;
		}

		/// <summary>
		/// Преобразовывает координату в Id. id = 0 будет самая первая ячейка, id=width*height-1 будет самая последняя
		/// </summary>
		/// <param name="v"></param>
		/// <param name="origin">Ячейка начала отсчета. Должен быть = <see cref="BoundsInt.min"/></param>
		/// <param name="size">Размер сетка X на Y </param>
		/// <returns></returns>
		public static int CalcId2d(this Vector3Int v, ref Vector3Int origin, ref Vector3Int size)
		{
			var shift = v - origin;

			return shift.y * size.x + shift.x;
		}
		/// <summary>
		/// Преобразовывает координату в Id. id = 0 будет самая первая ячейка, id=width*height-1 будет самая последняя
		/// </summary>
		/// <param name="v"></param>
		/// <param name="boundsInt"></param>
		/// <returns></returns>
		public static int CalcId2d(this Vector3Int v, ref BoundsInt boundsInt)
		{
			var shift = v - boundsInt.position;

			return shift.y * boundsInt.size.x + shift.x;
		}
		/// <summary>
		/// Преобразовывает координату в Id. id = 0 будет самая первая ячейка, id=width*height-1 будет самая последняя
		/// </summary>
		/// <param name="v"></param>
		/// <param name="tilemap"></param>
		/// <returns></returns>
		public static int CalcId2d(this Vector3Int v, ITilemap tilemap)
		{
			var shift = v - tilemap.origin;

			return shift.y * tilemap.size.x + shift.x;
		}
		/// <summary>
		/// Вставить в список по возрастанию XYZ. Сначала проверяется X, затем Y, затем Z
		/// </summary>
		/// <param name="self"></param>
		/// <returns></returns>
		public static List<Vector3Int> InsertOrderedXYZInto(this Vector3Int self, List<Vector3Int> into)
		{
			int count = into.Count;

			for (int i = 0; i < count; i++)
			{
				Vector3 compare = into[i];

				if (self.x == compare.x)
				{
					if (self.y == compare.y)
					{
						if (self.z == compare.z)
						{
							into.Insert(i, self);

							return into;
						}
						else
						{
							if (self.z < compare.z)
							{
								into.Insert(i, self);

								return into;
							}
						}
					}
					else
					{
						if (self.y < compare.y)
						{
							into.Insert(i, self);
							return into;
						}
					}
				}
				else
				{
					if (self.x < compare.x)
					{
						into.Insert(i, self);
						return into;
					}
				}
			}

			into.Add(self);

			return into;
		}
	}
}