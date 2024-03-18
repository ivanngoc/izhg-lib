using System;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Develop.Patterns
{
	/// <summary>
	/// Паттерны-шаблоны. Примеры или шаблоны для разработки (готовые или в процессе). Цель создать устойчивую арзитиектуру и возможно интерфейсы или базовые классы.
	/// </summary>
	internal class DevelopingPatterns
	{
		#region Unity Message


		#endregion


		/// <summary>
		/// В ситуациях когда объект может иметь динамическое количество аттрибутов. По философии напоминает компонентный подход к объектам в Unity.
		/// </summary>
		public class AdjastableDataAsAtributePattern
		{
			public struct AttributeAsKey
			{

			}

			public struct AttributeValueWrapper
			{

			}

			public interface IAttribueHolder
			{

			}
		}

		/// <summary>
		/// Бывает что объекты создаются в неопределенном или даже хаотичном порядке.
		/// Необходимо рещение которое позволит автоматически связывать объекты без штрафов к производительности.
		/// Например был создан А в котором требуется обхект Б но на момент создания А не известно был ли создан Б поэтому нельзя просто передать объект через конструктор.
		/// Один из ваирантов это применение схемы (указание не конкретного объекта а его типа как в качестве типа-параметра-дженерика так и в качестве передаующегося аргумента), которая подходит для синглтонов. 
		/// </summary>
		private class AsyncObjectCreationAndLinking
		{
			// 1 Вариант: создать объект который будет давать обещание на получение объекта. Из минуса - каждое обращение это акцессор.
			// 2 Вариант схож с первым. Обращаться через статические поля. Подходит только для синглтонов
		}

		/// <summary>
		/// Управление отображением
		/// </summary>
		private class ViewPatterns
		{
			/// <summary>
			/// 
			/// </summary>
			public class Window
			{
				public List<Form> rootForms;

				public void Open() { }
				public void Close() { }
				public void Minimize() { }
			}

			public class Form
			{
				/// <summary>
				/// Встроен ли в другую Form. Если false значит  находится в рут у window
				/// </summary>
				public bool IsEmbeded { get; set; }
				public bool IsUpdated { get; set; }
				public bool IsUpdateUiScheduled { get; set; }
				public void ScheduleUpdateUi() { }
				public void Show() { }
				public void Hide() { }

				/// <summary>
				/// To Window
				/// </summary>
				public void Evaluate()
				{ }

				/// <summary>
				/// Встроить
				/// </summary>
				public void Embed(RectTransform container)
				{

				}

				public void Detach() { RectTransform rectTransform = default; }

				private enum EFormType
				{
					Standart,
					Dynamic, // auto show on hover or click and auto hide
					Popup,
					Modal,
				}
			}

		}
		/// <summary>
		/// Изменение состояния объектов и их данных
		/// </summary>
		private class ControlPatterns
		{

		}
		/// <summary>
		/// Создание, Хранение, запись, поиск, идентификация,
		/// </summary>
		private class ManagePatterns
		{
			public class Factory
			{
				public void Create() { }
				public void Destroy() { }
				public void Utilize() { }
			}
			public class Pool
			{
				public void Rent() { }
				public void Return() { }
			}

		}
		/// <summary>
		/// Systems / Execution. Выполнение какой то задачи
		/// </summary>
		private class ExecutinPatterns
		{

		}

		private class AsyncPatterns
		{
			/// <summary>
			/// Есть сложный процесс который в конце запускает синхронную задачу. <br/>
			/// Процессы в составе сложного не знают напрямую друг о друге и запускаются в хаотичном порядке.
			/// Цель паттерная - обеспечить запуск сложного процесса при либом инициаторе подтянув при этом другие
			/// </summary>
			private class ComplexProcessWithAnyInitiator
			{
				public class ComplexProcess
				{
					private int id;
					public List<SubProcess> subProcesses;
					public Action finishAction;
					// here could be shared Data Pool

					public void ComplexStart()
					{
						foreach (var item in subProcesses)
						{
							if (!item.isTrigerred)
							{
								item.Execute();
							}
						}
					}

					public void Check()
					{
						foreach (var item in subProcesses)
						{
							if (!item.isComplete) return;
						}

						ComplexReset();

						finishAction?.Invoke();
					}

					public void ComplexReset()
					{
						foreach (var item in subProcesses)
						{
							item.Reset();
						}
					}
				}

				public class SubProcess
				{
					ComplexProcess complexProcess;

					public bool isComplete;
					public bool isTrigerred;
					public void Initiate()
					{
						complexProcess.ComplexStart(); //async

						Execute();
					}
					public void Execute()
					{
						isTrigerred = true;
						// do own action
					}
					public void Reset()
					{
						isComplete = false;

						isTrigerred = false;
					}
				}
			}
		}

		/// <summary>
		/// Адресацию можно создать только общий родительским классом между дженериками
		/// </summary>
		private class GenericHub
		{
			/// <summary>
			/// key - id of <see cref="SomeGeneric<T>"/>
			/// </summary>
			protected static Dictionary<Type, GenericHub> keyValuePairs;

			public virtual void Shared() { }

			public static void ConcreteGenericInstanceCall(Type type)
			{
				keyValuePairs[type].Shared();
			}

			private class SomeGeneric<T> : GenericHub
			{
				public int id;
				public T field;

				public SomeGeneric()
				{
					keyValuePairs.Add(typeof(T), this);
				}

				public override void Shared()
				{
					base.Shared();
				}
			}
		}

		private class ActivationModePatterns
		{
			public void Activate() { }
			public void Deactivate() { }
		}

		private class JobPatterns
		{
			public class Job
			{
				public void Start() { }
				public void Complete() { }
				public void ContinueWith() { }
				public void Cancel() { }
				public void Pause() { }
				public void Resume() { }
			}

			public class System
			{

			}
		}
	}
}
