using IziHardGames.Apps.ForUnity;
using UnityEngine;
using static IziHardGames.Apps.Abstractions.ForUnity.Presets.ConstantsForScriptableObjects;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{
    /// <summary>
    /// https://fmoralesdev.com/2019/04/16/reflection-get-type-by-string-instance-class-from-another-assembly/
    /// https://learn.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/specifying-fully-qualified-type-names
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ScriptableType), menuName = NAME_ROOT_MENU_NAME + "/" + NAME_MENU_CATEGORY_META + "/" + nameof(ScriptableType))]
    public class ScriptableType : ScriptableObject, IScriptableId
    {
        [SerializeField] private ScriptableId id;
        public string fullName;
        public string Assembly;
        public string Namespace;
        public string TypeName;
        public bool isParseRequired;
        public bool isRigistInIziTypes;

        public ScriptableId Id { get => id; set => id = value; }
    }
}