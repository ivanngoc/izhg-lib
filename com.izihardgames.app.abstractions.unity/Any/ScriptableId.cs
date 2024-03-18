using System;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{

	[CreateAssetMenu(fileName = nameof(ScriptableId), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_META + "/" + nameof(ScriptableId))]
    public class ScriptableId : ScriptableObject
    {
        public int idAsInt;
        public string idAsString;
        public string guid;
        [Space]
        [TextArea] public string description;
        public Guid Guid { get => GetOrCreate(); set => guid = value.ToString(); }

        public void Reset()
        {
            guid = Guid.NewGuid().ToString();
            idAsInt = guid.GetHashCode();
            idAsString = guid;
        }

        private Guid GetOrCreate()
        {
            if (Guid.TryParse(idAsString, out var result)) return result;
            throw new NullReferenceException("No GUID assigned");
        }

        public void RenewGuid()
        {
            guid = Guid.NewGuid().ToString();
        }
        public void RenewIdInt()
        {
            idAsInt = guid.GetHashCode();
        }
        public void RenewGuidAndIdInt()
        {
            RenewGuid();
            RenewIdInt();
        }
    }
}