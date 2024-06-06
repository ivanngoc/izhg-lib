using System.Collections.Generic;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames
{
    [CreateAssetMenu(fileName = nameof(UpdatingSchemePresetForStage), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_PRESETS_ITEMS + "/" + nameof(UpdatingSchemePresetForStage))]
    public class UpdatingSchemePresetForStage : ScriptableObject
    {
        public string title = "NoTitle";
        public string nameOfStage = "NoName";
        public bool isUnityEngineGroup;
        public EInsertType insertType;
        public string typeName;
        [SerializeField] private List<UpdatingSchemePresetForItem> items = new List<UpdatingSchemePresetForItem>();
        public IEnumerable<UpdatingSchemePresetForItem> All => items;
    }

    public enum EInsertType
    {
        None = 0,
        Before,
        After,
        NoInsert,
        InsideLast,
        InsideFirst,
    }
}
