using IziHardGames.Libs.Engine.CustomTypes;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Helpers
{

	public static class HelperRect
	{
		public static RectPosGlobal CalculateBounds(RectTransform[] transforms)
		{
			if (transforms.Length < 1) return default;

			var minX = transforms[0].position.x;
			var minY = transforms[0].position.y;

			var maxX = transforms[0].position.x;
			var maxY = transforms[0].position.y;

			for (var i = 0; i < transforms.Length; i++)
			{
				var transform = transforms[i];

				var rect = transform.rect;

				Vector2 posOrigin = transform.position;

				var pos = posOrigin + rect.min;

				if (minX > pos.x) minX = pos.x;
				if (minY > pos.y) minY = pos.y;

				if (maxX < pos.x) maxX = pos.x;
				if (maxY < pos.y) maxY = pos.y;

				pos = posOrigin + new Vector2(rect.xMin, rect.yMax);

				if (minX > pos.x) minX = pos.x;
				if (minY > pos.y) minY = pos.y;

				if (maxX < pos.x) maxX = pos.x;
				if (maxY < pos.y) maxY = pos.y;

				pos = posOrigin + new Vector2(rect.xMax, rect.yMin);

				if (minX > pos.x) minX = pos.x;
				if (minY > pos.y) minY = pos.y;

				if (maxX < pos.x) maxX = pos.x;
				if (maxY < pos.y) maxY = pos.y;

				pos = posOrigin + rect.max;

				if (minX > pos.x) minX = pos.x;
				if (minY > pos.y) minY = pos.y;

				if (maxX < pos.x) maxX = pos.x;
				if (maxY < pos.y) maxY = pos.y;
			}

			return new RectPosGlobal(minX, minY, maxX, maxY);
		}
	}

}