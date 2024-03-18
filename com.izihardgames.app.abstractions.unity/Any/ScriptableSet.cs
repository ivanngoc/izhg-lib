using IziHardGames.Apps.ForUnity;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{
    /// <summary>
    /// Сет рахнотипных объектов
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ScriptableSet), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_LINKERS + "/" + nameof(ScriptableSet))]
    public class ScriptableSet : ScriptableObject, IScriptableId
    {
        public ScriptableId id;
        public ScriptableGroupe groupe;
        public ScriptableObject[] scriptables;
        [TextArea] public string description;
        public ScriptableId Id { get => id; set => id = value; }
    }
}