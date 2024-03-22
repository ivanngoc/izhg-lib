using System;
using UnityEngine;

namespace IziHardGames.Libs.Engine.CustomTypes
{
	/// <summary>
	/// Позиция точек <see cref="Rect"/> внутри его parent <see cref="Rect"/><br/>
	/// Значения зафиксированы в пределах родительского Rect<br/>
	/// Координаты считаются как от левого нижнего угла с началом 0,0
	/// </summary>
	[Serializable]
	public struct RectPosLocal
	{
		public Vector2 center;
		public Vector2 topLeft;
		public Vector2 topRight;
		public Vector2 botLeft;
		public Vector2 botRight;

		public Vector2 anchorMin;
		public Vector2 anchorMax;
		public Vector2 origin;
		public static RectPosLocal Create(RectTransform target)
		{
			var parent = target.parent as RectTransform;

			var rect = target.rect;
			var rectParent = parent.rect;

			var origin = (Vector2)parent.position + rectParent.min;

			var rectPos = new RectPosLocal()
			{
				center = (Vector2)target.position - origin,
				botLeft = (Vector2)target.position + new Vector2(rect.xMin, rect.yMin) - origin,
				botRight = (Vector2)target.position + new Vector2(rect.xMax, rect.yMin) - origin,
				topLeft = (Vector2)target.position + new Vector2(rect.xMin, rect.yMax) - origin,
				topRight = (Vector2)target.position + new Vector2(rect.xMax, rect.yMax) - origin,
			};

			rectPos.origin = origin;

			rectPos.anchorMin = new Vector2()
			{
				x = Mathf.Clamp01(rectPos.botLeft.x / rectParent.width),
				y = Mathf.Clamp01(rectPos.botLeft.y / rectParent.height),
			};
			rectPos.anchorMax = new Vector2()
			{
				x = Mathf.Clamp01(rectPos.topRight.x / rectParent.width),
				y = Mathf.Clamp01(rectPos.topRight.y / rectParent.height),
			};

			return rectPos;
		}
	}

	[Serializable]
	public readonly struct RectPosGlobal
	{
		public readonly float minX;
		public readonly float minY;
		public readonly float maxX;
		public readonly float maxY;

		public float Width => maxX - minX;
		public float Height => maxY - minY;
		public float MidX => minX + (maxX - minX) / 2f;
		public float MidY => minY + (maxY - minY) / 2f;

		public Vector2 Center => GetCenter();
		public Vector2 BotLeft => new Vector2(minX, minY);
		public Vector2 TopLeft => new Vector2(minX, maxY);
		public Vector2 TopRight => new Vector2(maxX, maxY);
		public Vector2 BotRight => new Vector2(maxX, minY);

		public RectPosGlobal(Vector2 min, Vector2 max)
		{
			minX = min.x;
			minY = min.y;
			maxX = max.x;
			maxY = max.y;
		}
		public RectPosGlobal(float xMin, float yMin, float xMax, float yMax)
		{
			minX = xMin;
			minY = yMin;
			maxX = xMax;
			maxY = yMax;
		}
		public RectPosGlobal(RectTransform rectTransform)
		{
			minX = rectTransform.position.x + rectTransform.rect.xMin;
			minY = rectTransform.position.y + rectTransform.rect.yMin;
			maxX = rectTransform.position.x + rectTransform.rect.xMax;
			maxY = rectTransform.position.y + rectTransform.rect.yMax;
		}
		private Vector2 GetCenter()
		{
			var lenghtX = maxX - minX;
			var lenghtY = maxY - minY;

			var midX = minX + lenghtX / 2f;
			var midY = minY + lenghtY / 2f;

			return new Vector2(midX, midY);
		}

		public static bool operator ==(RectPosGlobal left, RectPosGlobal right)
		{
			return left.minX == right.minX && left.minY == right.minY && left.maxX == right.maxX && left.maxY == right.maxY;
		}
		public static bool operator !=(RectPosGlobal left, RectPosGlobal right)
		{
			return left.minX != right.minX && left.minY != right.minY && left.maxX != right.maxX && left.maxY != right.maxY;
		}
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public override string ToString()
		{
			return base.ToString();
		}
	}
}