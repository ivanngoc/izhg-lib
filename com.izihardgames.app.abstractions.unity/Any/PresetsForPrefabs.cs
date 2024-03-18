using System.Collections.Generic;
using System.Linq;
using IziHardGames.Apps.Abstractions.ForUnity.Presets;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames.Apps.ForUnity.Presets
{
	[CreateAssetMenu(fileName = nameof(PresetsForPrefabs), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_AGGREGATORS + "/" + nameof(PresetsForPrefabs))]
    public class PresetsForPrefabs : AppPreset
    {
        [SerializeField] private PrefabEntry[] prefabEntries;
        public PrefabEntry this[string name] => prefabEntries.First(x => x.id.idAsString == name);
        public PrefabEntry this[int id] => prefabEntries.First(x => x.id.idAsInt == id);
        public PrefabEntry this[ScriptableId id] => prefabEntries.First(x => x.id == id);
        public IEnumerable<PrefabEntry> All => prefabEntries;
    }
}