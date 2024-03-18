using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using IziHardGames.UserControl.Abstractions.NetStd21.Attributes;

namespace IziHardGames.UserControl.Abstractions.NetStd21
{
	public interface IInputSystem
	{

	}

	public interface ISetOfInputStores
	{

	}

	/// <summary>
	/// Единица с которого собираются данные
	/// </summary>
	public interface IInputStore
	{

	}

	public interface IInputItemData
	{

	}
	/// <summary>
	/// централизованный агреггированный набор данных. Здесь же хранятся производные значения
	/// </summary>
	public interface IInputDataSet
	{

	}

	public interface IInputCollector
	{
		void CollectAtNormalUpdate();
		void CollectAtFixedUpdate();
		void ResetForNextLoop();
	}

	public static class CombosData
	{

	}

	public static class ButtonsData
	{
		public static readonly Dictionary<int, bool> datas = new Dictionary<int, bool>();
	}
	[Registry]
	public static class RegistryForDevices
	{
		public readonly static List<Device> devices = new List<Device>();
		public static DeviceFinder Find { get; set; }

		public static Device Regist(object target)
		{
			var d = new Device();
			d.target = target;
			devices.Add(d);
			return d;
		}

		public static Device RegistOrUpdateDevice(object target)
		{
			return Device.Any;
		}
	}
	public abstract class DeviceFinder
	{
		public abstract Device ByTarget(object target);
	}

	public abstract class IziInputSystem : IInputSystem
	{
		protected IInputCollector? iCollector;
		protected IInputDataSet? iInputDataSet;
		protected ISetOfInputStores? setOfInputItem;
		public abstract T GetCollectorAs<T>() where T : class, IInputCollector;
	}

	public sealed class InputContainerData : IInputItemData
	{
		internal int streakTicksActive;
		internal int streakTicksInctive;

		internal float timeNormalActive;
		internal float timeNormalInactive;
		internal float timeFixedActive;
		internal float timeFixedInActive;

		internal bool isTransitionOnToOff;
		internal bool isTransitionOffToOn;
		/// <summary>
		/// Значение предыдущего кадра не совпадает со значением текущего кадра
		/// </summary>
		internal bool isChanged;
		internal bool isActive;
		/// <summary>
		/// Верхний фронт/ From Off To On
		/// </summary>
		internal bool isRaised;
        /// <summary>
        /// Нижний фронт/ From On To Off
        /// </summary>
        internal bool isRaisedReverse;

		public bool IsChanged => isChanged;

		public void SetActive(bool value)
		{
			this.isActive = value;
		}
		public void SetChanged(bool value)
		{
			isChanged = value;
		}
	}

	/// <summary>
	/// В случае с Unity можно также разделять устройство с которого собирается информация. Поэтому можно создать наборы активаций (<see cref="ISetOfTriggerSources"/>) отдельно для каждого устройства.
	/// Можно играть подключив 2 клавиатуры раздельно! Но это будет зависеть от реализации <see cref="IInputCollector"/> который должен разделять устройства.
	/// Здесь же обозначим, что например может быть 2 <see cref="InputContainer"/> для клавиши [W] от разных устройств
	/// </summary>
	public abstract class InputContainer : IInputStore
	{
		public int code;
		public readonly Device device;
		public readonly InputContainerData data = new InputContainerData();

		public InputContainer(int code, Device device)
		{
			this.code = code;
			this.device = device;
		}

		public virtual void UpdateValueInt(int value)
		{

		}
		public virtual void UpdateValueFloat(float value)
		{

		}
		public void SetChanged(bool value)
		{
			data.SetChanged(value);
		}
	}

	public sealed class SetOfInputStores : ISetOfInputStores
	{
		public InputContainer[] items;
	}

	/// <summary>
	/// Slot Of Device. Example: Device 1 is always for player 1; Device 2 is Always for player 2.
	/// </summary>
	public class Device
	{
		public readonly static Device Any = new Device();
		public readonly static Device Default = new Device();
		public readonly static Device NotFound = new Device();

		public int id;
		public string idString;
		public string serial;
		public object target;

		public void SetId(string idString)
		{
			this.idString = idString;
		}

		public bool Compare(Device device)
		{
			return this.idString == device.idString;
		}
	}

	public abstract class InputIContainerFactory
	{
		public abstract InputContainer GetNew(int id, int value,Device device);
	}

	/// <summary>
	/// Задача на сбор инпута для всех устройств. Нужно для того чтобы сначала собрать все данные со всех устройств а потом по необзодимости сравнивать.
	/// Для одного типа устройств может быть много связок тип_контейнера-устройство. Это значит что для одого устройства может быть несколько контейнеров различного типа. 
	/// Например: для клавиши Space будет два контейнера: один UnityContainer а другой CommonLibContainer
	/// </summary>
	public sealed class InputCollectTask
	{
		public int idTask;
		public int valueAsInt;
		public Type deviceType;
		private readonly List<PairContainerAndDevice> pairs = new List<PairContainerAndDevice>();
		public IEnumerable<PairContainerAndDevice> Pairs => pairs;

		public InputCollectTask(Type type, int idTask)
		{
			this.idTask = idTask;
			this.deviceType = type;
		}

		public void SetValue(int value)
		{
			valueAsInt = value;
		}

		public bool TryFindPair<T>(Device device, out PairContainerAndDevice pair)
		{
			pair = pairs.FirstOrDefault(x => x.device == device && x is T);
			return pair != null;
		}
		public bool TryFindContainer<T>(Device device, out InputContainer container)
		{
			container = pairs.FirstOrDefault(x => x.device == device && x is T)?.container;
			return container != null;
		}
		public bool TryFindContainer(Device device, out InputContainer container)
		{
			container = pairs.FirstOrDefault(x => x.device == device)?.container;
			return container != null;
		}
		public T FindContainer<T>(Device device) where T : InputContainer
		{
			return pairs.First(x => x.device == device).container as T;
		}
		public PairContainerAndDevice CreatePair()
		{
			var pair = new PairContainerAndDevice();
			pairs.Add(pair);
			return pair;
		}
		public void CreatePair(InputContainer container, Device device)
		{
#if true
			if (pairs.Any(x => x.device == device && x.container == container)) throw new ArgumentException("Both arguments already existed as pair");
			if (pairs.Any(x => x.device == device)) throw new ArgumentException("Same Device is Already Existed");
			if (pairs.Any(x => x.container == container)) throw new ArgumentException("Same Container is Already Existed");
#endif
			PairContainerAndDevice pairContainer = CreatePair();
			pairContainer.Bind(device, container);
		}

		internal bool IsAnyPressed()
		{
			throw new NotImplementedException();
		}
	}

	public class PairContainerAndDevice
	{
		public Device device;
		public InputContainer container;
		public readonly int id;
		public PairContainerAndDevice()
		{
			id = GetHashCode();
		}

		internal void Bind(Device device, InputContainer container)
		{
			this.device = device;
			this.container = container;
		}
	}
}
