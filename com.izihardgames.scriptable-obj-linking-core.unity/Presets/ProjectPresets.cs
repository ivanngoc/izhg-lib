using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{

    [CreateAssetMenu(fileName = nameof(ProjectPresets), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_AGGREGATORS + "/" + nameof(ProjectPresets))]
    public class ProjectPresets : ScriptableObject
    {
        [SerializeField] public ScriptableId idProject;
        [SerializeField] public string projectName;
        [SerializeField] public string variant;
        [SerializeField] public string version;
        [SerializeField] private AppPreset[] presets;
        public AppPreset this[Type type] => presets.First(x => x.GetType() == type);
        public IEnumerable<AppPreset> All => presets;
    }
}