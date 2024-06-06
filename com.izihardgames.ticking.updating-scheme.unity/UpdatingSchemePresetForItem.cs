using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames
{
    [CreateAssetMenu(fileName = nameof(UpdatingSchemePresetForItem), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_PRESETS_ITEMS + "/" + nameof(UpdatingSchemePresetForItem))]
    public class UpdatingSchemePresetForItem : ScriptableObject
    {
        public string description = "NoDescription";
        public string key = "NoKey";
        /// <summary>
        /// Specify the way to obtain exemplar of update (tick) agent
        /// </summary>
        public int initType;
        public int i;
        public int j;
        /// <summary>
        /// Value to obtain exemplar in Runtime
        /// </summary>
        public int association;
    }
}
