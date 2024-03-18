using System;
using IziHardGames.UserControl.InputSystem.ForUnity;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static IziHardGames.ForUnityEditor.ConstantsForUnityEditor;
//extern alias CustomTypes;  

namespace IziHardGames.UserControl.ForUnityEditor
{
    public class EditorWindowForDataInput : EditorWindow
    {
        private const string TITLE = nameof(DataInput) + ".cs";
        private const string GUID_UXML_DOC = "08e20549b4c0e5a418666bbbdebdc25c";

        private VisualElement? template;
        private Label labelMousePos;
        public Vector3 MousePos;
        public Vector3 PointerScreenToWorldDirection;
        public int instances;

        [MenuItem(MENU_CATEGORY_NAME + "/Windows/For Types/" + TITLE)]
        public static void ShowMyEditor()
        {
            EditorWindowForDataInput wnd = GetWindow<EditorWindowForDataInput>();
            wnd.titleContent = new GUIContent(TITLE);

            VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(GUID_UXML_DOC));
            VisualElement ui = uiAsset.Instantiate();
            wnd.rootVisualElement.Add(ui);
            wnd.labelMousePos = ui.Q<Label>("MousePos");
            ui.Bind(new SerializedObject(wnd));
        }

        public void CreateGUI()
        {

        }

        private void Update()
        {
            var data = DataInput.forEditor;
            if (data != null)
            {
                instances = DataInput.instances;
                MousePos = data.pointerPosAtScreen3d;
                PointerScreenToWorldDirection = data.rayMainCameraToPointer.direction;
                //rootVisualElement.Bind(new SerializedObject(this));
                //labelMousePos.text = data.pointerPosAtScreen3d.ToString();
            }
        }
    }
}
