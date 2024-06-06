using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{
    [CreateAssetMenu(fileName = nameof(RegistryForScriptableIds), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_AGGREGATORS + "/" + nameof(RegistryForScriptableIds))]
    public class RegistryForScriptableIds : AppPreset
    {
        public ScriptableId[] ids;
        public ScriptableObject[] dependecies;

        [ContextMenu("Renew All IdInt")]
        public void RenewIdInt()
        {
            foreach (var id in ids)
            {
                id.RenewIdInt();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(id);
#endif
            }

        }
        [ContextMenu("Renew All Guid")]
        public void RenewGuids()
        {
            foreach (var id in ids)
            {
                id.RenewGuid();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(id);
#endif
            }
        }
        [ContextMenu("Renew All Guid And IdInt")]
        public void RenewGuidsAndIntIds()
        {
            foreach (var id in ids)
            {
                id.RenewGuidAndIdInt();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(id);
#endif
            }
        }
    }
}