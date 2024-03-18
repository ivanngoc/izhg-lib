using IziHardGames.Libs.NonEngine.Vectors;
using System;

namespace IziHardGames.Libs.NonEngine.UI.ReusableScrollList
{
	[Serializable]
	public class ScrollListPreset
	{
		/// <summary>
		/// Point to set first element at start position.
		/// From <see cref="origin"/> along <see cref="directionToScrollForward01"/>
		/// </summary>
		public Point3 pointAtScrollVectorVisibleStart;
		/// <summary>
		/// Point at which placed item will be still visible, but if continue scroll by any value will disappear (become not visible)
		/// </summary>
		public Point3 pointAtScrollVectorVisibleEnd;

		public Point3 directionToScrollForward01;
		public Point3 directionToScrollBackward01;

		/// <summary>
		/// Represents single item (Direction+Length). 
		/// Must be in same direction as <see cref="directionToScrollForward01"/>
		/// </summary>
		public Point3 vectorForSingleItem;
		/// <summary>
		/// Сдвиг слота (строки списка) относительно его положения на векторе между <see cref="ScrollWithReusableItems{TView, TBind}.pointAtScrollVectorHead"/> 
		/// и <see cref="ScrollWithReusableItems{TView, TBind}.pointAtScrollVectorTail"/>
		/// </summary>
		public Point3 offsetInSlot;
		/// <summary>
		/// By Pivot Position of visible rect.
		/// Center of scrolling.
		/// Must be between <see cref="pointAtScrollVectorVisibleStart"/> and <see cref="pointAtScrollVectorVisibleEnd"/>
		/// </summary>
		public Point3 origin;

		public float lenghtAlongScrollVectorForSingleSlot;
		public float lenghtAlongScrollVectorVisible;

		/// <summary>
		/// case <see cref="EScrollAxis.Horizontal"/> => <see cref="lenghtPadPerItemX"/>
		/// case <see cref="EScrollAxis.Vertical"/> => <see cref="lenghtPadPerItemY"/>
		/// case <see cref="EScrollAxis.CustomDirectionVector"/> is magnitute of vector
		/// </summary>
		public float lenghtPadAlongScrollAxis;
		public float lenghtPadPerItemX;
		public float lenghtPadPerItemY;

		public int countOfVisibleItemsMax;
		public int countOfVisibleItemsMin;

		public void SetSingleSlotVector(float lengthSingleSlot)
		{
			this.lenghtAlongScrollVectorForSingleSlot = lengthSingleSlot;
			vectorForSingleItem = directionToScrollForward01 * lengthSingleSlot;
		}

		public void Clear()
		{
			pointAtScrollVectorVisibleStart = Point2.NaN;
			pointAtScrollVectorVisibleEnd = Point2.NaN;

			directionToScrollForward01 = Point3.NaN;
			directionToScrollBackward01 = Point3.NaN;

			vectorForSingleItem = Point3.NaN;
			origin = Point3.NaN;

			lenghtAlongScrollVectorForSingleSlot = float.NaN;
			lenghtAlongScrollVectorVisible = float.NaN;
			lenghtPadPerItemX = float.NaN;
			lenghtPadPerItemY = float.NaN;

			countOfVisibleItemsMin = int.MinValue;
			countOfVisibleItemsMax = int.MinValue;
		}
	}
}
