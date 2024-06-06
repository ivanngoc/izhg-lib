using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;



namespace IziHardGames.ForUnityEditor.Windows
{
    /// <summary>
    /// Show Info About Current Domain
    /// </summary>
    public class DomainInfoWindow : IziEditorWindow
    {
        // package.json > root.displayName
        private const string PATH = "Packages/izhg.izihardgames.com.izihardgames.editor.reflections.unity/Editor/UIToolkit/";

        [MenuItem(NAME_ROOT_MENU_NAME + "/" + nameof(DomainInfoWindow))]
        public static void ShowWindow()
        {
            DomainInfoWindow w = GetWindow<DomainInfoWindow>();
            w.titleContent = new GUIContent(nameof(DomainInfoWindow));
            w.rootVisualElement.Clear();

            // VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/MyWindow.uxml");
            VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PATH + nameof(DomainInfoWindow) + ".uxml") ?? throw new NullReferenceException();
            VisualElement ui = uiAsset.Instantiate();
            w.rootVisualElement.Add(ui);
            w.Draw();
        }

        public void CreateGUI()
        {

        }

        private void Draw()
        {
            // Query by #id
            var container = rootVisualElement.Q(name: "unity-content-container") ?? throw new NullReferenceException();
            var arr = container.Children().ToArray();

            foreach (var item in container.Children())
            {
                var laber = item.Q<Label>();
                if (laber != null)
                {
                    laber.text = DateTime.Now.ToString();
                }
            }

            Label label= new Label();
            label.text = "Assemblies:";
            container.hierarchy.Add(label);

            foreach (var item in AppDomain.CurrentDomain.GetAssemblies().OrderBy(x=>x.FullName))
            {
                Label lab = new Label();
                lab.text = item.FullName;
                container.hierarchy.Add(lab);
            }
        }
    }
}
