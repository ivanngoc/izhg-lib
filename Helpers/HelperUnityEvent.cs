#if UNITY_EDITOR
using IziHardGames.Libs.NonEngine.Reflection;
using System;
using System.Buffers;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
#endif


namespace UnityEngine.Events
{
#if UNITY_EDITOR
	public static class HelperUnityEvent
	{
		/// <summary>
		/// Добавить если еще нет подписчика
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="unitEvent"></param>
		/// <param name="unityAction"></param>
		public static void UniqueListenerAdd(UnityEvent unitEvent, UnityAction unityAction, Object unityActionSource)
		{

			int count = unitEvent.GetPersistentEventCount();

			for (int i = 0; i < count; i++)
			{
				Object target = unitEvent.GetPersistentTarget(i);

				if (target == unityActionSource && unityAction.Method.Name == unitEvent.GetPersistentMethodName(i))
				{
					UnityEventTools.RegisterPersistentListener(unitEvent, i, unityAction);

					return;
				};
			}
			UnityEventTools.AddPersistentListener(unitEvent, unityAction);
		}
		public static void UniqueListenerAdd<T>(UnityEvent<T> arg, UnityAction<T> unityAction, Object sourceOfMethod)
		{
#if UNITY_EDITOR

			int count = arg.GetPersistentEventCount();

			for (int i = 0; i < count; i++)
			{
				Object target = arg.GetPersistentTarget(i);

				//if (target == targetArg && methodInfo != null)
				if (target == sourceOfMethod && unityAction.Method.Name == arg.GetPersistentMethodName(i))
				{
					UnityEventTools.RegisterPersistentListener(arg, i, unityAction);

					return;
				};
			}

			UnityEventTools.AddPersistentListener(arg, unityAction);
#endif
		}

		public static void CheckForMissingMethodRefereces<T>(UnityEvent<T> unityEvent, Object eventSource)
		{
#if UNITY_EDITOR
			int count = unityEvent.GetPersistentEventCount();

			for (int i = 0; i < count; i++)
			{
				Object target = unityEvent.GetPersistentTarget(i);
				string methodName = unityEvent.GetPersistentMethodName(i);
				MethodInfo methodInfo = UnityEventBase.GetValidMethodInfo(target, methodName, new Type[] { typeof(T) });

				if (methodInfo == null)
				{
					Debug.LogError($"Reference to method is missing on event. Method Name [{methodName}] with arg type [{typeof(T).Name}]", eventSource);
				}
			}
#endif
		}
		public static void CheckForMissingMethodRefereces(UnityEvent unityEvent, Object eventSource)
		{
			int count = unityEvent.GetPersistentEventCount();

			for (int i = 0; i < count; i++)
			{
				Object target = unityEvent.GetPersistentTarget(i);
				string methodName = unityEvent.GetPersistentMethodName(i);
				MethodInfo methodInfo = UnityEventBase.GetValidMethodInfo(target, methodName, Array.Empty<Type>());

				if (methodInfo == null)
				{
					Debug.LogError($"Reference to method is missing on event. Method Name {methodName}", eventSource);
				}
			}
		}

		public static void DeleteMissingeReferences(UnityEvent unitEvent, Object eventSource)
		{
#if UNITY_EDITOR
			int count = unitEvent.GetPersistentEventCount();

			for (int i = 0; i < count; i++)
			{
				Object target = unitEvent.GetPersistentTarget(i);
				string methodName = unitEvent.GetPersistentMethodName(i);
				MethodInfo methodInfo = UnityEventBase.GetValidMethodInfo(target, methodName, Array.Empty<Type>());

				if (methodInfo == null)
				{
					UnityEventTools.RemovePersistentListener(unitEvent, i);
					i--;
				}
			}
#endif
		}
		public static void DeleteMissingeReferences<T>(UnityEvent<T> unitEvent, Object eventSource)
		{
#if UNITY_EDITOR
			int count = unitEvent.GetPersistentEventCount();

			for (int i = 0; i < count; i++)
			{
				Object target = unitEvent.GetPersistentTarget(i);
				string methodName = unitEvent.GetPersistentMethodName(i);
				MethodInfo methodInfo = UnityEventBase.GetValidMethodInfo(target, methodName, new Type[] { typeof(T) });

				if (methodInfo == null)
				{
					UnityEventTools.RemovePersistentListener(unitEvent, i);
					i--;
				}
			}
#endif
		}

