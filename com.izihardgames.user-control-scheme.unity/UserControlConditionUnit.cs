using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames
{
    [CreateAssetMenu(fileName = nameof(UserControlConditionUnit), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_PRESETS_ITEMS + "/" + nameof(UserControlConditionUnit))]
    public class UserControlConditionUnit : ScriptableObject
    {
        public string title = "No Title For User Condition Unit";
        /// <summary>
        /// id of this
        /// </summary>
        public int id;
        /// <summary>
        /// id to find object in runtime
        /// </summary>
        public int idBind;
        public int type;
    }
}
