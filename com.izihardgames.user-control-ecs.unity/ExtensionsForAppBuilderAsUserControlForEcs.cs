using System;
using System.Collections.Generic;
using System.Text;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.UserControl.InputSystem.ForUnity;
using IziHardGames.Attributes;
using IziHardGames.Apps.NetStd21;

namespace IziHardGames.UserControl.ForUnity.ForEcs
{
    public static class ExtensionsForAppBuilderAsUserControlForEcs
    {
        [ExtendApp(EUseType.Dependent)]
        public static void UseRaycastEcs(this IIziAppBuilder builder)
        {
            //var promise = builder.GetSingletonPromise<DataInput>();
        }
    }
}
