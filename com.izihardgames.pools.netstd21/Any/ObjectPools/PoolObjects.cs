using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IziHardGames.Libs.Engine.Memory
{
	public class PoolObjects<T> where T : class, new()
	{
		public static PoolObjects<T> Shared
		{
			get
			{
				if (shared == null)
				{
					return CreateDefaultPool();
				}
				return shared;
			}
			set => shared = value;
		}

		private static PoolObjects<T> shared;

		private EPoolExtensionState currentState;
		private int callTick;

		public const int STANDART_CAPACITY = 128;

		public const int STANDART_FREQ_BACKGROUND = 1;
		public const int STANDART_COUNT_PER_BACKGROUND = 1;

		public const int STANDART_FREQ_LAZY = 1000;
		public const int STANDART_COUNT_PER_LAZY = 100;

		/// <summary>
		/// Величина на которую увкличиться пул при исчерпании списка свободых объектов
		/// </summary>
		protected int poolExtansion = 50;
		/// <summary>
		/// Максимальное количество объектов в пуле свободных объектов
		/// </summary>
		protected int freeLimit;

		protected List<T> objectOn;
		protected List<T> objectOff;

		protected virtual Func<T> FuncCreate { get; set; }
		protected virtual Action<T> ActionDestroy { get; set; }

		/// <summary>
		/// Сколько осталось создать отложенно объектов
		/// </summary>
		private int leftToExtandLazy;
		/// <summary>
		/// Сколько создать объектов при отложенной загрузке за вызов
		/// </summary>
		private int extandPerCallLazy;
		/// <summary>
		/// Частота с которой вызов отложного расширения будет срабатывать. <see cref="callTick"/> % <see cref="lazyCallInterval"/>
		/// </summary>
		private int lazyCallInterval;

		private int leftToExtandBackground;
		private int extandPerCallBackGround;
		private int backgroundCallInterval;

		private int leftToExtandPreAlloc;

		/// <summary>
		/// Автоматически расширяться если после взятия не оставлось объектов на величину = <see cref="poolExtansion"/>
		/// </summary>

		protected bool isAutoExtandOnPostExceed;
		/// <summary>
		/// Автоматически расширяться если перед взятием не оставлось объектов на величину = <see cref="poolExtansion"/>
		/// </summary>
		protected bool isAutoExtandOnPreExceed;
		/// <summary>
		/// Пул не может превышать значения <see cref="freeLimit"/>
		/// </summary>
		protected bool isFreeLimited;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="capacity"> <see cref="List{T}.Capacity"/></param>
		/// <param name="createItemMethod"></param>
		/// <param name="destroy"></param>
		public PoolObjects(int capacity, Func<T> createItemMethod, Action<T> destroy)
		{
			Initilize(capacity, createItemMethod, destroy);
		}

		public void Initilize(int capacity, Func<T> createItemMethod, Action<T> destroy)
		{
			FuncCreate = createItemMethod;
			currentState = EPoolExtensionState.None;

			objectOn = new List<T>(capacity);
			objectOff = new List<T>(capacity);
		}

		public virtual T Rent()
		{
			T result = default;

			if (objectOff.Count < 1)
			{
				if (isAutoExtandOnPreExceed)
				{
					Extend(poolExtansion);
				}
				else
				{
					Extend(1);
				}
			}
			result = objectOff[0];

			objectOn.Add(result);

			objectOff.Remove(result);

			if (objectOff.Count < 1 && isAutoExtandOnPostExceed)
			{
				Extend(poolExtansion);
			}

			return result;
		}
		/// <summary>
		/// добавить в пул объект который изначально не был создан в пуле
		/// </summary>
		/// <param name="value"></param>
		public virtual void ExtandWithNew(T value)
		{
			throw new NotImplementedException();
		}
		public void ReturnReusable<TClean>(TClean value) where TClean : class, T, IPoolable
		{
			value.CleanToReuse();
			Return(value);
		}

		public virtual void Return(T value)
		{
			objectOn.Remove(value);
			objectOff.Add(value);

			var count = objectOff.Count;

			if (isFreeLimited && count > freeLimit)
			{
				Shrink(count - freeLimit);
			}
		}

		public void Extend(int count)
		{
			for (var i = 0; i < count; i++)
			{
				var obj = (FuncCreate == null) ? new T() : FuncCreate();

				objectOff.Add(obj);
			}
		}

		private void Shrink(int count)
		{
			for (var i = 0; i < count; i++)
			{
				var obj = objectOff[i];

				objectOff.RemoveAt(i);

				i--;

				if (ActionDestroy != null)
				{
					ActionDestroy(obj);
				}
			}
		}

		public void Initilize(int preallocCount, int backGroundCount, int lazyCount)
		{
			leftToExtandPreAlloc = preallocCount;
			leftToExtandBackground = backGroundCount;
			leftToExtandLazy = lazyCount;
		}

		public void Configure(int perBckgrCallCount,
							  int perLazyCallCount,
							  int freqBackground,
							  int freqLazy)
		{
			extandPerCallBackGround = perBckgrCallCount;
			extandPerCallLazy = perLazyCallCount;

			backgroundCallInterval = freqBackground;
			lazyCallInterval = freqLazy;
		}

		public void ConfigureStandrat()
		{
			extandPerCallBackGround = STANDART_COUNT_PER_BACKGROUND;
			extandPerCallLazy = STANDART_COUNT_PER_LAZY;

			backgroundCallInterval = STANDART_FREQ_BACKGROUND;
			lazyCallInterval = STANDART_FREQ_LAZY;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>Все задания завершены. Нет активынх заданий </returns>
		public bool DoTask()
		{
			var isComplete = currentState == EPoolExtensionState.None;

			if (!isComplete)
			{
				callTick++;

				switch (currentState)
				{
					case EPoolExtensionState.Pause: break;
					case EPoolExtensionState.None: break;
					case EPoolExtensionState.PreAlloc: isComplete = PreAllocate(); break;
					case EPoolExtensionState.Background:
						{
							if (callTick % backgroundCallInterval > 0) return false;

							isComplete = ExtandBackground(); break;
						}
					case EPoolExtensionState.Lazy:
						{
							if (callTick % lazyCallInterval > 0) return false;

							isComplete = ExtandLazy(); break;
						}
					case EPoolExtensionState.End: break;
					default: break;
				}

				if (currentState >= EPoolExtensionState.End)
				{
					currentState = EPoolExtensionState.None;
				}
				return currentState == EPoolExtensionState.None;
			}
			return isComplete;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>Завершен ли процесс за этот вызов</returns>
		private bool ExtandBackground()
		{
			var isComplete = false;

			if (leftToExtandBackground > 0)
			{
				if (currentState != EPoolExtensionState.Background) return isComplete;

				var count = Math.Clamp(leftToExtandBackground - extandPerCallBackGround, 1, leftToExtandBackground);

				leftToExtandBackground -= count;

				Extend(count);

				isComplete = leftToExtandBackground <= 0;

				if (isComplete)
				{
					currentState++;
				}
			}

			return isComplete;
		}
		/// <summary>
		/// Постепенное расширение пула
		/// </summary>
		/// <returns>Закончен ли процесс за этот вызов</returns>
		private bool ExtandLazy()
		{
			var isComplete = false;

			if (leftToExtandLazy > 0)
			{
				if (currentState != EPoolExtensionState.Lazy) return false;

				var count = Math.Clamp(leftToExtandLazy - extandPerCallLazy, 1, leftToExtandLazy);

				leftToExtandLazy -= count;

				Extend(count);

				isComplete = leftToExtandLazy <= 0;

				if (isComplete)
				{
					currentState++;
				}
			}
			return isComplete;
		}

		/// <summary>
		/// Наолпнить пул заранее, в момент когда рендеринга нет
		/// </summary>
		private bool PreAllocate()
		{
			var isCompleted = currentState != EPoolExtensionState.PreAlloc;

			if (!isCompleted && leftToExtandPreAlloc > 0)
			{
				Extend(leftToExtandPreAlloc);

				leftToExtandPreAlloc = default;
			}

			currentState++;

			return true;
		}

		private void AllocateInBackground()
		{
			throw new Exception();
		}

		public virtual void Clean()
		{
			objectOff.AddRange(objectOn);

			objectOn.Clear();
		}

		public virtual void Unload()
		{
			for (var i = 0; i < objectOn.Count; i++)
			{
				if (ActionDestroy != null)
				{
					ActionDestroy(objectOn[i]);
				}
			}

			for (var i = 0; i < objectOff.Count; i++)
			{
				if (ActionDestroy != null)
				{
					ActionDestroy(objectOff[i]);
				}
			}
			objectOn.Clear();

			objectOff.Clear();
		}

		private enum EPoolExtensionState
		{
			Pause,
			None,
			/// <summary>
			/// Самый лучший момент для расширения - момент когда рендерная нагрузка минмальна
			/// </summary>
			PreAlloc,
			/// <summary>
			/// Фоновый режим создания. Создаает мало, но на большом промежутке времени
			/// </summary>
			Background,
			/// <summary>
			/// Отложенный вызов
			/// </summary>
			Lazy,

			End,
		}

		public static PoolObjects<T> CreateDefaultPool()
		{
			PoolObjects<T> poolObjects = new PoolObjects<T>(0, null, null);

			shared = poolObjects;

			return poolObjects;
		}

#if !UNITY_WEBGL
		/// <summary>
		/// Создать объекты через другой поток (если задача не синхронизована)
		/// </summary>
		/// <returns></returns>
		public Task AllocateInSeparateThread(int count)
		{
			Extend(count);

			return Task.CompletedTask;
		}
#endif
	}
}