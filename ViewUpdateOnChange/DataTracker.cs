using IziHardGames.Core;
using IziHardGames.Libs.NonEngine.Events;
using System;
using System.Collections.Generic;

namespace IziHardGames.Patterns.ViewUpdateOnChange
{
	public interface IDataTracker
	{


	}

	public interface IDataTrackable : IUnique
	{

	}

	/// <summary>
	/// Регистр изменений за определенный цикл
	/// Предоставляет возможность отследить изменения по конкретному типу объекта
	/// Работает по принципу пуш в него изменения
	/// </summary>
	/// <remarks>
	/// TBD: persistant change flag: было внесено изменение но форма не была открыта и обновлена в этом цикле. А последующие циклы не фиксировали изменений.
	/// Таким образом форма останется не обнолвенной
	/// Самый оптимальный способ это делать пуш в места востребования
	/// TBD: correspond with specific sender. Make child class
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class DataTracker<T> : IDataTracker
		where T : IDataTrackable, IEquatable<T>
	{
		public static DataTracker<T> Shared { get => GetOrCreateDefault(); }
		private static DataTracker<T> shared;

		public static readonly List<T> changedItems = new List<T>(100);
		public static readonly List<ECrudOperation> changedItemsRespectively = new List<ECrudOperation>(100);

		public static DataTracker<T> instance;
		public static bool isAtLeastOneChange;

		public static event Action OnChangeAnyEvent;
		public static event Action<T> OnChangeConcreteEvent;
		public static readonly Dictionary<int, EventPublisher<T>> notifiersPerEvent = new Dictionary<int, EventPublisher<T>>();

		//public event Action<T> OnRegistAddEvent;


		#region Unity Message	

		#endregion

		public DataTracker()
		{
			instance = this;
		}
		/// <summary>
		/// Начать цикл отслеживания
		/// </summary>
		public void CycleStart()
		{

		}
		/// <summary>
		/// Закончить цикл отслеживания
		/// </summary>
		public void CycleEnd()
		{
			Reset();
		}
		/// <summary>
		/// Зафиксировать изменение объекта
		/// </summary>
		/// <param name="itemArg"></param>
		/// <param name="eCrudOperationArf"></param>
		public static void ChangeRegist(T itemArg, ECrudOperation eCrudOperationArg)
		{
			changedItems.Add(itemArg);

			changedItemsRespectively.Add(eCrudOperationArg);

			if (notifiersPerEvent.TryGetValue((int)eCrudOperationArg, out EventPublisher<T> publisher))
			{
				publisher.Invoke(itemArg);
			}
			OnChangeAnyEvent?.Invoke();

			OnChangeConcreteEvent?.Invoke(itemArg);

			isAtLeastOneChange = true;
		}

		public void ChangeConsume(T itemArg, ECrudOperation eCrudOperation)
		{

		}

		public static bool TryGetChanges(ref List<T> result, ECrudOperation eCrudOperation)
		{
			bool isAtLeastOne = default;

			for (int i = 0; i < changedItemsRespectively.Count; i++)
			{
				if (changedItemsRespectively[i] == eCrudOperation)
				{
					result.Add(changedItems[i]);

					isAtLeastOne = true;
				}
			}
			return isAtLeastOne;
		}

		public static void ChangeRemove(T dataInventoryItem, ECrudOperation eCrudOperation)
		{
			for (int i = 0; i < changedItems.Count; i++)
			{
				if (changedItems[i].Equals(dataInventoryItem) && changedItemsRespectively[i] == eCrudOperation)
				{
					changedItems.RemoveAt(i);

					changedItemsRespectively.RemoveAt(i);

					i--;
				}
			}
		}
		/// <summary>
		/// Флаг:<br/>
		/// <see cref="true"/> - етсь изменения по всему типу. То есть хотя бы 1 объект был изменен<br/>
		/// <see cref="false"/> - изменений нет<br/>
		/// </summary>
		/// <param name="itemArg"></param>
		/// <returns></returns>
		public static bool IsChanged()
		{
			return isAtLeastOneChange || changedItems.Count > 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="itemArg"></param>
		/// <returns></returns>
		public static bool IsHasChanged(T itemArg)
		{
			return changedItems.Contains(itemArg);
		}

		public static void TrackChangesAnyStop(Action handler)
		{
			OnChangeAnyEvent -= handler;
		}
		public static void TrackChangesAny(Action handler)
		{
			OnChangeAnyEvent += handler;
		}
		public static void TrackChangesSpecific(Action<T> handler)
		{
			OnChangeConcreteEvent += handler;
		}
		public static void TrackChangesSpecific(Action<T> handler, ECrudOperation eCrudOperation)
		{
			int operation = (int)eCrudOperation;

			if (notifiersPerEvent.TryGetValue(operation, out EventPublisher<T> publisher))
			{
				publisher.AddListener(handler);
			}
			else
			{
				publisher = new EventPublisher<T>();

				publisher.AddListener(handler);

				notifiersPerEvent.Add(operation, publisher);
			}
		}
		/// <summary>
		/// Выставить флаг об начличии изменений = <see cref="true"/>
		/// Используется например при инициализации или первом запуске приложения или рестарте игры
		/// </summary>
		public static void ChangesFlagOn()
		{
			isAtLeastOneChange = true;

			OnChangeAnyEvent?.Invoke();
		}

		public static void Reset()
		{
			changedItems.Clear();
			changedItemsRespectively.Clear();
		}

		public static void Clean()
		{
			Reset();

			foreach (var item in notifiersPerEvent)
			{
				item.Value.Clean();
			}
			isAtLeastOneChange = false;

			OnChangeAnyEvent.Clean();

			OnChangeConcreteEvent.Clean();
		}

		public static DataTracker<T> GetOrCreateDefault()
		{
			if (shared == null)
			{
				shared = CreateDefault();
			}
			return shared;
		}
		public static DataTracker<T> CreateDefault()
		{
			DataTracker<T> dataTracker = new DataTracker<T>();

			return dataTracker;
		}
	}


	/// <summary>
	/// <see cref="DataTrackable"/> Дополенен возможностью сохранять состояния прерыдущих циклов и управлять их временем жизни
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Obsolete]
	public class DataTrackerRecordable<T> : DataTracker<T> where T : IDataTrackable, IEquatable<T>
	{

	}
}