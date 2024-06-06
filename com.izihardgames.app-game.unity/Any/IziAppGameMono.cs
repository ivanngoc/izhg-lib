using System;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Apps.ForUnity;
using IziHardGames.Apps.Games.Lib;
using UnityEngine;

namespace IziHardGames.Apps.Games.ForUnity
{
    public class IziAppGameMono : IziAppGame
    {
        public IziAppGameMono(IIziAppBuilder builder) : base(builder)
        {
            OnSingletonAddOrSwapSync<Camera>((x) => AppMonoGlobalEvents.NotifyCameraChange((x as Camera) ?? throw new InvalidCastException()));
            OnSingletonRemoveSync<Camera>((x) => AppMonoGlobalEvents.NotifyNoCamera());
        }
    }
}