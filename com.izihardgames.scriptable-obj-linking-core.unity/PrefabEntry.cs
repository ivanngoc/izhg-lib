using IziHardGames.Apps.Abstractions.ForUnity.Presets;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;
namespace IziHardGames.Apps.ForUnity.Presets
{
    [CreateAssetMenu(fileName = nameof(PrefabEntry), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_PRESETS_ITEMS + "/" + nameof(PrefabEntry))]
    public class PrefabEntry : ScriptableObject, IScriptableId
    {
        public ScriptableId id;
        public GameObject prefab;
        public ScriptableGroupe groupe;
        public ScriptableId Id { get => id; set => id = value; }

    }
}