using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor
{
    public abstract class IziEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            //var result = base.CreateInspectorGUI();
            //// Create a new VisualElement to be the root of our Inspector UI.
            VisualElement root = new VisualElement();
            UnityEditor.UIElements.InspectorElement.FillDefaultInspector(root, serializedObject, this);
            // Add a simple label.
            root.Add(new Label("This is a custom Inspector"));

            // Return the finished Inspector UI.
            return root;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
