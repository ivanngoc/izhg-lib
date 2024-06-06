using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames
{
    [CreateAssetMenu(fileName = nameof(ScrUserMode), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_PRESETS_ITEMS + "/" + nameof(ScrUserMode))]
    public class ScrUserMode : ScriptableObject
    {
        public string title = "No Title For User Mode";
    }
}
