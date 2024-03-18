using System.Linq;

namespace UnityEngine
{

	public static class ExtensionRect
	{
		public static Rect[] RectSplitIntoLineHorizontal(this Rect rect, int count)
		{
			Rect[] rects = new Rect[count];

			for (int i = 0; i < count; i++)
			{
				rects[i] = new Rect(rect.x + rect.width * i / count, rect.y, rect.width * (1f / count), rect.height);
			}
			return rects;
		}
		public static Rect RectSplitIntoLineHorizontal(this Rect rect, int count, int i)
		{
			return new Rect(rect.x + rect.width * i / count, rect.y, rect.width * (1f / count), rect.height);
		}
		public static Rect RectSplitIntoLineHorizontal(this Rect rect, int count, int i, float widthPadInterpolated)
		{
			float width = rect.width;

			float pad = width * widthPadInterpolated;

			return new Rect(rect.x + width * i / count, rect.y, width * (1f / count) - pad, rect.height);
		}

		/// <summary>
		/// Возвращает неравномерный сплит Rect'a с постоянной высотой. <br/>
		/// Каждый сплит пропорционален по ширине заданному параметру <br/>
		/// Non alloc
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="i"></param>
		/// <param name="pad">0..1 interpolated </param>
		/// <param name="percentages">0-100% each</param>
		/// <returns></returns>
		public static void GetSplitWithPropotion(this Rect rect, float pad, Rect[] result, params int[] percentages)
		{
			if (percentages.Sum() > 100) throw new System.Exception(">100");

			int count = percentages.Length;

			float widthOffset = default;

			pad = rect.width * pad;

			for (int i = 0; i < count; i++)
			{
				result[i] = new Rect(widthOffset,
									 rect.y,
									 rect.width * (percentages[i] / 100f) - pad,
									 rect.height);

				widthOffset += result[i].width;
			}
		}
	}

}