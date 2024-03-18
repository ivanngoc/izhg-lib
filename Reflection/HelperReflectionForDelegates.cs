using System;
using System.Reflection;
using System.Reflection.Emit;

namespace IziHardGames.Libs.NonEngine.Reflection
{
	public static class HelperReflectionForDelegates
	{
		/// <summary>
		/// C
		/// </summary>
		/// <typeparam name="TDelegate"></typeparam>
		/// <param name="methodInfo"></param>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static TDelegate CreateDelegateWithMethodInfo<TDelegate>(MethodInfo methodInfo, object instance) where TDelegate : Delegate
		{
			Type type = typeof(TDelegate);

			if (methodInfo.IsStatic)
			{
				if (methodInfo.IsGenericMethodDefinition)
				{
					var args = type.GetGenericArguments();
					var methodInfoGen =  methodInfo.MakeGenericMethod(args);
					var res = methodInfoGen.CreateDelegate(type, instance) as TDelegate;
					return res;
				}
				else
				{
					TDelegate res = methodInfo.CreateDelegate(type) as TDelegate;
					return res;
				}
			}
			else
			{
				if (methodInfo.IsGenericMethodDefinition)
				{
					var args = type.GetGenericArguments();
					var methodInfoGen =  methodInfo.MakeGenericMethod(args);
					var res = methodInfoGen.CreateDelegate(type, instance) as TDelegate;
					return res;
				}
				else
				{

					var res = methodInfo.CreateDelegate(type, instance) as TDelegate;
					return res;
				}
			}
		}

#if UNITY_EDITOR || DEBUG
		internal delegate void DelegateTest<T>(T s);
		internal delegate void DelegateTestWithParams(int p1, float p2, string p3);
		public static void Test<T>()
		{
			string trace = $"{System.Reflection.MethodInfo.GetCurrentMethod().ReflectedType}.{System.Reflection.MethodInfo.GetCurrentMethod().ToString()}";

			ClassSpecificMethodHolder specificMethodHolder =  new ClassSpecificMethodHolder();
			var action1 = CreateDelegateWithMethodInfo<DelegateTestWithParams>(typeof(ClassSpecificMethodHolder).GetMethod(nameof(ClassSpecificMethodHolder.NonGenericWithParameters)),specificMethodHolder);
			var action = CreateDelegateWithMethodInfo<DelegateTest<T>>(typeof(ClassSpecificMethodHolder).GetMethod(nameof(ClassSpecificMethodHolder.DoSomethingGeneric)),specificMethodHolder);
			var actionStaticNonGeneric = CreateDelegateWithMethodInfo<DelegateTestWithParams>(typeof(ClassSpecificMethodHolder).GetMethod(nameof(ClassSpecificMethodHolder.StaticMethod)), default);
			var actionStatic = CreateDelegateWithMethodInfo<DelegateTest<T>>(typeof(ClassSpecificMethodHolder).GetMethod(nameof(ClassSpecificMethodHolder.StaticGenericMethod)), default);

			//null: always need to create new
			Action<T> actionConvert = action as Action<T>;
		}

		internal class ClassSpecificMethodHolder
		{
			public void NonGeneric()
			{

			}
			public void NonGenericWithParameters(int p1, float p2, string p3)
			{

			}
			public void DoSomethingGeneric<T>(T item)
			{
				MethodBase methodBase = MethodInfo.GetCurrentMethod();
				MethodInfo methodInfo = methodBase as MethodInfo;
				// null
				DynamicMethod dynamicMethod = methodInfo as DynamicMethod;
			}

			public static void StaticGenericMethod<T>(T item)
			{

			}

			public static void StaticMethod(int p1, float p2, string p3)
			{

			}
		}
#endif
	}
}