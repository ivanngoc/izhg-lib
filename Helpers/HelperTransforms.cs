using System.Linq;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Helpers
{
	public static class HelperTransforms
	{
		#region Set sibling index accordingly
		public static void SetSiblingIndexByAxisXAscending(Transform[] rectTransforms, int from, int to)
		{
			var enumeration = rectTransforms.Skip(from).Take(to - from).OrderBy(x => x.position.x);

			var enumrator = enumeration.GetEnumerator();

			var minIndex = rectTransforms.Min(x => x.GetSiblingIndex());

			for (var i = from; i < to; i++)
			{
				enumrator.MoveNext();

				var transform = enumrator.Current;

				transform.SetSiblingIndex(minIndex + i);
			}
		}
		public static void SetSiblingIndexByAxisXDescending(Transform[] rectTransforms, int from, int to)
		{
			var enumeration = rectTransforms.Skip(from).Take(to - from).OrderByDescending(x => x.position.x);

			var enumrator = enumeration.GetEnumerator();

			var minIndex = rectTransforms.Min(x => x.GetSiblingIndex());

			for (var i = from; i < to; i++)
			{
				enumrator.MoveNext();

				var transform = enumrator.Current;

				transform.SetSiblingIndex(minIndex + i);
			}
		}
		public static void SetSiblingIndexByAxisYDescending(Transform[] rectTransforms, int from, int to)
		{
			var enumeration = rectTransforms.Skip(from).Take(to - from).OrderBy(x => x.position.y);

			var enumrator = enumeration.GetEnumerator();

			var minIndex = rectTransforms.Min(x => x.GetSiblingIndex());

			for (var i = from; i < to; i++)
			{
				enumrator.MoveNext();

				var transform = enumrator.Current;

				transform.SetSiblingIndex(minIndex + i);
			}
		}
		public static void SetSiblingIndexByAxisYAscending(Transform[] rectTransforms, int from, int to)
		{
			var enumeration = rectTransforms.Skip(from).Take(to - from).OrderByDescending(x => x.position.y);

			var enumrator = enumeration.GetEnumerator();

			var minIndex = rectTransforms.Min(x => x.GetSiblingIndex());

			for (var i = from; i < to; i++)
			{
				enumrator.MoveNext();

				var transform = enumrator.Current;

				transform.SetSiblingIndex(minIndex + i);
			}
		}
		#endregion

		#region Sibling Hierarchy (то же самя функция что сверху только с внутренней проверкой)
		/// <summary>
		/// Раставить чайлды в иерархии по их координате X по возрастанию
		/// </summary>
		public static void SortHierarchyAscendingX(Transform[] rectTransforms, int from, int to)
		{
			if (IsSiblings(rectTransforms, from, to, out var parent))
			{
				var enume = rectTransforms.OrderBy(x => x.position.x);

				int count = default;

				foreach (var item in enume)
				{
					item.SetSiblingIndex(count);

					count++;
				}
			}
		}
		public static void SortHierarchyDescendingX(Transform[] rectTransforms, int from, int to)
		{
			if (IsSiblings(rectTransforms, from, to, out var parent))
			{
				var enume = rectTransforms.OrderByDescending(x => x.position.x);

				int count = default;

				foreach (var item in enume)
				{
					item.SetSiblingIndex(count);

					count++;
				}
			}
		}
		public static void SortHierarchyAscendingY(Transform[] rectTransforms, int from, int to)
		{
			if (IsSiblings(rectTransforms, from, to, out var parent))
			{
				var enume = rectTransforms.OrderBy(x => x.position.y);

				int count = default;

				foreach (var item in enume)
				{
					item.SetSiblingIndex(count);

					count++;
				}
			}
		}
		public static void SortHierarchyDescendingY(Transform[] rectTransforms, int from, int to)
		{
			if (IsSiblings(rectTransforms, from, to, out var parent))
			{
				var enume = rectTransforms.OrderByDescending(x => x.position.y);

				int count = default;

				foreach (var item in enume)
				{
					item.SetSiblingIndex(count);

					count++;
				}
			}
		}
		#endregion

		public static bool IsSiblings(Transform[] transforms, int from, int to, out Transform parent)
		{
			parent = transforms[from].parent;

			for (var i = from; i < to; i++)
			{
				if (parent != transforms[i].parent) return false;
			}

			return true;
		}
	}

}