using System;
using System.Collections.Generic;
using IziHardGames.Apps.Abstractions.ForUnity.Presets;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames.Apps.ForUnity.Presets
{
    [CreateAssetMenu(fileName = nameof(PresetsForTypes), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_AGGREGATORS + "/" + nameof(PresetsForTypes))]
    public class PresetsForTypes : AppPreset, IAutoRegPresetItem
    {
        [SerializeField] private ScriptableType[] types;
        public IEnumerable<ScriptableType> All => types;
		public override IEnumerable<ScriptableObject> AllAsScriptables => types;
		public Type Type { get => typeof(ScriptableType); }
    }
}