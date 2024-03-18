using System;
using IziHardGames.Apps.Abstractions.Lib;
using UnityEngine;

namespace IziHardGames.Apps.ForUnity
{
    public class MonoIntervalProviderFixed : IntervalProvider
    {
        public override void Execute()
        {
            foreach (var groupe in All)
            {
                int milliseconds = (int)(Time.fixedDeltaTime * 1000);

                foreach (var data in groupe.All)
                {
                    if (data.Decrease(milliseconds))
                    {
                        data.Fire();
                    }
                }
            }
        }
    }

    public class MonoIntervalProviderNormal : IntervalProvider
    {
        public override void Execute()
        {
            foreach (var groupe in All)
            {
                int milliseconds = (int)(Time.deltaTime * 1000);

                foreach (var data in groupe.All)
                {
                    if (data.Decrease(milliseconds))
                    {
                        data.Fire();
                    }
                }
            }
        }
    }
}