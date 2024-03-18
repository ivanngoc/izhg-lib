using IziHardGames.Libs.NonEngine.Collections;
using IziHardGames.Libs.NonEngine.Math.Float;
using IziHardGames.Libs.NonEngine.Rectangles;
using IziHardGames.Libs.NonEngine.Rectangles.Helpers;
using IziHardGames.Libs.NonEngine.UI.Scrolling;
using IziHardGames.Libs.NonEngine.Vectors;
using System;
using System.Collections;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.UI.ReusableScrollList
{
	/// <summary>
	/// </summary>
	/// <typeparam name="TView"></typeparam>
	/// <typeparam name="TBind"></typeparam>
	[Serializable]
	public class ScrollWithReusableItems<TView, TBind> : ControlForScroll
		where TView : IReusableScrollItemView, IEquatable<TView>
	{
		public ScrollListOptions scrollListOptions;
		public ScrollListPreset scrollListPreset;

		/// <summary>
		/// Point of scroll between <see cref="ScrollListPreset.pointAtScrollVectorVisibleStart"/> and <see cref="ScrollListPreset.pointAtScrollVectorVisibleEnd"/>
		/// </summary>
		protected Point3 pointBetweenVisibleStartAndEnd;

		/// <summary>
		/// Start point of scroll vector as if there were not reusable items.
		/// </summary>
		protected Point3 pointAtScrollVectorHead;
		/// <summary>
		/// End point of scroll vector as if there were not reusable items.
		/// </summary>
		protected Point3 pointAtScrollVectorTail;

		/// <summary>
		/// Lerp(<see cref="pointScrollVectorStart"/>, <see cref="pointAtScrollVectorTail"/>, <see cref="scrollBarValue"/>)<br/>
		/// Point between <see cref="pointScrollVectorStart"/> and <see cref="pointAtScrollVectorTail"/> as if there were not reusable items
		/// </summary>
		protected Point3 pointBetweenHeadAndTail;


		protected float offsetMagnitude;
		/// <summary>
		/// If this value sum with <see cref="pointAtScrollVectorHead"/> it resulted to <see cref="ScrollListPreset.origin"/>.
		/// Represents vector with magnitude equal to scrolled distance (in forward direction)
		/// </summary>
		protected Point3 vectorForScrollOffset;
		protected Point3 vectorForScrollOffsetPrevious;
		protected Point3 vectorForScrollDelta;


		protected List<TBind> binds;
		protected QueueCircled<TView> views;

		public Point3[] positionsPerView;

		protected EScrollAxis eScrollAxis;


		protected int countOfLinesToFitVisibleCurrent;
		protected int countOfVisibleItemsCurrent;
		protected int countOfVisibleItemsBeforeScroll;
		protected int countOfBinds;


		protected int countDeltaToPutAtStart;
		protected int countDeltaToPutAtEnd;


		protected int indexOfBindVisibleAtStart;
		protected int indexOfBindVisibleAtStartPrevious;
		protected int indexOfBindVisibleAtEnd;
		protected int indexOfBindVisibleAtEndPrevious;


		protected float lenghtAlongScrollVectorTotal;

		protected float scrollBarValueFrameLast;
		protected float scrollBarValueDelta;
		protected float scrollBarValue;
		protected float scrollbarSize;
		/// <summary>
		/// —оотношение длины  видимой части к длине всего списка. ≈сли <1 то прокрутка доступна. 
		/// <see cref="isScrollAwailable"/> = <see langword="true"/>
		/// </summary>
		protected float ratioVisibleToTotalLength;
		/// <summary>
		/// Represent normilized value if visible part relative to total list length<br/>
		/// <see cref="partInvisibleAtStart01"/> + <see cref="partInvisible01"/> + <see cref="partInvisibleAtEnd01"/> = 1
		/// </summary>
		protected float partVisible01;
		/// <summary>
		/// Reverse value for <see cref="partVisible01"/>. (1-<see cref="partVisible01"/>)
		/// </summary>
		protected float partInvisible01;
		protected float partInvisibleAtStart01;
		protected float partInvisibleAtEnd01;

		/// <summary>
		/// Ќа сколько элементов изменилась лента. ¬сегда положительное. »зменение состава <see cref="monosActive"/>
		/// </summary>
		protected int changingDynamic;

		protected bool isShow;
		protected bool isForward;
		/// <summary>
		/// Ќе плавна€ прокрутка (прокрутка на более чем размер страницы/видимой части)
		/// </summary>
		protected bool isScrollPage;
		/// <summary>
		/// в списке либо по€вились либо убавились выдимые элементы
		/// </summary>
		protected bool isItemsChanged;
		protected bool isScrollAwailable;

		protected Action<TBind, TView> viewUpdate;
		protected Action<TView> viewUpdateOnReuse;
		protected Action<TView> viewUpdateOnReuseReverse;
		protected virtual float ScrollBarValueFromSource { get => throw new NotImplementedException(); }

		public ScrollWithReusableItems()
		{

		}

		public virtual void Initilize()
		{
			BuildScrollList();
		}
		/// <summary>
		/// Build all rects and fill it with items. Fit items to scrollbar value position
		/// Not controlling visibility.
		/// </summary>
		protected virtual void BuildScrollList()
		{
			UpdateDatasOfRectSimulated();
			SetDatasForScrolling(scrollBarValue);
			CreateScrollElements();
			ContentModifyOnScrollPage(scrollBarValue);
		}


		/// <summary>
		/// 
		/// </summary>
		protected void FitItemsToVisibleArea()
		{

		}


		protected virtual void CreateScrollElements()
		{
			positionsPerView = new Point3[scrollListPreset.countOfVisibleItemsMax];
			views = new QueueCircled<TView>(scrollListPreset.countOfVisibleItemsMax);
		}

		#region Updating
		/// <summary>
		/// Call this Update after <see cref="binds"/> were changed.
		/// Doesn't apply any change to view state of scroll list.
		/// </summary>
		public virtual void UpdateOnContentChange()
		{
			UpdateDatasOfRectSimulated();
		}
		protected virtual void UpdateDatasOfRectSimulated()
		{
#if UNITY_EDITOR
			if (scrollListPreset.lenghtAlongScrollVectorForSingleSlot < MathForFloat.Epsilon) throw new ArgumentOutOfRangeException($"[{nameof(scrollListPreset.lenghtAlongScrollVectorForSingleSlot)}] is not set");
#endif
			countOfBinds = binds.Count;
			lenghtAlongScrollVectorTotal = scrollListPreset.lenghtAlongScrollVectorForSingleSlot * countOfBinds;
			ratioVisibleToTotalLength = scrollListPreset.lenghtAlongScrollVectorVisible / lenghtAlongScrollVectorTotal;

			this.partVisible01 = MathForFloat.Clamp01(ratioVisibleToTotalLength);
			this.partInvisible01 = 1 - partVisible01;

			UpdateDataForScrollbar();
			SetScrollAwailableFlag();
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void UpdateDatasOfRectVisible()
		{
			FitItemsToVisibleArea();
		}

		protected virtual void UpdateDataForScrollbar()
		{
			scrollbarSize = MathForFloat.Clamp01(scrollListPreset.lenghtAlongScrollVectorVisible / this.lenghtAlongScrollVectorTotal);
		}
		#endregion

		#region Configure
		protected virtual void SetDirection(Point3 vector)
		{
			scrollListPreset.directionToScrollForward01 = vector;
			scrollListPreset.directionToScrollBackward01 = vector * -1;
		}

		protected void SetDataForSingleSlot(float lengthSingleSlot)
		{
			scrollListPreset.SetSingleSlotVector(lengthSingleSlot);
		}

		protected void SetScrollAwailableFlag()
		{
			isScrollAwailable = ratioVisibleToTotalLength < 1;
		}

		protected virtual void SetStartPoint(Point3 startPoint)
		{
			scrollListPreset.origin = startPoint;
		}
		protected virtual void SetVisibleRectPoints(Rect2D rectVisibleArea)
		{
			Rectangle2D rect = new Rectangle2D(rectVisibleArea);
			JobFindRectangleIntersect jobFindRectangleIntersect = new JobFindRectangleIntersect(in rect, scrollListPreset.origin, scrollListPreset.directionToScrollForward01, false);
			jobFindRectangleIntersect.FindIntersects();
			if (jobFindRectangleIntersect.Count != 2) throw new ArgumentOutOfRangeException("Origin Point must be inside visible rect and intersections == 2");
			var p1 = jobFindRectangleIntersect[0];
			var p2 = jobFindRectangleIntersect[1];
			var origin = (Point2)scrollListPreset.origin;
			var p1Magn = (p1 - origin).Magnitude;
			var p2Magn = (p2 - origin).Magnitude;

			if (p1Magn > p2Magn)
			{
				scrollListPreset.pointAtScrollVectorVisibleStart = p2;
				scrollListPreset.pointAtScrollVectorVisibleEnd = p1;
			}
			else
			{
				scrollListPreset.pointAtScrollVectorVisibleStart = p1;
				scrollListPreset.pointAtScrollVectorVisibleEnd = p2;
			}
			scrollListPreset.lenghtAlongScrollVectorVisible = (p1 - p2).Magnitude;


			scrollListPreset.countOfVisibleItemsMin = (int)System.Math.Ceiling(scrollListPreset.lenghtAlongScrollVectorVisible / scrollListPreset.lenghtAlongScrollVectorForSingleSlot);
			scrollListPreset.countOfVisibleItemsMax = scrollListPreset.countOfVisibleItemsMin + 1;
		}
		/// <summary>
		/// Call when rect is resized for example on resolution changing. 
		/// Also using for store predefined values in serilizable object
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="rectVisibleArea"></param>
		/// <param name="axisForScrollForward"></param>
		/// <param name="paddings"></param>
		/// <param name="lengthForSingleSlot"></param>
		public void SetPreset(Point3 origin, Rect2D rectVisibleArea, Point3 axisForScrollForward, Point3 paddings, Point3 offsetInSlot, float lengthForSingleSlot)
		{
			scrollListPreset.offsetInSlot = offsetInSlot;
			scrollListPreset.lenghtPadAlongScrollAxis = paddings.Magnitude;
			SetDirection(axisForScrollForward);
			SetDataForSingleSlot(lengthForSingleSlot);
			SetStartPoint(origin);
			SetVisibleRectPoints(rectVisibleArea);
		}
		#endregion

		#region Content Control
		public virtual void ItemAdd(TBind bind)
		{
			binds.Add(bind);
			UpdateDatasOfRectSimulated();
			throw new NotImplementedException();
		}
		public virtual void ItemInsert(TBind itemm, int index)
		{
			throw new NotImplementedException();
		}
		public virtual void ItemRemove(TBind bind)
		{
			binds.Remove(bind);
			UpdateDatasOfRectSimulated();
			throw new System.NotImplementedException();
		}

		protected virtual void RebindScrollItemsCorrespond()
		{

		}

		protected void PairingViewWithBind(int indexOfView, int indexOfBind)
		{
			TView mono = views[indexOfView];
			PairingViewWithBind(mono, indexOfBind);
		}

		protected virtual void PairingViewWithBind(TView mono, int indexOfBind)
		{

		}

		protected virtual void PairingViewWithBindReverse(TView mono)
		{

		}

		private TView ScrollItemSetAsFreeLast()
		{
			TView mono = views.ShrinkLast();
			PairingViewWithBindReverse(mono);
			return mono;
		}
		private TView ScrollItemSetAsFreeFirst()
		{
			TView mono = views.ShrinkHead();
			PairingViewWithBindReverse(mono);
			return mono;
		}
		private void ScrollItemSetAsFree(int index)
		{
			TView mono = views.GetByIndex(index);
			views.RemoveAt(index);
			PairingViewWithBindReverse(mono);
		}


		protected virtual void ContentModifyOnScrollStep(float scrollbarValue01)
		{
			if (isItemsChanged)
			{
				// —начала убираем убывающие элементы
				// decrese the end of the list when scroll up
				if (countDeltaToPutAtEnd < 0)
				{
					for (int i = countDeltaToPutAtEnd; i < 0; i++)
					{
						var mono = ScrollItemSetAsFreeLast();
						viewUpdateOnReuseReverse?.Invoke(mono);
					}
				}
				// decrease the begining of the list when scrolls up
				if (countDeltaToPutAtStart < 0)
				{
					for (int i = countDeltaToPutAtStart; i < 0; i++)
					{
						var mono = ScrollItemSetAsFreeFirst();
						viewUpdateOnReuseReverse?.Invoke(mono);
					};
				}

				// затем добавл€ем прибывающие
				// increase the end of the list when scroll down
				if (countDeltaToPutAtEnd > 0)
				{
					for (int i = 0; i < countDeltaToPutAtEnd; i++)
					{
						TView view = views.ExtendAfterLast();
						PairingViewWithBind(view, indexOfBindVisibleAtEndPrevious + i + 1);
						viewUpdateOnReuse?.Invoke(view);
					}
				}

				if (countDeltaToPutAtStart > 0)
				{
					for (int i = 0; i < countDeltaToPutAtStart; i++)
					{
						TView view = views.ExtendBeforeHead();
						PairingViewWithBind(view, indexOfBindVisibleAtStartPrevious - i - 1);
						viewUpdateOnReuse?.Invoke(view);
					}
				}
			}
			MatchViewPositions();
		}
		protected virtual void ContentModifyOnScrollPage(float scrollbarValue01)
		{
			for (int i = 0; i < views.Count; i++)
			{
				TView view = views[i];
				PairingViewWithBindReverse(view);
				if (isShow) viewUpdateOnReuseReverse?.Invoke(view);
			}
			views.Reset();
			for (int i = 0; i < countOfVisibleItemsCurrent; i++)
			{
				views.ExtendAfterLast();
				TView view = views[i];
				PairingViewWithBind(view, i + indexOfBindVisibleAtStart);
				if (isShow) viewUpdateOnReuse?.Invoke(view);
			}
			MatchViewPositions();
		}
		#endregion

		#region Handle scrolling
		protected virtual void SetDataForScrollbar(float scrollBarValue)
		{
			scrollBarValueDelta = scrollBarValue - scrollBarValueFrameLast;
			scrollBarValueFrameLast = this.scrollBarValue;
			this.scrollBarValue = MathForFloat.Clamp01(scrollBarValue);
			// если движемс€ вперед (вниз)
			isForward = scrollBarValueDelta < 0;

			vectorForScrollOffsetPrevious = vectorForScrollOffset;

			if (isScrollAwailable)
			{
				vectorForScrollOffset = scrollListPreset.directionToScrollForward01 * ((lenghtAlongScrollVectorTotal - scrollListPreset.lenghtAlongScrollVectorVisible) * scrollBarValue);
				vectorForScrollDelta = vectorForScrollOffset - vectorForScrollOffsetPrevious;
				offsetMagnitude = vectorForScrollOffset.Magnitude;
			}
			else
			{
				vectorForScrollOffset = Point3.zero;
				vectorForScrollDelta = Point3.zero;
				offsetMagnitude = 0;
			}


			pointAtScrollVectorHead = scrollListPreset.origin + (scrollListPreset.directionToScrollBackward01 * vectorForScrollOffset.Magnitude);
			pointAtScrollVectorTail = pointAtScrollVectorHead + scrollListPreset.directionToScrollForward01 * lenghtAlongScrollVectorTotal;
			pointBetweenHeadAndTail = Point3.Lerp(pointAtScrollVectorHead, pointBetweenHeadAndTail, scrollBarValue);

			isScrollPage = vectorForScrollDelta.Magnitude >= scrollListPreset.lenghtAlongScrollVectorVisible;
		}
		protected virtual void CalculateScrollItteration(float scrollbarValue)
		{
			if (isScrollAwailable)
			{
				partInvisibleAtStart01 = (pointAtScrollVectorHead - scrollListPreset.pointAtScrollVectorVisibleStart).Magnitude / lenghtAlongScrollVectorTotal;
				partInvisibleAtEnd01 = MathForFloat.Clamp01(partInvisibleAtStart01 + partVisible01);

				indexOfBindVisibleAtStartPrevious = indexOfBindVisibleAtStart;
				indexOfBindVisibleAtEndPrevious = indexOfBindVisibleAtEnd;

				SetDataForVisibleRange();

				pointBetweenVisibleStartAndEnd = scrollListPreset.directionToScrollBackward01.normalized * (partInvisibleAtStart01 * lenghtAlongScrollVectorTotal);

				countDeltaToPutAtStart = indexOfBindVisibleAtStartPrevious - indexOfBindVisibleAtStart;
				countDeltaToPutAtEnd = indexOfBindVisibleAtEnd - indexOfBindVisibleAtEndPrevious;

				changingDynamic = System.Math.Abs(countDeltaToPutAtStart + countDeltaToPutAtEnd);
				isItemsChanged = changingDynamic != 0;
			}
			else
			{
				indexOfBindVisibleAtStart = 0;
				indexOfBindVisibleAtEnd = System.Math.Clamp(binds.Count - 1, 0, binds.Count);
				countOfLinesToFitVisibleCurrent = scrollListPreset.countOfVisibleItemsMin;
				countOfVisibleItemsCurrent = binds.Count;

				countDeltaToPutAtStart = 0;
				countDeltaToPutAtEnd = 0;
				changingDynamic = 0;
				isItemsChanged = false;
			}
		}

		protected virtual void SetPositionsMatchingToScrollbarValue(float scrollbarValue)
		{

		}

		protected virtual void SetDatasForScrolling(float newValueOfScrollBar)
		{
			SetDataForScrollbar(newValueOfScrollBar);
			CalculateScrollItteration(newValueOfScrollBar);
		}
		/// <summary>
		/// set foreach visible view appropriate position
		/// </summary>
		protected virtual void MatchViewPositions()
		{
			if (binds.Count > 0)
			{
				var offsetInSlot = scrollListPreset.offsetInSlot;
				var offsetBetweenItems = scrollListPreset.vectorForSingleItem;
				var positionOfVisibleStart = CalcPos(0);

				for (int i = 0; i < countOfVisibleItemsCurrent; i++)
				{
					positionsPerView[i] = positionOfVisibleStart + offsetInSlot + (offsetBetweenItems * i);
				}
			}
		}
		protected Point3 CalcPos(int indexVisible)
		{
			int actualIndex = indexOfBindVisibleAtStart + indexVisible;
			float lenghtTillThis = actualIndex * scrollListPreset.lenghtAlongScrollVectorForSingleSlot;
			var offsetFromHead = lenghtTillThis * scrollListPreset.directionToScrollForward01;
			return offsetFromHead + pointAtScrollVectorHead;
		}

		/// <summary>
		/// Instant set scrolllist to given position of scrollbar and update views. Reset previous state
		/// </summary>
		/// <remarks>
		/// Use to programmaticly set scrollList state to given value Including position of scrollbar itself.
		/// Use for first appearance and set remembered previous scrollbar position value.
		/// </remarks>
		/// <param name="scrollBarValue01"></param>
		public virtual void ScrollSetTo(float scrollBarValue01)
		{
			SetDatasForScrolling(scrollBarValue01);
			ContentModifyOnScrollPage(scrollBarValue01);
			MatchViewPositions();
		}
		/// <summary>
		/// Navigate with updating views. 
		/// Reset previous state if delta of scroll distance between previous itteration and current is more than distance of one visible page.
		/// </summary>
		/// <remarks>
		/// Use to apply changes after user manipulate scrollbar value.
		/// Subscribe as event handler on scrollbar value changing.
		/// </remarks>
		/// <param name="scrollBarValue01"></param>
		public virtual void ScrollNavigateTo(float scrollBarValue01)
		{
			SetDatasForScrolling(scrollBarValue01);
			if (isScrollPage)
			{
				ContentModifyOnScrollPage(scrollBarValue01);
			}
			else
			{
				ContentModifyOnScrollStep(scrollBarValue01);
			}
			MatchViewPositions();
		}
		public virtual void ScrollByLength(float length)
		{
			throw new NotImplementedException();
		}
		public virtual void ScrollByItems(int count)
		{
			throw new NotImplementedException();
		}
		public virtual void ScrollToPosition(Point3 vector3Sur)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Visibility control
		public virtual void ShowAll()
		{
			isShow = true;
		}
		public virtual void HideAll()
		{
			isShow = false;
		}
		#endregion

		#region Getters
		public Point3 GetHead() => pointAtScrollVectorHead;
		public Point3 GetTail() => pointAtScrollVectorTail;
		public Point3 GetPosAsPoint3(int index) => positionsPerView[views.SequenceIndexToActualIndex(index)];

		private void SetDataForVisibleRange()
		{
			int countInvisibleFromStart = (int)System.MathF.Floor(offsetMagnitude / scrollListPreset.lenghtAlongScrollVectorForSingleSlot);
			indexOfBindVisibleAtStart = countInvisibleFromStart;
			float distanceVisibleBegin = scrollListPreset.lenghtAlongScrollVectorForSingleSlot * countInvisibleFromStart;
			float lengthInvisibleOfFirstItem = offsetMagnitude - distanceVisibleBegin;
			float lengthVisibleOfFirstItem = scrollListPreset.lenghtAlongScrollVectorForSingleSlot - lengthInvisibleOfFirstItem;
			var localIndexVisibleEnd = (int)System.MathF.Ceiling((scrollListPreset.lenghtAlongScrollVectorVisible - lengthVisibleOfFirstItem) / scrollListPreset.lenghtAlongScrollVectorForSingleSlot);
			countOfLinesToFitVisibleCurrent = 1 + localIndexVisibleEnd;
			indexOfBindVisibleAtEnd = System.Math.Clamp(localIndexVisibleEnd + indexOfBindVisibleAtStart, indexOfBindVisibleAtStart, binds.Count - 1);

			countOfVisibleItemsBeforeScroll = countOfVisibleItemsCurrent;
			countOfVisibleItemsCurrent = indexOfBindVisibleAtEnd - indexOfBindVisibleAtStart + 1;
		}
		#endregion

		public override void Clean()
		{
			base.Clean();

			countOfLinesToFitVisibleCurrent = int.MinValue;
			countOfBinds = int.MinValue;
			countOfVisibleItemsBeforeScroll = int.MinValue;
			isForward = default;
			scrollbarSize = float.NaN;

			views?.Clear();
			binds?.Clear();
			scrollListPreset.Clear();
		}
	}
}
