using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Events
{
	#region API EXAMPLES
#if UNITY_EDITOR
	/// <summary>
	/// минус в том что 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class EVENTAPI<T>
	{
		private event Action<T> eventHolder;

		private readonly List<Action<T>> delegates = new List<Action<T>>();

		public event Action<T> EventProp
		{
			add
			{
				eventHolder += value;
				delegates.Add(value);
			}
			remove
			{
				eventHolder -= value;
				delegates.Remove(value);
			}
		}
		public void Clean()
		{
			foreach (var item in delegates)
			{
				eventHolder -= (Action<T>)item;
			}
			delegates.Clear();
		}
	}
#endif

	#endregion
	public class EventPublisher<T>
	{
		private event Action<T> eventHolder;

		public void Invoke(T item)
		{
			eventHolder?.Invoke(item);
		}

		public void AddListener(Action<T> action)
		{
			eventHolder += action;
		}
		public void RemoveListener(Action<T> action)
		{
			eventHolder -= action;
		}

		public void Clean()
		{
			var list = eventHolder.GetInvocationList();

			foreach (var item in list)
			{
				eventHolder -= (Action<T>)item;
			}
		}

		public static implicit operator Delegate(EventPublisher<T> d) => d.eventHolder;

		public static implicit operator Action<T>(EventPublisher<T> d) => d.eventHolder;

		public static EventPublisher<T> operator +(EventPublisher<T> a, Action<T> action)
		{
			a.eventHolder += action;

			return a;
		}

		public ref Action<T> GetDelegate()
		{
			return ref eventHolder;
		}
	}

	public class EventPublisher
	{
		private event Action eventHolder;

		public void Invoke()
		{
			eventHolder?.Invoke();
		}

		public void AddListener(Action action)
		{
			eventHolder += action;
		}
		public void RemoveListener(Action action)
		{
			eventHolder -= action;
		}
		public void Clean()
		{
			var list = eventHolder.GetInvocationList();

			foreach (var item in list)
			{
				eventHolder -= (Action)item;
			}
		}

#if UNITY_EDITOR
		private class ProgramTest
		{
			static EventPublisher eventPublisher1;

			static void MainTest(string[] args)
			{
				DoCall();

				eventPublisher1?.Invoke();

				//NumberFormatInfo
				Console.ReadLine();
			}

			public static void DoCall()
			{
				ref Action action = ref ReturnAction();

				Console.WriteLine(action.GetType());

				action?.Invoke();

				action += SomeMEthod2;

				action?.Invoke();

				Console.WriteLine($"Target ={action.Target}");
			}

			public static ref Action ReturnAction()
			{
				eventPublisher1 = new EventPublisher();

				eventPublisher1.AddListener(SomeMEthod);

				return ref eventPublisher1.eventHolder;
			}

			public static void SomeMEthod()
			{
				Console.WriteLine($"asdjaskjdSomeMEthod");
			}
			public static void SomeMEthod2()
			{
				Console.WriteLine($"2222222");
			}

			public static void SomeMEthod3()
			{
				Console.WriteLine($"3333");
			}

			public class HexFormat : IFormatProvider, ICustomFormatter
			{
				public object GetFormat(Type formatType)
				{
					if (formatType == typeof(ICustomFormatter))
						return this;
					else
						return null;
				}

				public string Format(string format, object arg, IFormatProvider formatProvider)
				{
					throw new NotImplementedException();
				}
			}
		}
#endif
		public static implicit operator Delegate(EventPublisher d) => d.eventHolder;

		public static implicit operator Action(EventPublisher d) => d.eventHolder;

		public static EventPublisher operator +(EventPublisher a, Action action)
		{
			a.eventHolder += action;

			return a;
		}

		public ref Action GetDelegate()
		{
			return ref eventHolder;
		}
	}
}