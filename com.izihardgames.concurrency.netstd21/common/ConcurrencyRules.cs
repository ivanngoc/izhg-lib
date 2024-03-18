using System;
using System.Collections.Generic;
using System.Linq;

namespace IziHardGames.Libs.NonEngine.Concurrency
{
	/// <summary>
	/// Configuration object.
	/// Set of rulest to resolve at ordering then elimination cycles.<br/>
	/// Can be shared for multiple <see cref="ConcurrencyItem"/><br/>
	/// Подход в сортировке ставит цель обходить строгое присвоение <see cref="ConcurrencyItem.order"/> так как при модификации списка придется обновлять все значения и сдвинутых объектов.
	/// Определение порядка становится относительным. Также при таком подходе должна быть проверка на dead-lock в случае если программист ошибается.
	/// Последовательность общая:
	/// -Приоритезация/сортировка (вне рантайма. запекается при запуска но прописывается в момент разработки)
	/// -Активация по внешним условиям: объект включен и учавствует в борьбе (этап внешней фильтрации уже пройден перед активацией. Определяется по флагу <see cref="ConcurrencyItem.isFiltered"/>)
	/// Группа ВЫКЛЮЧЕНИЯ
	/// -Уступание: из борьбы выпадают те которые явно выключаются теми кто активирован
	/// -Подавление: тоже самое что и уступание только наоборот. не имеет значение что происходи первым
	/// Группа СОДЕЙСТВИЯ
	/// -Втягивание: те объекты которые должны активироваться следом за оставшимися после борьбы объектами
	/// Группа 
	/// 
	/// Приоритет действий подавления (элиминации/уничтожения/избавления):
	/// -kill (если я активировался то даже если меня деактивируют я должен выключить. строгая иерархия приоритетов)
	/// -surrender (если меня выключили то все те кого я включал тоже должны выключиться)
	/// </summary>
	/// TODO: Также необходимо подумать о визуальном редактировании списка. например в эдиторе Unity.
	/// <remarks>
	/// Для избежания рекурсий нужно формировать правила без рекурсий то есть со строгой иерархией
	/// </remarks>
	public class ConcurrencyRules
	{
		public int idGroupe;
		/// <summary>
		/// Got direct connection to each items from list
		/// </summary>
		private List<ConcurrencyItem> relations = new List<ConcurrencyItem>();

		private ConcurrencyResolver concurrencyResolver;

		#region Ordering - Control order
		/// <summary>
		/// Items that goes before this item
		/// </summary>
		public List<ConcurrencyItem> beforeThis = new List<ConcurrencyItem>();
		/// <summary>
		/// Items that goes after this item
		/// </summary>
		public List<ConcurrencyItem> afterThis = new List<ConcurrencyItem>();
		#endregion

		#region Elimination - Control Activation
		/// <summary>
		/// This object-bind turns off items in list. Items that if activated must be eliminated. 
		/// <see cref="ConcurrencyItem.isEliminated"/> will be set to <see langword="true"/> if this item is activated
		/// </summary>
		public List<ConcurrencyItem> toSuppress = new List<ConcurrencyItem>();
		/// <summary>
		/// This object-bind is turned of by items in list. Items that this item must yeild to. If any of items is <see cref="ConcurrencyItem.isActivated"/> 
		/// </summary>
		public List<ConcurrencyItem> yeildTo = new List<ConcurrencyItem>();
		#endregion

		#region Conditional
		/// <summary>
		/// This object-bind is activated only when all of given objects are activated or not eliminated.
		/// </summary>
		public List<ConcurrencyItem> with = new List<ConcurrencyItem>();
		/// <summary>
		/// Очередь типов без конкретного экземпляра в <see cref="with"/>. Получается когда схема связей создается через Generic методы. Для этого списка метод <see cref="OnlyWith{T}"/>
		/// </summary>
		public List<Type> withNotLinked = new List<Type>();
		public List<Type> yiledToNotLinked = new List<Type>();
		public List<Type> eliminateNotLinked = new List<Type>();
		/// <summary>
		/// This object-bind is activated only when all of given objects NOT activated
		/// </summary>
		public List<ConcurrencyItem> without = new List<ConcurrencyItem>();

