#if UNITY_EDITOR
using IziHardGames.CustomEditor;
using IziHardGames.Extensions.PremetiveTypes;
using IziHardGames.ProjectResourceManagment;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static IziHardGames.ProjectResourceManagment.TaggedAsset;

namespace IziHardGames.GameProject1
{
	public class WindowProjectRegistry : EditorWindow
	{
		private RegistryAsset captured;
		private ReorderableList reorderableList;
		private SerializedObject serializedObject;

		private Vector2 pos;
		private bool isListOpened;

		#region Unity Message
		private void OnGUI()
		{
			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script Editor:", MonoScript.FromScriptableObject(this), typeof(WindowProjectRegistry), true);
			if (captured != null)
			{
				EditorGUILayout.ObjectField("Script RegistryAsset:", MonoScript.FromScriptableObject(captured), typeof(RegistryAsset), true);
			}
			GUI.enabled = true;

			captured = EditorGUILayout.ObjectField("Registr:", captured, typeof(RegistryAsset), true) as RegistryAsset;

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Refresh this Layout"))
			{
				Refresh();
			}
			if (GUILayout.Button("—генерировать ассет регистр"))
			{
				GenerateFiles();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			//ShowList();
			ShowListV2();
		}
		#endregion
		[MenuItem("Window/Custom/Resources/Project Resources Manager")]
		private static void Init()
		{
			// Get existing open window or if none, make a new one:
			WindowProjectRegistry window = (WindowProjectRegistry)EditorWindow.GetWindow(typeof(WindowProjectRegistry));

			window.titleContent = new GUIContent("Project Resources Manager");

			window.CreateElements();

			window.Show();
		}

		public void CreateElements()
		{
			if (captured != null)
			{
				serializedObject = new SerializedObject(captured);

				reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("objects"));
			}
		}

		public void ShowList()
		{
			pos = EditorGUILayout.BeginScrollView(pos);

			if (reorderableList != null)
			{
				reorderableList.DoLayoutList();
			}
			EditorGUILayout.EndScrollView();
		}

		public void ShowListV2()
		{
			if (captured == null) return;

			isListOpened = EditorGUILayout.InspectorTitlebar(isListOpened, captured);

			if (isListOpened)
			{
				float cache = EditorGUIUtility.labelWidth;

				EditorGUIUtility.labelWidth = 30;

				pos = EditorGUILayout.BeginScrollView(pos);

				if (captured.objects.Count != captured.ids.Count ||
					captured.objects.Count != captured.localIdentifiers.Count ||
					captured.objects.Count != captured.guids.Count)
				{
					captured.CreateIds();
				}

				for (int i = 0; i < captured.objects.Count; i++)
				{
					Object item = captured.objects[i];

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Id");
					EditorGUILayout.DelayedIntField(captured.ids[i]);
					EditorGUILayout.PrefixLabel("HexId");
					EditorGUILayout.DelayedTextField(captured.ids[i].UIntToStringHex());
					EditorGUILayout.PrefixLabel("GUID");
					EditorGUILayout.DelayedTextField(captured.guids[i]);
					EditorGUILayout.PrefixLabel("LID");
					EditorGUILayout.LongField(captured.localIdentifiers[i]);

					EditorGUILayout.ObjectField(item, item.GetType());

					EditorGUILayout.EndHorizontal();
				}

				EditorGUIUtility.labelWidth = cache;

				EditorGUILayout.EndScrollView();
			}
		}

		public void Refresh()
		{
			CreateElements();
		}


		public void GenerateFiles()
		{
			List<TaggedAsset> taggedAssets = new List<TaggedAsset>();

			int offset = 2000;

			for (int i = 0; i < captured.objects.Count; i++)
			{
				TaggedAsset taggedAsset = ScriptableObject.CreateInstance<TaggedAsset>();

				Object asset = captured.objects[i];

				taggedAsset.asset = asset;
				taggedAsset.idProjectAsset = i + offset;

				if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out string guid, out long localId))
				{
					taggedAsset.idAsset = guid;
					taggedAsset.localIdentificator = localId;
				}

				EAssetType eAssetType = taggedAsset.DefineType();

				taggedAssets.Add(taggedAsset);

				string path = Path.Combine(PathsEditor.TAG_ASSETS, PathsEditor.GetTagName(taggedAsset.idProjectAsset));

				if (!Directory.Exists(PathsEditor.TAG_ASSETS))
				{
					Directory.CreateDirectory(PathsEditor.TAG_ASSETS);
				}
				AssetDatabase.CreateAsset(taggedAsset, path);
			}
			AssetDatabase.SaveAssets();

			EditorUtility.FocusProjectWindow();

			Selection.activeObject = taggedAssets.Last();
		}
	}
}
#endif
