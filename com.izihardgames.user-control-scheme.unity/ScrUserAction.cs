using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames
{
    [CreateAssetMenu(fileName = nameof(ScrUserAction), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_PRESETS_ITEMS + "/" + nameof(ScrUserAction))]
    public class ScrUserAction : ScriptableObject
    {
        public string title = "No Title For User Action";
        public UserControlSetOfConditions? conditions;
    }
}
