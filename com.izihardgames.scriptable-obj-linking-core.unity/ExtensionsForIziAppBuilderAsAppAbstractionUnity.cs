using System;
//using IziHardGames.Apps.Abstractions.Lib;
//using IziHardGames.Apps.Abstractions.NetStd21;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{
    public static class ExtensionsForIziAppBuilderAsAppAbstractionUnity
    {
        //public static void UseScriptableTypes(this IIziAppBuilder builder)
        //{
        //    IziHandler.selector[typeof(ScriptableType)] = (x) =>
        //    {
        //        var meta = x as ScriptableType;
        //        Type? type = default;

        //        if (meta.isParseRequired)
        //        {
        //            type = Type.GetType(meta.fullName);
        //        }

        //        if (meta.isRigistInIziTypes)
        //        {
        //            IziTypes.Default[meta.Id.idAsString] = new MetadataForType()
        //            {
        //                type = type,
        //                idAsInt = meta.Id.idAsInt,
        //                idAsString = meta.Id.idAsString,
        //            };
        //        }
        //    };
        //}
        //public static void UsePresetsAppAbstractionUnity(this IIziAppBuilder builder, ProjectPresets presets)
        //{
        //    builder.AddSingleton(presets);
        //}
    }
}
