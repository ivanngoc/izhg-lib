using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Apps.ForUnity
{
    public class StartupArguments : MonoBehaviour
    {
        [SerializeField] private List<GameObject>? argsAsGameObjects;
        [SerializeField] private List<Component>? argsAsComponents;
        [SerializeField] private List<ScriptableObject>? argsAsScriptableObjects;
    }
}
