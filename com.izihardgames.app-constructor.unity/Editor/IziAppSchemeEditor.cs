using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IziHardGames.AppConstructor;
using UnityEngine.UIElements;
using System.IO;
using System;
using static IziHardGames.ForEditor.AppConstructor.AssemblyConstants;

namespace IziHardGames.ForEditor.AppConstructor
{

    [CustomEditor(typeof(IziAppScheme))]
    public class IziAppSchemeEditor : IziEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            string path = PathToUI + nameof(IziAppSchemeEditor) + ".uxml";
            VisualTreeAsset? uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);

            if (uiAsset is null)
            {
                Debug.LogError($"Can't find {nameof(VisualTreeAsset)} at path: {path}");
                var visualElement = base.CreateInspectorGUI();
                return visualElement;
            }
            else
            {
                var visualElement = base.CreateInspectorGUI();
                return visualElement;
            }
        }
    }
}
