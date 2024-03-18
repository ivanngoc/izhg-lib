using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
	/// <summary>
	/// Основная часть. Все типы с которыми происходит работа принадлежат движку
	/// </summary>
	public static partial class ExtensionRectTransform
	{
		public static void CopyFrom(this RectTransform rectTransform, RectTransform from)
		{
			rectTransform.anchorMin = from.anchorMin;
			rectTransform.anchorMax = from.anchorMax;

			rectTransform.pivot = from.pivot;

			rectTransform.sizeDelta = from.sizeDelta;

			rectTransform.position = from.position;
			rectTransform.localScale = from.localScale;
			rectTransform.rotation = from.rotation;
		}
		public static bool IsRectAbove(this RectTransform self, RectTransform it)
		{
			Vector2 minSelf = (Vector2)self.position + self.rect.min;
			Vector2 maxWith = (Vector2)it.position + it.rect.max;

			return minSelf.y > maxWith.y;
		}
		public static bool IsRectBeneath(this RectTransform self, RectTransform it)
		{
			Vector2 maxSelf = (Vector2)self.position + self.rect.max;
			Vector2 minWith = (Vector2)it.position + it.rect.min;

			return maxSelf.y < minWith.y;
		}
		public static bool IsIntersectWith(this RectTransform self, RectTransform with)
		{
			Vector2 minSelf = (Vector2)self.position + self.rect.min;
			Vector2 maxSelf = (Vector2)self.position + self.rect.max;

			Vector2 minWith = (Vector2)with.position + with.rect.min;
			Vector2 maxWith = (Vector2)with.position + with.rect.max;

			if (maxSelf.y < minWith.y || minSelf.y > maxWith.y)
				return false;

			if (maxSelf.x < minWith.x || minSelf.x > maxWith.x)
				return false;

			return true;
		}

		public static float GetWorldRectMaxX(this RectTransform self)
		{
			return self.position.x + self.rect.xMax;
		}
		public static float GetWorldRectMaxY(this RectTransform self)
		{
			return self.position.y + self.rect.yMax;
		}
		public static float GetWorldRectMinX(this RectTransform self)
		{
			return self.position.x + self.rect.xMin;
		}
		public static float GetWorldRectMinY(this RectTransform self)
		{
			return self.position.y + self.rect.yMin;
		}
		public static float GetWorldRectMidX(this RectTransform self)
		{
			return self.position.x + self.rect.center.x;
		}
		public static float GetWorldRectMidY(this RectTransform self)
		{
			return self.position.y + self.rect.center.y;
		}
		/// <summary>
		/// Установить координату по центру вне зависимости от нахождения пивота
		/// </summary>
		/// <param name="self"></param>
		/// <param name="pos"></param>
		public static void SetPositionCentered(this RectTransform self, Vector3 pos)
		{
			throw new System.NotImplementedException();
		}
		public static void FitToParent(this RectTransform self)
		{
			self.FitToRectTransform(self.parent as RectTransform);
		}
		public static void FitToRectTransform(this RectTransform self, RectTransform toFit)
		{
			// Min bound
			Vector3 temp = (Vector3)self.rect.min + self.position;
			Vector3 tempFit = (Vector3)toFit.rect.min + toFit.position;
			Vector2 diff = tempFit - temp;
			self.offsetMin += diff;

			temp = (Vector3)self.rect.max + self.position;
			tempFit = (Vector3)toFit.rect.max + toFit.position;
			diff = tempFit - temp;
			self.offsetMax += diff;
		}

		#region Get Bounds World. Template Get [naming by Order XYZ] Left/Mid/Right + Bot/Mid/Top + Forward/Mid/Back

		public static Vector3 GetWorldLeftBot(this RectTransform rectTransform)
		{
			var rect = rectTransform.rect;
			return rectTransform.position + new Vector3(rect.xMin, rect.yMin);
		}
		public static Vector3 GetWorldLeftMid(this RectTransform rectTransform)
		{
			var rect = rectTransform.rect;
			return rectTransform.position + new Vector3(rect.xMin, rect.center.y, 0);
		}
		public static Vector3 GetWorldLeftTop(this RectTransform rectTransform)
		{
			var rect = rectTransform.rect;
			return rectTransform.position + new Vector3(rect.xMin, rect.yMax, 0);
		}
		public static Vector3 GetWorldMidTop(this RectTransform rectTransform)
		{
			var rect = rectTransform.rect;
			return rectTransform.position + new Vector3(rect.center.x, rect.yMax, 0);
		}
		public static Vector3 GetWorldMidMid(this RectTransform rectTransform)
		{
			var rect = rectTransform.rect;
			return rectTransform.position + new Vector3(rect.center.x, rect.center.y);
		}
		public static Vector3 GetWorldRightTop(this RectTransform rectTransform)
		{
			var rect = rectTransform.rect;
			return rectTransform.position + new Vector3(rect.xMax, rect.yMax, 0);
		}
		#endregion

		#region Get Offset. Template Get Left/Center/Right + Bot/Mid/Top
		/// <summary>
		/// Получить величину для прибавки к pivot для получения необходимой коордианты.
		/// </summary>
		/// <param name="rectTransform"></param>
		/// <returns></returns>
		public static Vector3 GetOffsetToLeftMid(this RectTransform rectTransform)
		{
			var rect = rectTransform.rect;
			return new Vector3(-rect.xMin, -rect.center.y);
		}
		public static Vector3 GetOffsetMidTop(this RectTransform rectTransform)
		{
			var rect = rectTransform.rect;
			return new Vector3(-rect.center.x, -rect.yMax);
		}
		/// <summary>
		/// to Mid/Mid/
		/// </summary>
		/// <param name="rectTransform"></param>
		/// <returns></returns>
		public static Vector3 GetOffsetToMidMid(this RectTransform rectTransform)
		{
			var rect = rectTransform.rect;
			return new Vector3(-rect.center.x, -rect.center.y);
		}
		#endregion

		#region Shift Sides
		/// <summary>
		/// Сдвигает только левую грань на [value] пикселей
		/// </summary>
		/// <param name="self"></param>
		/// <param name="value"></param>
		public static void ShiftSideLeft(this RectTransform self, float value)
		{
			//Vector2 pivot = self.pivot;
			//self.pivot = new Vector2(1, 1);

			self.offsetMin += new Vector2(value, 0);
			//self.pivot = pivot;
		}
		public static void ShiftSideRight(this RectTransform self, float value)
		{
			self.offsetMax += new Vector2(value, 0);
		}
		public static void ShiftSideTop(this RectTransform self, float value)
		{
			self.offsetMax += new Vector2(0, value);
		}
		public static void ShiftSideBot(this RectTransform self, float value)
		{
			self.offsetMin += new Vector2(value, 0);
		}
		#endregion
	}
}