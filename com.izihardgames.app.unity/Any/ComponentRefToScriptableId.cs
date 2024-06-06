using IziHardGames.Apps.ForUnity;
using UnityEngine;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{
	public sealed class ComponentRefToScriptableId : MonoBehaviour, IScriptableId
    {
        public ScriptableId Id { get => id; set => id = value; }
        [SerializeField] private ScriptableId id;
    }
}