		private static bool EnsureSingleReferenceBase(UnityEvent unityEvent, Delegate del, Object eventSource, out int indexToOverride, out Type[] methodParametersTypes)
		{
			RemoveDuplicates(unityEvent);

			int count = unityEvent.GetPersistentEventCount();

			for (int i = 0; i < count; i++)
			{
				Object target = unityEvent.GetPersistentTarget(i);
				if (target == null)
				{
					Debug.LogError($"Pos#{i} Missing Persistent Target", eventSource);
				}
			}
			methodParametersTypes = del.Method.GetParameters().Select(x => x.ParameterType).ToArray();

			for (int i = 0; i < count; i++)
			{
				Object target = unityEvent.GetPersistentTarget(i);

				if (target != null)
				{
					string methodNameExisted = unityEvent.GetPersistentMethodName(i);
					string methodNameToEnsure = del.Method.Name;

					if (methodNameExisted == methodNameToEnsure)
					{
						MethodInfo methodInfo = UnityEventBase.GetValidMethodInfo(target, methodNameExisted, methodParametersTypes);

						if (methodInfo != null)
						{
							indexToOverride = i;
							return true;
						}
					}
				}
			}
			indexToOverride = -1;
			return false;
		}

		private static void RemoveDuplicates(UnityEvent unityEvent)
		{
START:
			int count = unityEvent.GetPersistentEventCount();

			for (int i = 0; i < count; i++)
			{
				Object targetI = unityEvent.GetPersistentTarget(i);
				string nameI = unityEvent.GetPersistentMethodName(i);
				MethodInfo methodInfoI = UnityEventBase.GetValidMethodInfo(targetI, nameI, Array.Empty<Type>());

				for (int j = 0; j < count; j++)
				{
					if (i == j) continue;

					Object targetJ = unityEvent.GetPersistentTarget(j);
					string nameJ = unityEvent.GetPersistentMethodName(j);
					MethodInfo methodInfoJ = UnityEventBase.GetValidMethodInfo(targetJ, nameJ, Array.Empty<Type>());

					if (targetI == targetJ)
					{
						if (methodInfoI == methodInfoJ)
						{
							UnityEventTools.RemovePersistentListener(unityEvent, j);
							goto START;
						}
					}
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="unityEvent"></param>
		/// <param name="action"></param>
		public static void EnsureReference(UnityEvent unityEvent, Delegate del, Object eventSource)
		{
			if (EnsureSingleReferenceBase(unityEvent, del, eventSource, out int indexToOverride, out Type[] methodParametersTypes))
			{

			}
			int count = unityEvent.GetPersistentEventCount();

			if (methodParametersTypes.Length == 0)
			{
				UnityEventTools.RegisterPersistentListener(unityEvent, count, (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), del.Method));
			}
			else
			{
				throw new ArgumentOutOfRangeException("There is must be delegate without arguments");
			}
		}

		public static void EnsureSingleReference<T>(UnityEvent unityEvent, Delegate del, T argument, Object eventSource) where T : Object
		{
			if (EnsureSingleReferenceBase(unityEvent, del, eventSource, out int indexToOverride, out Type[] methodParametersTypes))
			{
				UnityAction<T> unityAction = HelperReflectionForDelegates.CreateDelegateWithMethodInfo<UnityAction<T>>(del.Method, del.Target);
				UnityEventTools.RegisterObjectPersistentListener(unityEvent, indexToOverride, unityAction, argument);
			}
			else
			{
				int count = unityEvent.GetPersistentEventCount();

				if (methodParametersTypes.Length == 1)
				{
					if ((typeof(Object).IsAssignableFrom(methodParametersTypes[0])))
					{
						Type typeAction = typeof(UnityAction<T>);
						MethodInfo methodInfo = typeAction.GetMethod("Invoke");
						UnityAction<T> unityAction = HelperReflectionForDelegates.CreateDelegateWithMethodInfo<UnityAction<T>>(del.Method, del.Target);
						UnityEventTools.AddObjectPersistentListener(unityEvent, unityAction, argument);
						return;
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}
		public static void EnsureSingleReferences(UnityEvent unityEvent, Action[] actions)
		{
			throw new NotImplementedException();
		}
		public static void EnsureReferences<T>(UnityEvent<T> unityEvent, Action<T>[] actions)
		{
			throw new NotImplementedException();
		}
	}
#endif
}