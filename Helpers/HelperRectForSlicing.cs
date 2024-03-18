using System;
using UnityEngine;

namespace IziHardGames.Libs.Engine.Helpers
{
	public static class HelperRectForSlicing
	{

	}


	public struct RectSlicing
	{
		public float xPad;
		public Rect origin;

		public float xCurrent;
		public float yCurrent;

		public float left01;
		public float leftWidth;

		public RectSlicing(Rect rect) : this()
		{
			this.origin = rect;
			left01 = 1;
			xCurrent = rect.x;
			yCurrent = rect.y;
			leftWidth = rect.width;
		}
		public RectSlicing(Rect rect, float xPad) : this(rect)
		{
			this.xPad = xPad;
		}

		public Rect GetSliceAlongXWithWidth(float widthToTake)
		{
			float partition = widthToTake/origin.width;

			if (partition > 1)
			{
				return new Rect(xCurrent, yCurrent, 0, origin.height);
			}

			if (leftWidth < widthToTake)
			{
				return new Rect(xCurrent, yCurrent, 0, origin.height);
			}

			left01 -= partition;
			leftWidth -= widthToTake;
			float xToGet = xCurrent;
			float xAfterShift = xCurrent + widthToTake;
			xCurrent = xAfterShift;
			return new Rect(xToGet, yCurrent, widthToTake, origin.height);
		}
		public Rect GetSliceAlongXWithWidthAndPad(float widthToTake)
		{
			float partition = (widthToTake+xPad)/origin.width;

			if (partition > 1)
			{
				return new Rect(xCurrent, yCurrent, 0, origin.height);
			}

			if (leftWidth < widthToTake)
			{
				return new Rect(xCurrent, yCurrent, 0, origin.height);
			}

			left01 -= partition;
			leftWidth -= widthToTake;
			float xToGet = xCurrent;
			float xAfterShift = xCurrent + xPad + widthToTake;
			xCurrent = xAfterShift;
			return new Rect(xToGet, yCurrent, widthToTake, origin.height);
		}
		public Rect GetSliceAlongXWithPad(float partition01)
		{
			partition01 += (xPad / origin.width);

			if (left01 < partition01)
			{
				return new Rect(xCurrent, yCurrent, 0, origin.height);
			}

			float widthToTake =  origin.width * partition01;

			leftWidth -= widthToTake;
			left01 -= partition01;

			float xToGet = xCurrent;
			float xAfterShift = xCurrent + xPad + widthToTake;
			xCurrent = xAfterShift;

			return new Rect(xToGet, yCurrent, widthToTake, origin.height);
		}
		public Rect GetSliceAlongX(float partition01)
		{
			if (left01 < partition01)
			{
				return new Rect(xCurrent, yCurrent, 0, origin.height);
			}

			float widthToTake =  origin.width * partition01;

			leftWidth -= widthToTake;
			left01 -= partition01;

			float xToGet = xCurrent;
			float xAfterShift = xCurrent +  widthToTake;
			xCurrent = xAfterShift;

			return new Rect(xToGet, yCurrent, widthToTake, origin.height);
		}
		/// <summary>
		/// ПОлучить оставшийся кусок
		/// </summary>
		/// <param name="partition01"></param>
		/// <returns></returns>
		public Rect GetReminder(float pad)
		{
			return new Rect(xCurrent, yCurrent, leftWidth - pad, origin.height);
		}
	}
}