using IziHardGames.Apps.ForUnity;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{
	[CreateAssetMenu(fileName = nameof(ScriptableGroupe), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_META + "/" + nameof(ScriptableGroupe))]
	public class ScriptableGroupe : ScriptableObject,IScriptableId
	{
		public ScriptableId id;
		[TextArea] public string description;
        public ScriptableId Id { get => id; set => id = value; }
    }
}