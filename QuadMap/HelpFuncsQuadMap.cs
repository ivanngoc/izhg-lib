using IziHardGames.Libs.Engine.SpaceMap.QuadMap;
using IziHardGames.NonEngine.Mapping.Mapping2D;
using System;
using UnityEngine;

namespace IziHardGames.Libs.Engine.SpaceMap.QuadMap
{
	/// <summary>
	/// ‘ункции дл¤ работы с картой из четырех сторонних фигур.  ажда¤ сторона соединена только 1 раз. Ќапример - шахматна¤ доска
	/// </summary>
	public static class HelpFuncsQuadMap
	{		
		/// <summary>
		/// ѕолучить количество переходов до указанной точки включа¤ диагональные переходы.
		/// ќн же радиус
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static int GetTransitions2D(Vector3Int from, Vector3Int to)
		{
			Vector3Int delta = (to - from);

			int x = Mathf.Abs(delta.x);
			int y = Mathf.Abs(delta.y);

			if (x > y)
			{
				return x;
			}
			return y;
		}
		/// <summary>
		/// ƒиапазон поиска расшир¤етс¤ кругами из центра, равноудаленно.
		/// ѕервый критерий - близость.
		/// ¬торой критерий - нахождение от точки вращени¤/отсчета. 
		/// Ќапример если точка отсчета задана на 15 часов по циферблату то 16 часов будет ближайшим а 14 часов - самым дальним. 
		/// ќпци¤ вращени¤ по часовой стрелке задана здесь специально. ƒЋ¤ других режимов требуютс¤ другие методы
		/// </summary>
		/// <param name="positions"></param>
		/// <param name="takenFlags"></param>
		/// <param name="start"></param>
		/// <returns></returns>
		public static Vector3Int GetFreeTile(Vector3Int[] positions, bool[] takenFlags, Vector3Int start)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// —генерировать массив добавочных векторов (векторов смещени¤). ƒобавл¤¤ смещение к центру получаетс¤ координата ¤чейки совпадающей с кольцевым индексом дл¤ указанного радиуса. 
		/// Ёлемент [0] будет соотвествовать крайней левой нижней ¤чейке. Ёлемент [Lenght/2] - крайней верхней правой ¤чейке. 
		/// дл¤ радиуса 0 - 1 ¤чейка, то есть сам центр
		/// дл¤ радиуса 1 - 8 непосредственно прилегаюших ¤чеек к указанному центру
		/// дл¤ радиуса 2 - 16 ¤чеек и так далее равноудал¤сь от центра как по пр¤мым так и по диоганальным направлени¤м
		/// </summary>
		/// <param name="radius"></param>
		/// <returns></returns>
		public static Vector3Int[] GenerateOffsetsClockwise(int radius)
		{
			if (radius == 0) return new Vector3Int[] { Vector3Int.zero };

			int count = radius * 8;

			Vector3Int[] offsets = new Vector3Int[count];

			for (int i = 0; i < count; i++)
			{
				offsets[i] = Vector3Int.zero.GetAtRadiusQuadsLayerPosition(radius, i);
			}

			return offsets;
		}

		public static Vector2Int[] GenerateOffsetsClockwiseWithIndexes(int radius, int countColumns)
		{
			int count = radius * 8;

			Vector2Int[] offsets = new Vector2Int[count];

			for (int i = 0; i < count; i++)
			{
				var v = Mapping2DHelpFuncs.GetIndexBound(0, radius, countColumns, i);

				offsets[i] = new Vector2Int(v.Item2, v.Item3);
			}

			return offsets;
		}
	}
}