#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
// using Image = UnityEngine.UI.Image;

namespace IziHardGames.ProjectResourceManagment
{
	[UnityEditor.CustomEditor(typeof(TaggedAsset))]
	public class TaggedAssetEditor : UnityEditor.Editor
	{
		// GameObject previewSprite;

		// UnityEditor.Editor editorPreview;

		// #region Unity Message

		// public override VisualElement CreateInspectorGUI()
		// {
		// 	//Initialize(new Object[] { target, target as TaggedAsset });


		// 	return base.CreateInspectorGUI();
		// }
		// public override void DrawPreview(Rect previewArea)
		// {
		// 	base.DrawPreview(previewArea);
		// }
		// public override void OnInspectorGUI()
		// {
		// 	TaggedAsset asset = target as TaggedAsset;

		// 	GUI.enabled = false;
		// 	//EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour(target as MonoBehaviour), typeof(TaggedAsset), true);
		// 	EditorGUILayout.ObjectField("Script Editor:", MonoScript.FromScriptableObject(this), typeof(TaggedAssetEditor), true);
		// 	GUI.enabled = true;

		// 	DrawDefaultInspector();

		// 	if (previewSprite == null)
		// 	{
		// 		previewSprite = EditorUtility.CreateGameObjectWithHideFlags("Preview", HideFlags.HideAndDontSave, typeof(Image));

		// 		previewSprite.GetComponent<Image>().sprite = (target as TaggedAsset).AsSprite();
		// 	}
		// 	if (editorPreview == null)
		// 	{
		// 		editorPreview = UnityEditor.Editor.CreateEditor(previewSprite);
		// 	}
		// 	float x = EditorGUIUtility.currentViewWidth;

		// 	Rect rect = EditorGUILayout.GetControlRect();



		// 	switch (asset.eAssetType)
		// 	{
		// 		case TaggedAsset.EAssetType.Sprite:
		// 			Rect pos = new Rect(0, -Screen.height, Screen.width / 2f, Screen.height);

		// 			Texture2D text = AssetPreview.GetAssetPreview(asset.asset);
		// 			//this.DrawPreview(pos);
		// 			EditorGUI.DrawPreviewTexture(pos, text);
		// 			//editorPreview.OnPreviewGUI(pos, EditorStyles.whiteLabel);
		// 			//editorPreview.OnInteractivePreviewGUI(pos, EditorStyles.whiteLabel);
		// 			break;
		// 		case TaggedAsset.EAssetType.Texture:
		// 			break;
		// 		default:
		// 			break;
		// 	}


		// 	//myEditor = Editor.CreateEditor(previewGameObject);

		// 	//EditorGUI.pre


		// }
		// #endregion
	}
}
#endif
