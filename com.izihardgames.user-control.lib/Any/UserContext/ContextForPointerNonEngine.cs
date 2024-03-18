using IziHardGames.UserControl.Lib.Contexts.PredefindedSelectors;
using IziHardGames.Libs.Sorting;
using System;
using System.Collections.Generic;
using IziHardGames.UserControl.Abstractions.NetStd21.Contexts;

namespace IziHardGames.UserControl.Lib.Contexts
{
	public class ContextForPointerNonEngine : UserContext
	{
		private List<Selector> selectorsByOrder;
		private readonly Dictionary<Type, Selector> selectors;
		protected object onTopObjectUnderPointer;

		public ContextForPointerNonEngine(int prealocatedSize) : base()
		{
			selectors = new Dictionary<Type, Selector>(prealocatedSize);
			selectorsByOrder = new List<Selector>(prealocatedSize);
		}

		protected override void Grab()
		{
			base.Grab();

			foreach (var selector in selectors.Values)
			{
				selector.Execute();
			}

			onTopObjectUnderPointer = null;

			for (int i = 0; i < selectorsByOrder.Count; i++)
			{
				if (selectorsByOrder[i].isHit)
				{
					onTopObjectUnderPointer = selectorsByOrder[i].resultObj;
					return;
				}
			}
		}


		public void AddSelector<T>(Func<(bool, object)> func, int order = int.MaxValue)
		{
			Selector selector = new Selector(typeof(T)) { func = func, indexOrder = order };
			selectors.Add(typeof(T), selector);
			selectorsByOrder.Add(selector);
			SortingIList.SortSelectionAscending<List<Selector>, Selector, int>(selectorsByOrder, x => x.indexOrder);
		}
		/// <summary>
		/// Use incase preallocated <see cref="selectors"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <param name="index"></param>
		public void AddOrUpdateSelector<T>(Func<(bool, object)> func, int order = int.MaxValue)
		{
			if (selectors.TryGetValue(typeof(T), out Selector selector))
			{
				selector.func = func;
				selector.indexOrder = order;
			}
			else
			{
				selector = new Selector(typeof(T)) { func = func, indexOrder = order };
				selectors.Add(typeof(T), selector);
				selectorsByOrder.Add(selector);
			}
			SortingIList.SortSelectionAscending<List<Selector>, Selector, int>(selectorsByOrder, x => x.indexOrder);
		}

		public Selector GetSelector<T>()
		{
			return selectors[typeof(T)];
		}
		public Func<(bool, object)> GetFunc<T>()
		{
			return selectors[typeof(T)].func;
		}

		public bool GetPointerResult<T>()
		{
			return selectors[typeof(T)].isHit;
		}
		public object GetOnTopAsObject()
		{
			return onTopObjectUnderPointer;
		}
		public T GetOnTopAs<T>() where T : class
		{
			return onTopObjectUnderPointer as T;
		}
		public bool TryGetOnTopAs<T>(out T result) where T : class
		{
			result = onTopObjectUnderPointer as T;
			return result != null;
		}
		public bool TryGetOnTopUIAs<T>(out T result) where T : class
		{
			if (onTopObjectUnderPointer is PointerTargetAsUI wrap)
			{
				if (wrap.target != null)
				{
					if (wrap.target is T res)
					{
						result = res;
						return true;
					}
				}
			}
			result = default;
			return false;
		}
	}

	public class Selector
	{
		public Func<(bool, object)> func;
		public object resultObj;
		public bool isHit;
		public readonly Func<bool> funcGetResult;
		public readonly Func<bool> funcGetResultReverse;
		public Type type;
		public int indexOrder;

		public Selector(Type type)
		{
			this.type = type;
			funcGetResult = () => isHit;
			funcGetResultReverse = () => !isHit;
		}
		internal void Execute()
		{
			var cortage = func();
			resultObj = cortage.Item2;
			isHit = cortage.Item1;
		}
	}

}

namespace IziHardGames.UserControl.Lib.Contexts.PredefindedSelectors
{
	public interface ISelector
	{
		Func<bool> Selector { get; }
	}
	public class PredefinedPointer : ISelector
	{
		public Func<bool> Selector { get => throw new NotImplementedException(); }
	}
	public class Any : PredefinedPointer
	{

	}
	/// <summary>
	/// 
	/// </summary>
	public class Terrain : PredefinedPointer
	{

	}
	/// <summary>
	/// Empty space. Ray from camera to point is infinite and does not hit anything
	/// </summary>
	public class PointerTargetAsVoid : PredefinedPointer
	{
		public readonly static PointerTargetAsVoid Default;
		static PointerTargetAsVoid()
		{
			Default = new PointerTargetAsVoid();
		}
		public PointerTargetAsVoid()
		{

		}
	}
	public class PointerTargetAsUI : PredefinedPointer
	{
		public object target;
		public readonly static PointerTargetAsUI Default;

		static PointerTargetAsUI()
		{
			Default = new PointerTargetAsUI();
		}

		public static object Wrap(object obj)
		{
			Default.target = obj;
			return Default;
		}
	}
	/// <summary>
	/// Pointer over game window. Focused or Not Doesn't matter
	/// </summary>
	public class GameWindowAny : PredefinedPointer
	{

	}
	/// <summary>
	/// Pointer Over game window and window is active (active + focused? To Check WinAPI)
	/// </summary>
	public class GameWindowFocused : PredefinedPointer
	{

	}
}
