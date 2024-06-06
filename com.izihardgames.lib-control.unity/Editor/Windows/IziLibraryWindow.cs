using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using IziHardGames;
using IziHardGames.Libs.IziLibrary.Contracts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

public class IziLibraryWindow : IziEditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
    private readonly IziUnityHttpClient httpClient = new IziUnityHttpClient(new System.Net.Http.HttpClientHandler() { ServerCertificateCustomValidationCallback = (x, y, z, w) => System.Net.ServicePointManager.ServerCertificateValidationCallback(x, y, z, w) });

    // package.json > root.displayName
    private const string PATH = "Packages/izhg.lib-control.unity/Editor/UIToolkit";
    public const string BASE_URI = "http://localhost/libcontrol";

    [MenuItem(NAME_ROOT_MENU_NAME + "/" + nameof(IziLibraryWindow))]
    public static void ShowWindow()
    {
        IziLibraryWindow w = GetWindow<IziLibraryWindow>();
        w.titleContent = new GUIContent(nameof(IziLibraryWindow));

        //w.rootVisualElement.Clear();
        //// VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/MyWindow.uxml");
        //VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PATH + nameof(IziLibraryWindow) + ".uxml") ?? throw new NullReferenceException();
        //VisualElement ui = uiAsset.Instantiate();
        //w.rootVisualElement.Add(ui);
        //w.Draw();
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        // Get a reference to the field from UXML and append a value to it.
        var uxmlField = root.Q<TextField>("the-uxml-field");
        uxmlField.value += "..";

        // Create a new field, disable it, and give it a style class.
        var autoCompleteGuid = new TextField("Autocomplete Guid");
        autoCompleteGuid.value = "It's snowing outside...";
        autoCompleteGuid.SetEnabled(false);
        autoCompleteGuid.AddToClassList("some-styled-field");
        autoCompleteGuid.value = uxmlField.value;

        var fieldDirectory = new TextField("Directory");
        fieldDirectory.SetEnabled(false);
        fieldDirectory.AddToClassList("some-styled-field");

        var fieldFilename = new TextField("Filename");
        fieldFilename.SetEnabled(false);
        fieldFilename.AddToClassList("some-styled-field");


        root.Add(autoCompleteGuid);
        // Mirror the value of the UXML field into the C# field.
        uxmlField.RegisterCallback<ChangeEvent<string>>((evt) =>
        {
            var task = httpClient.RequestAsync<ModelAsmdef>("GET", BASE_URI + $"/api/UnityAsmdefs/search/{evt.newValue}");
            task.AsTask().Wait();
            var model = task.Result;
            if (model != null)
            {
                fieldDirectory.value = model.Directory;
                autoCompleteGuid.value = model.Guid.ToString();
            }
            else
            {
                autoCompleteGuid.value = evt.newValue;
            }
        });
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

        Label label = new Label();
        label.text = "Library:";
        container.hierarchy.Add(label);
    }
}