		internal ConcurrencyConditionChecker concurrencyConditionChecker;
		#endregion

		public ConcurrencyRules(int groupe)
		{
			idGroupe = groupe;
			concurrencyConditionChecker = new ConcurrencyConditionChecker();
		}

		internal void BindTo(ConcurrencyResolver concurrencyResolver)
		{
			this.concurrencyResolver = concurrencyResolver;
		}

		public ConcurrencyRules Eliminate<T>()
		{
#if DEBUG
			if (eliminateNotLinked.Contains(typeof(T))) throw new ArgumentException($"Type {typeof(T).FullName} is already existed in list");
#endif
			ConcurrencyItem existed = concurrencyResolver.items.FirstOrDefault(x => typeof(T).IsAssignableFrom(x.bind.GetType()));

			if (existed != null)
			{
				Eliminate(existed);
			}
			else
			{
				eliminateNotLinked.Add(typeof(T));
			}
			return this;
		}
		public ConcurrencyRules Eliminate(ConcurrencyItem concurrencyItem)
		{
			Add(concurrencyItem);
			toSuppress.Add(concurrencyItem);
			return this;
		}
		public ConcurrencyRules YieldTo(ConcurrencyItem concurrencyItem)
		{
			Add(concurrencyItem);
			yeildTo.Add(concurrencyItem);
			return this;
		}
		public ConcurrencyRules OnlyWith(ConcurrencyItem concurrencyItem)
		{
			Add(concurrencyItem);
			with.Add(concurrencyItem);
			return this;
		}
		/// <summary>
		/// Required <see cref="ConcurrencyItem"/> with <see cref="ConcurrencyItem.bind"/> typeof T must be <see cref="ConcurrencyItem.isActivated"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public ConcurrencyRules OnlyWith<T>()
		{
#if DEBUG
			if (withNotLinked.Contains(typeof(T))) throw new ArgumentException($"Type {typeof(T).FullName} is already existed in list");
#endif
			ConcurrencyItem existed = concurrencyResolver.items.FirstOrDefault(x => typeof(T).IsAssignableFrom(x.bind.GetType()));

			if (existed != null)
			{
				OnlyWith(existed);
			}
			else
			{
				withNotLinked.Add(typeof(T));
			}
			return this;
		}
		public ConcurrencyRules YeildTo<T>()
		{
#if DEBUG
			if (yiledToNotLinked.Contains(typeof(T))) throw new ArgumentException($"Type {typeof(T).FullName} is already existed in list");
#endif
			ConcurrencyItem existed = concurrencyResolver.items.FirstOrDefault(x => typeof(T).IsAssignableFrom(x.bind.GetType()));

			if (existed != null)
			{
				YieldTo(existed);
			}
			else
			{
				yiledToNotLinked.Add(typeof(T));
			}
			return this;
		}

		private void Add(ConcurrencyItem concurrencyItem)
		{
			if (relations.Contains(concurrencyItem)) throw new ArgumentException($"У объекта {GetHashCode()} уже имеется связь с объектом {concurrencyItem.GetHashCode()}");
			relations.Add(concurrencyItem);
		}

		/// <summary>
		/// Aproximate position of item in list. For more precise result graph-based approach should be used.
		/// </summary>
		/// <returns></returns>
		internal int GetAproxPriority()
		{
			return afterThis.Count - beforeThis.Count;
		}

		internal void CalculateConditionFlag()
		{
			concurrencyConditionChecker.Execute();
		}
		internal bool IsPassedPhase3()
		{
			foreach (var item in with)
			{
				if (!item.isActivated || item.isEliminated) return false;
			}

			foreach (var item in without)
			{
				if (item.isActivated || !item.isEliminated) return false;
			}
			return true;
		}
	}
}