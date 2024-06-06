using System;
using System.Collections.Generic;
using IziHardGames.Apps.Abstractions.ForUnity.Presets;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames
{
    [CreateAssetMenu(fileName = nameof(TickingSchemePreset), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_AGGREGATORS + "/" + nameof(TickingSchemePreset))]
    public class TickingSchemePreset : AppPreset
    {
        [SerializeField] private List<UpdatingSchemePresetForStage> stages = new List<UpdatingSchemePresetForStage>();
        public IEnumerable<UpdatingSchemePresetForStage> All => stages;
    }
}
