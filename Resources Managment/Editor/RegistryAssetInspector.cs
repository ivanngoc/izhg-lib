#if UNITY_EDITOR
using IziHardGames.Extensions.PremetiveTypes;
using UnityEditor;
using UnityEngine;

namespace IziHardGames.ProjectResourceManagment.Editor
{
	[UnityEditor.CustomEditor(typeof(RegistryAsset))]
	public class RegistryAssetInspector : UnityEditor.Editor
	{
		Object toInsert;
		bool isToInsertChecked = false;

		string guidToSearch;
		bool isUpdatedGuidField;

		int guidInProjectToSearch;
		bool isUpdatedByGuidInt;

		string guidInPorjectHex;
		bool isUpdateByGuidHex;

		bool isContainsObject;
		bool isGotAssetGuid;
		bool isContainsGuid;


		public int idInProject;
		public string description;
		bool isUpdatedDescription;

		#region Unity Message

		public override void OnInspectorGUI()
		{
			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script Editor:", MonoScript.FromScriptableObject(this), typeof(RegistryAssetInspector), true);
			GUI.enabled = true;

			DrawDefaultInspector();

			RegistryAsset exemplar = target as RegistryAsset;

			EditorGUILayout.Space();

			GUILayout.Label("Служебные функции");

			#region Select Object
			Rect rect = EditorGUILayout.BeginHorizontal();

			var temp = EditorGUILayout.ObjectField(toInsert, typeof(Object), true);

			if (temp != toInsert)
			{
				toInsert = temp;

				isToInsertChecked = false;

				UpdateIds(exemplar.GetIds(toInsert));
			}

			if (!isToInsertChecked && toInsert != null)
			{
				var flags = exemplar.IsContains(toInsert, out idInProject);

				isContainsObject = flags.Item1;
				isGotAssetGuid = flags.Item2;
				isContainsGuid = flags.Item3;

				isToInsertChecked = true;
			}


			GUI.enabled = toInsert != null;

			if (GUILayout.Button("Вставить в регистр"))
			{
				if (exemplar.Insert(toInsert, description, out int id))
				{
					isToInsertChecked = false;

					UpdateIds(exemplar.GetIds(toInsert));
				}
			}

			GUI.enabled = true;

			EditorGUILayout.EndHorizontal();
			#endregion


			GUILayout.Label($"{isContainsObject}|{isGotAssetGuid}|{isContainsGuid}");

			if (toInsert != null)
			{
				GUI.enabled = false;

				Rect rect2 = EditorGUILayout.BeginHorizontal();

				EditorGUILayout.IntField(idInProject);

				EditorGUILayout.TextField(idInProject.UIntToStringHex());

				EditorGUILayout.EndHorizontal();

				GUI.enabled = true;
			}

			EditorGUILayout.Space();

			FindWithFields(exemplar);

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Сбросить свободный Id"))
			{
				exemplar.freeGuid = int.MinValue;

				EditorUtility.SetDirty(exemplar);
			}
			if (GUILayout.Button("Distinct"))
			{
				exemplar.Destinct();

				EditorUtility.SetDirty(exemplar);
			}
			if (GUILayout.Button("Regain"))
			{
				exemplar.RegainIdsFromFile();

				EditorUtility.SetDirty(exemplar);
			}
			EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Override File"))
			{
				exemplar.OverrideFile();

				EditorUtility.SetDirty(exemplar);
			}

		}

		public void FindWithFields(RegistryAsset exemplar)
		{
			#region Asset GUID
			Rect rect3 = EditorGUILayout.BeginHorizontal();

			var tempGuid = EditorGUILayout.TextField(guidToSearch);

			if (guidToSearch != tempGuid)
			{
				guidToSearch = tempGuid;

				isUpdatedGuidField = false;
			}

			if (!isUpdatedGuidField)
			{
				string path = AssetDatabase.GUIDToAssetPath(guidToSearch);

				if (string.IsNullOrEmpty(path))
				{
					toInsert = null;
				}
				else
				{
					toInsert = AssetDatabase.LoadAssetAtPath(path, typeof(Object));

					UpdateIds(exemplar.GetIds(toInsert));
				}

				isUpdatedGuidField = true;
			}
			#endregion

			EditorGUILayout.EndHorizontal();

			Rect rect4 = EditorGUILayout.BeginHorizontal();

			float cacheWidth = EditorGUIUtility.labelWidth;

			EditorGUIUtility.labelWidth = 50f;

			#region Int FIELD GUID
			EditorGUILayout.PrefixLabel("int field");

			var guidInProjectToSearchTemp = EditorGUILayout.IntField(guidInProjectToSearch);

			if (guidInProjectToSearch != guidInProjectToSearchTemp)
			{
				guidInProjectToSearch = guidInProjectToSearchTemp;

				isUpdatedByGuidInt = false;
			}

			if (!isUpdatedByGuidInt)
			{
				toInsert = exemplar.GetObject(guidInProjectToSearch);

				UpdateIds(exemplar.GetIds(toInsert));

				isUpdatedByGuidInt = true;
			}

			#endregion

			#region HEX FIELD
			EditorGUILayout.PrefixLabel("Hex Field");

			var guidInPorjectHexTemp = EditorGUILayout.TextField(guidInPorjectHex);

			if (guidInPorjectHex != guidInPorjectHexTemp)
			{
				isUpdateByGuidHex = false;
			}

			if (!isUpdateByGuidHex)
			{
				toInsert = exemplar.GetObjectByHex(guidInPorjectHex);

				UpdateIds(exemplar.GetIds(toInsert));

				isUpdateByGuidHex = true;
			}
			#endregion

			EditorGUIUtility.labelWidth = cacheWidth;

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.PrefixLabel("Description");

			var descriptionTemp = EditorGUILayout.DelayedTextField(description);

			if (description != descriptionTemp)
			{
				description = descriptionTemp;

				exemplar.UpdateDescription(guidInProjectToSearch, description);

				EditorUtility.SetDirty(exemplar);

				isUpdatedDescription = false;
			}

			if (!isUpdatedDescription)
			{
				// do something
				isUpdatedDescription = true;
			}
		}


		public void UpdateIds((int, string, string, string) ids)
		{
			guidToSearch = ids.Item2;

			guidInProjectToSearch = ids.Item1;

			guidInPorjectHex = ids.Item3;

			description = ids.Item4;
		}
		#endregion
	}
}
#endif
