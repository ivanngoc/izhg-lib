using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IziHardGames.Libs.NonEngine.Reflection
{
	public static class HelperReflection
	{
		public static T[] GetTypesWithAttribute<T, TAttribute>(TAttribute type, Assembly assembly)
			where T : Type
			where TAttribute : Attribute
		{
			var types = assembly.GetTypes()
								.Where(x => (x as MemberInfo).GetCustomAttribute(typeof(TAttribute)) != null)
								.Where(z => z is T).Select(t => t as T).ToArray();

			return types;
		}

		public static TAttribute[] GetAttributeInstances<TAttribute, TType>(Assembly assembly)
			where TAttribute : Attribute
			where TType : class
		{
			var types = assembly.GetTypes().ToArray();

			var typesFounded = types.Where(x => typeof(TType).IsAssignableFrom(x)).ToArray();

			var attributes = typesFounded.Select(t => t.GetCustomAttribute<TAttribute>()).Where(x => x != null).ToArray();

			return attributes;
		}

		public static Dictionary<Type, TAtttribute> GetTypesWithAttributes<TType, TAtttribute>(Assembly assembly)
			where TAtttribute : Attribute
		{
			var types = assembly.GetTypes();

			var result = new Dictionary<Type, TAtttribute>();

			for (var i = 0; i < types.Length; i++)
			{
				if (typeof(TType).IsAssignableFrom(types[i]))
				{
					var atttribute = types[i].GetCustomAttribute<TAtttribute>();

					if (atttribute != null)
					{
						result.Add(types[i], atttribute);
					}
				}
			}

			return result;
		}

		public static void CopyFieldsShallow<T>(T from, T to) where T : class
		{
			var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			for (var i = 0; i < fields.Length; i++)
			{
				fields[i].SetValue(to, fields[i].GetValue(from));
			}
		}
	}
}