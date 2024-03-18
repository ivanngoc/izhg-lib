namespace IziHardGames.CustomEditor
{
	public class PathsEditor
	{
		public const string ROOT_ASSETS = "Assets";
		public const string COMMON_PROJECT_ASSETS_MANAGMENT = "Assets/Editor/Project Resources Managment";
		public const string TAG_ASSETS = "Assets/Editor/Project Resources Managment/TAGS";
		public const string MAP_ASSET = "Assets/[Project] GameProject1/Scriptable Objects/Level Map";
		public const string GAME_ITEMS_PRESETS = "Assets/[Project] GameProject1/Scriptable Objects/Game Items Presets";

		public static string GetTagName(int id)
		{
			return $"TAG_{id}.asset";
		}

		public static string GetMapNodeName(int index)
		{
			return $"MAP_NODE_{index}.asset";
		}
		public static string GetMapEdgeName(int index)
		{
			return $"MAP_EDGE_{index}.asset";
		}
	}
}