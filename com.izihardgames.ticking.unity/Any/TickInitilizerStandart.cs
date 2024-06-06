using IziHardGames.AppConstructor;
using System;
using IziHardGames.Ticking.ForUnity;
using Object = UnityEngine.Object;

namespace IziHardGames.Ticking
{
    public class TickInitilizerStandart : TickInitilizer
    {
        public override void InitilizeBegin(IziAppModuled app)
        {
            GeneratorOfUpdatesSeparate generator = Object.FindObjectOfType<GeneratorOfUpdatesSeparate>();
            app.PutItem(generator.GetType().FullName, generator);
        }
        public override void InitilizeEnd()
        {
            throw new NotImplementedException();
        }
    }
}
