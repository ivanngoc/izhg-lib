using System.Collections.Generic;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames
{
    [CreateAssetMenu(fileName = nameof(UserControlSetOfConditions), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_PRESETS_ITEMS + "/" + nameof(UserControlSetOfConditions))]
    public class UserControlSetOfConditions : ScriptableObject
    {
        public List<UserControlCondition> userControlCondition = new List<UserControlCondition>();


        /*
        Is Mode Enabled
        Is Action Triggered
        Is Action Filtered
        Is Action Fired (Например чтобы запустить следом следующее действие)
        */
    }
}
