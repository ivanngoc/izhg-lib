using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace System
{
	public static class ExtensionObject
	{
		#region Unity Message


		#endregion
		private static BinaryFormatter formatter = new BinaryFormatter();

		public static T DeepCopy<T>(this T target) where T : class
		{
			using (var ms = new MemoryStream())
			{
				formatter.Serialize(ms, target);
				ms.Position = 0;
				return formatter.Deserialize(ms) as T;
			}
		}
	}

	public static class ExtensionAction
	{
		public static void Clean<T>(this Action<T> action)
		{
			var delegates = action.GetInvocationList();

			foreach (var item in delegates)
			{
				action -= (Action<T>)item;
			}
		}
		public static void Clean(this Action action)
		{
			var delegates = action.GetInvocationList();

			foreach (var item in delegates)
			{
				action -= (Action)item;
			}
		}
	}
}