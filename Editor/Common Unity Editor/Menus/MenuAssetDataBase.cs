using UnityEditor;

#if UNITY_EDITOR
public class MenuAssetDataBase : Editor
{
	[MenuItem("Tools*/ADB*/RefreshScriptcsOnly")] // ctrl+shift+a
	public static void RefreshScriptcsOnly()
	{
		AssetDatabase.Refresh(ImportAssetOptions.Default);
	}

}
#endif
