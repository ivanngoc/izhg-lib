using IziHardGames.Libs.Engine.CustomTypes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Helpers
{
	/// <summary>
	/// Методы расширения для работы с <see cref="RectTransform"/><br/>
	/// Выравнивание по области<br/>
	/// Распределения по области<br/>
	/// Разбиение пополам<br/>
	/// </summary>
	/// <remarks>
	/// Remarks:<br/>
	/// isFlip - параметр определяющий с какой стороной будет прикреплен объект к границе<br/>
	/// По умолчанию (isFlip = false) например к нижней границе объект будет приклреплен нижней стороной<br/>
	/// С параметров isFlip = false к нижней границе объект будет приклреплен верхней стороной
	/// </remarks>
	public static class HelperRectTransform
	{

		#region Align Objects Vertical

		public static void AlignVerticalTop(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isFlip = default)
		{
			var count = transforms.Length;

			for (var i = 0; i < count; i++)
			{
				var pos = transforms[i].position;

				if (isFlip)
				{
					transforms[i].position = new Vector3(pos.x, rectPosGlobal.maxY + -transforms[i].rect.yMin, pos.z);
				}
				else
				{
					transforms[i].position = new Vector3(pos.x, rectPosGlobal.maxY + -transforms[i].rect.yMax, pos.z);
				}
			}
		}
		public static void AlignVerticalMid(RectTransform[] transforms, in RectPosGlobal rectPosGlobal)
		{
			var count = transforms.Length;

			for (var i = 0; i < count; i++)
			{
				var pos = transforms[i].position;

				transforms[i].position = new Vector3(pos.x, rectPosGlobal.MidY + -transforms[i].rect.center.y, pos.z);
			}
		}
		public static void AlignVerticalBot(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isFlip = default)
		{
			var count = transforms.Length;

			for (var i = 0; i < count; i++)
			{
				var pos = transforms[i].position;

				if (isFlip)
				{
					transforms[i].position = new Vector3(pos.x, rectPosGlobal.minY + -transforms[i].rect.yMax, pos.z);
				}
				else
				{
					transforms[i].position = new Vector3(pos.x, rectPosGlobal.minY + -transforms[i].rect.yMin, pos.z);
				}
			}
		}
		#endregion

		#region Align Objects Horizontal
		public static void AlignHorizontalLeft(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isFlip = default)
		{
			var count = transforms.Length;

			for (var i = 0; i < count; i++)
			{
				var pos = transforms[i].position;

				if (isFlip)
				{
					transforms[i].position = new Vector3(rectPosGlobal.minX + -transforms[i].rect.xMax, pos.y, pos.z);
				}
				else
				{
					transforms[i].position = new Vector3(rectPosGlobal.minX + -transforms[i].rect.xMin, pos.y, pos.z);
				}

			}
		}
		public static void AlignHorizontalMid(RectTransform[] transforms, in RectPosGlobal rectPosGlobal)
		{
			var count = transforms.Length;

			for (var i = 0; i < count; i++)
			{
				var pos = transforms[i].position;

				transforms[i].position = new Vector3(rectPosGlobal.MidX - transforms[i].rect.center.x, pos.y, pos.z);
			}
		}
		public static void AlignHorizontalRight(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isFlip = false)
		{
			var count = transforms.Length;

			for (var i = 0; i < count; i++)
			{
				var pos = transforms[i].position;

				if (isFlip)
				{
					transforms[i].position = new Vector3(rectPosGlobal.maxX + -transforms[i].rect.xMin, pos.y, pos.z);
				}
				else
				{
					transforms[i].position = new Vector3(rectPosGlobal.maxX + -transforms[i].rect.xMax, pos.y, pos.z);
				}
			}
		}
		#endregion

		#region Destribute Objects Vertical
		public static void DestributeVerticalTop(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isOrderByPos)
		{
			if (transforms.Length < 3) return;

			IEnumerable<RectTransform> enumerable = transforms;

			if (isOrderByPos)
			{
				enumerable = transforms.OrderByDescending(x => x.position.y);
			}

			var height = rectPosGlobal.Height;

			var heightDif = enumerable.Last().rect.height;

			var heightShort = height - heightDif;

			var count = transforms.Length;

			var enumerator = enumerable.GetEnumerator();

			float f = count - 1;

			for (var i = 0; i < count; i++)
			{
				enumerator.MoveNext();

				var item = enumerator.Current as RectTransform;

				var limit = heightShort * ((f - i) / f) + rectPosGlobal.minY + heightDif;

				var y = limit - item.rect.yMax;

				item.position = new Vector3(item.position.x, y, item.position.z);
			}
		}
		public static void DestributeVerticalMid(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isOrderByPos)
		{
			if (transforms.Length < 3) return;

			IEnumerable<RectTransform> enumeration = transforms;

			if (isOrderByPos)
			{
				enumeration = transforms.OrderBy(x => x.position.y);
			}

			var height = rectPosGlobal.Height;

			var heightPad = enumeration.First().rect.height / 2;

			var heightDif = enumeration.Last().rect.height / 2 + heightPad;

			var heightShort = height - heightDif;

			var count = transforms.Length;

			var enumerator = enumeration.GetEnumerator();

			float f = count - 1;

			for (var i = 0; i < count; i++)
			{
				enumerator.MoveNext();

				var item = enumerator.Current as RectTransform;

				var limit = heightShort * (i / f) + rectPosGlobal.minY + heightPad;

				var y = limit - item.rect.center.y;

				item.position = new Vector3(item.position.x, y, item.position.z);
			}
		}
		public static void DestributeVerticalBot(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isOrderByPos)
		{
			if (transforms.Length < 3) return;

			IEnumerable<RectTransform> enumeration = transforms;

			if (isOrderByPos)
			{
				enumeration = transforms.OrderBy(x => x.position.y);
			}

			var height = rectPosGlobal.Height;

			var heightDif = enumeration.First().rect.height;

			var heightShort = height - heightDif;

			var count = transforms.Length;

			var enumerator = enumeration.GetEnumerator();

			float f = count - 1;

			for (var i = 0; i < count; i++)
			{
				enumerator.MoveNext();

				var item = enumerator.Current as RectTransform;

				var limit = rectPosGlobal.maxY - heightShort * ((f - i) / f) - heightDif;

				var y = limit - item.rect.yMin;

				item.position = new Vector3(item.position.x, y, item.position.z);
			}
		}
		public static void DestributeVerticalSpaces(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isOrderByPos)
		{
			if (transforms.Length < 3) return;

			IEnumerable<RectTransform> enumeration = transforms;

			if (isOrderByPos)
			{
				enumeration = transforms.OrderByDescending(x => x.position.y);
			}

			var height = rectPosGlobal.Height;

			var heightTaken = transforms.Sum(x => x.rect.height);

			var heightFree = height - heightTaken;

			float heightFreePerObject = default;

			var count = transforms.Length - 1;

			var isFreeSpace = heightFree > 0f;

			if (!isFreeSpace)
			{
				DestributeVerticalMid(transforms, in rectPosGlobal, isOrderByPos);

				return;
			}

			float lenghtProceed = default;

			var enumerator = enumeration.GetEnumerator();

			enumerator.MoveNext();

			var first = enumerator.Current;

			heightFreePerObject = heightFree / ((count - 1) * 2 + 2);

			lenghtProceed = rectPosGlobal.maxY - enumerator.Current.rect.height - heightFreePerObject;

			float f = count;

			for (var i = 1; i < count; i++)
			{
				enumerator.MoveNext();

				var item = enumerator.Current as RectTransform;

				lenghtProceed -= heightFreePerObject;

				item.position = new Vector3(item.position.x, lenghtProceed - item.rect.yMax, item.position.z);

				lenghtProceed -= item.rect.height;

				lenghtProceed -= heightFreePerObject;
			}
			enumerator.MoveNext();

			var last = enumerator.Current;

			first.position = new Vector3(first.position.x, rectPosGlobal.maxY + -first.rect.yMax, first.position.z);

			last.position = new Vector3(last.position.x, rectPosGlobal.minY + -last.rect.yMin, last.position.z);
		}
		#endregion

		#region Distribute Objects Horizontal
		public static void DestributeHorizontalRight(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isOrderByPos)
		{
			if (transforms.Length < 3) return;

			IEnumerable<RectTransform> enumerable = transforms;

			if (isOrderByPos)
			{
				enumerable = transforms.OrderByDescending(x => x.position.x);
			}

			var width = rectPosGlobal.Width;

			var widthDif = enumerable.Last().rect.width;

			var widthShort = width - widthDif;

			var count = transforms.Length;

			var enumerator = enumerable.GetEnumerator();

			float f = count - 1;

			for (var i = 0; i < count; i++)
			{
				enumerator.MoveNext();

				var item = enumerator.Current as RectTransform;

				var limit = widthShort * ((f - i) / f) + rectPosGlobal.minX + widthDif;

				var x = limit - item.rect.xMax;

				item.position = new Vector3(x, item.position.y, item.position.z);
			}
		}
		public static void DestributeHorizontalMid(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isOrderByPos)
		{
			if (transforms.Length < 3) return;

			IEnumerable<RectTransform> enumerable = transforms;

			if (isOrderByPos)
			{
				enumerable = transforms.OrderBy(x => x.position.x);
			}

			var width = rectPosGlobal.Width;

			var widthPad = enumerable.First().rect.width / 2;

			var widthDif = enumerable.Last().rect.width / 2 + widthPad;

			var widthShort = width - widthDif;

			var count = transforms.Length;

			var enumerator = enumerable.GetEnumerator();

			float f = count - 1;

			for (var i = 0; i < count; i++)
			{
				enumerator.MoveNext();

				var item = enumerator.Current as RectTransform;

				var limit = widthShort * (i / f) + rectPosGlobal.minX + widthPad;

				var x = limit - item.rect.center.x;

				item.position = new Vector3(x, item.position.y, item.position.z);
			}
		}
		public static void DestributeHorizontalLeft(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isOrderByPos)
		{
			if (transforms.Length < 3) return;

			IEnumerable<RectTransform> enumerable = transforms;

			if (isOrderByPos)
			{
				enumerable = transforms.OrderBy(x => x.position.x);
			}

			var width = rectPosGlobal.Width;

			var widthDif = enumerable.First().rect.width;

			var widthShort = width - widthDif;

			var count = transforms.Length;

			var enumerator = enumerable.GetEnumerator();

			float f = count - 1;

			for (var i = 0; i < count; i++)
			{
				enumerator.MoveNext();

				var item = enumerator.Current as RectTransform;

				var limit = rectPosGlobal.maxX - widthShort * ((f - i) / f) - widthDif;

				var x = limit - item.rect.xMin;

				item.position = new Vector3(x, item.position.y, item.position.z);
			}
		}
		public static void DestributeHorizontalSpaces(RectTransform[] transforms, in RectPosGlobal rectPosGlobal, bool isOrderByPos)
		{
			if (transforms.Length < 3) return;

			IEnumerable<RectTransform> enumerable = transforms;

			if (isOrderByPos)
			{
				enumerable = transforms.OrderByDescending(x => x.position.x);
			}


			var width = rectPosGlobal.Width;

			var widthTaken = transforms.Sum(x => x.rect.width);

			var widthFree = width - widthTaken;

			float widthFreePerObject = default;

			var count = transforms.Length - 1;

			var isFreeSpace = widthFree > 0f;

			if (!isFreeSpace)
			{
				DestributeHorizontalMid(transforms, in rectPosGlobal, isOrderByPos);

				return;
			}

			float lenghtProceed = default;

			var enumerator = enumerable.GetEnumerator();

			enumerator.MoveNext();

			var first = enumerator.Current;

			widthFreePerObject = widthFree / ((count - 1) * 2 + 2);

			lenghtProceed = rectPosGlobal.maxX - enumerator.Current.rect.width - widthFreePerObject;

			float f = count;

			for (var i = 1; i < count; i++)
			{
				enumerator.MoveNext();

				var item = enumerator.Current as RectTransform;

				lenghtProceed -= widthFreePerObject;

				item.position = new Vector3(lenghtProceed - item.rect.xMax, item.position.y, item.position.z);

				lenghtProceed -= item.rect.width;

				lenghtProceed -= widthFreePerObject;
			}
			enumerator.MoveNext();

			var last = enumerator.Current;

			first.position = new Vector3(rectPosGlobal.maxX + -first.rect.xMax, first.position.y, first.position.z);

			last.position = new Vector3(rectPosGlobal.minX + -last.rect.xMin, last.position.y, last.position.z);
		}
		#endregion

		#region Spacing And Padding
		public static void SpaceRemoveVerticalToTop(RectTransform[] transforms, int from, int to)
		{
			var order = transforms.Skip(from).Take(to - from).OrderByDescending(x => x.position.y);

			var enumerator = order.GetEnumerator();

			enumerator.MoveNext();

			var previous = enumerator.Current;

			for (var i = from + 1; i < to; i++)
			{
				enumerator.MoveNext();

				var next = enumerator.Current;

				var diff = previous.GetWorldRectMinY() - next.GetWorldRectMaxY();

				next.transform.position = new Vector3(next.transform.position.x, next.transform.position.y + diff, next.transform.position.z);

				previous = next;
			}
		}
		public static void SpaceRemoveVerticalToBot(RectTransform[] transforms, int from, int to)
		{
			var order = transforms.Skip(from).Take(to - from).OrderBy(x => x.position.y);

			var enumerator = order.GetEnumerator();

			enumerator.MoveNext();

			var previous = enumerator.Current;

			for (var i = from + 1; i < to; i++)
			{
				enumerator.MoveNext();

				var next = enumerator.Current;

				var diff = previous.GetWorldRectMaxY() - next.GetWorldRectMinY();

				next.transform.position = new Vector3(next.transform.position.x, next.transform.position.y + diff, next.transform.position.z);

				previous = next;
			}
		}

		public static void SpaceRemoveHorizontalToLeft(RectTransform[] transforms, int from, int to)
		{
			var order = transforms.Skip(from).Take(to - from).OrderBy(x => x.position.x);

			var enumerator = order.GetEnumerator();

			enumerator.MoveNext();

			var previous = enumerator.Current;

			for (var i = from + 1; i < to; i++)
			{
				enumerator.MoveNext();

				var next = enumerator.Current;

				var diff = previous.GetWorldRectMaxX() - next.GetWorldRectMinX();

				next.transform.position = new Vector3(next.transform.position.x + diff, next.transform.position.y, next.transform.position.z);

				previous = next;
			}
		}

		public static void SpaceRemoveHorizontalToRight(RectTransform[] transforms, int from, int to)
		{
			var order = transforms.Skip(from).Take(to - from).OrderByDescending(x => x.position.x);

			var enumerator = order.GetEnumerator();

			enumerator.MoveNext();

			var previous = enumerator.Current;

			for (var i = from + 1; i < to; i++)
			{
				enumerator.MoveNext();

				var next = enumerator.Current;

				var diff = previous.GetWorldRectMinX() - next.GetWorldRectMaxX();

				next.transform.position = new Vector3(next.transform.position.x + diff, next.transform.position.y, next.transform.position.z);

				previous = next;
			}
		}
		#endregion

		#region Partition
		public static void PartingHorizontalCloneToLeft(RectTransform[] rectTransforms, int from, int to, int fromR, RectTransform[] result = default)
		{
			var count = to - from;

			for (var i = 0; i < count; i++)
			{
				var rectTransform = rectTransforms[i + from];

				var x = rectTransform.rect.width / 2;

				rectTransform.offsetMin += new Vector2(x, 0);

				var clone = Object.Instantiate(rectTransform.gameObject, rectTransform.parent);

				var copy = clone.GetComponent<RectTransform>();

				copy.position = new Vector3(rectTransform.position.x - x, rectTransform.position.y, rectTransform.position.z);

				if (result != null)
				{
					result[fromR + i] = copy;
				}
			}
		}
		public static void PartingHorizontalCloneToRight(RectTransform[] rectTransforms, int from, int to, int fromR, RectTransform[] result = default)
		{
			var count = to - from;

			for (var i = 0; i < count; i++)
			{
				var rectTransform = rectTransforms[i + from];

				var x = rectTransform.rect.width / 2;

				rectTransform.offsetMax -= new Vector2(x, 0);

				var clone = Object.Instantiate(rectTransform.gameObject, rectTransform.parent);

				var copy = clone.GetComponent<RectTransform>();

				copy.position = new Vector3(rectTransform.position.x + x, rectTransform.position.y, rectTransform.position.z);

				if (result != null)
				{
					result[fromR + i] = copy;
				}
			}
		}
		public static void PartingVerticalCloneToTop(RectTransform[] rectTransforms, int from, int to, int fromR, RectTransform[] result = default)
		{
			var count = to - from;

			for (var i = 0; i < count; i++)
			{
				var rectTransform = rectTransforms[i + from];

				var y = rectTransform.rect.height / 2;

				rectTransform.offsetMax -= new Vector2(0, y);

				var clone = Object.Instantiate(rectTransform.gameObject, rectTransform.parent);

				var copy = clone.GetComponent<RectTransform>();

				copy.position = new Vector3(rectTransform.position.x, rectTransform.position.y + y, rectTransform.position.z);

				if (result != null)
				{
					result[fromR + i] = copy;
				}
			}
		}
		public static void PartingVerticalCloneToBot(RectTransform[] rectTransforms, int from, int to, int fromR, RectTransform[] result = default)
		{
			var count = to - from;

			for (var i = 0; i < count; i++)
			{
				var rectTransform = rectTransforms[i + from];

				var y = rectTransform.rect.height / 2;

				rectTransform.offsetMin += new Vector2(0, y);

				var clone = Object.Instantiate(rectTransform.gameObject, rectTransform.parent);

				var copy = clone.GetComponent<RectTransform>();

				copy.position = new Vector3(rectTransform.position.x, rectTransform.position.y - y, rectTransform.position.z);

				if (result != null)
				{
					result[fromR + i] = copy;
				}
			}
		}
		#endregion

		#region Spacing add/sub
		/// <summary>
		/// Supose to be sorted <br/>
		/// Dir - normalized<br/>
		/// Тот кто находится в конце списка получит наибольшее смещение<br/>
		/// </summary>
		/// <param name="rectTransforms"></param>
		public static void SpaceAddAscending(RectTransform[] rectTransforms, Vector3 dir, int from, int to, float space)
		{
			for (var i = from; i < to; i++)
			{
				rectTransforms[i].transform.position += dir * (space * i);
			}
		}

		public static void SpaceAddDescending(RectTransform[] rectTransforms, Vector3 dir, int from, int to, float space)
		{
			var count = to - from - 1;

			for (var i = from; i < to; i++)
			{
				rectTransforms[i].transform.position += dir * (space * (count - i));
			}
		}
		public static void SpaceSubAscending(RectTransform[] rectTransforms, Vector3 dir, int from, int to, float space)
		{
			for (var i = from; i < to; i++)
			{
				rectTransforms[i].transform.position -= dir * (space * i);
			}
		}
		public static void SpaceSubDescending(RectTransform[] rectTransforms, Vector3 dir, int from, int to, float space)
		{
			var count = to - from - 1;

			for (var i = from; i < to; i++)
			{
				rectTransforms[i].transform.position -= dir * (space * (count - i));
			}
		}
		#endregion


	}
}