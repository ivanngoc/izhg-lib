using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Graphs
{
	public class GraphTools
	{
		#region Unity Message
		#endregion
	}
	/// <summary>
	/// Создает граф в котором каждый элемент может иметь максимум 3 связи. и если граф размером больше 3 то всегда у каждого элемента будет 3 связи
	/// </summary>
	public static class CreatorTriangularGraph
	{
		/// <summary>
		/// найти 3 ближайштх объекта из списка
		/// </summary>
		/// <typeparam name="TList"></typeparam>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="list"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public static (TItem, TItem, TItem) FindBindings<TList, TItem>(TList list, TItem item)
			where TList : IList<TItem>
			where TItem : Component
		{
			int count = list.Count;

			if (count < 1) return default;

			if (count > 3)
			{
				TItem close0 = default;
				TItem close1 = default;
				TItem close2 = default;

				float sqrDistance0 = float.MaxValue;
				float sqrDistance1 = float.MaxValue;
				float sqrDistance2 = float.MaxValue;

				// нужно найти три ближайшие точки
				foreach (var element in list)
				{
					float sqrDistance = (item.transform.position - element.transform.position).sqrMagnitude;

					if (sqrDistance2 > sqrDistance)
					{
						if (sqrDistance1 > sqrDistance)
						{
							if (sqrDistance0 > sqrDistance)
							{
								close0 = element;
								sqrDistance0 = sqrDistance;
							}
							else
							{
								close1 = element;
								sqrDistance1 = sqrDistance;
							}
						}
						else
						{
							close2 = element;
							sqrDistance2 = sqrDistance;
						}
					}
				}
				return (close0, close1, close2);
			}
			else
			{
				if (count > 2)
				{
					return (list.ElementAt(0), list.ElementAt(1), list.ElementAt(3));
				}
				else
				{
					if (count > 1)
					{
						return (list.ElementAt(0), list.ElementAt(1), default);
					}
					else
					{
						return (list.ElementAt(0), default, default);
					}
				}
			}
		}
	}
}