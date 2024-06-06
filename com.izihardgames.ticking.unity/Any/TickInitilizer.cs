using IziHardGames.AppConstructor;

namespace IziHardGames.Ticking
{
    public abstract class TickInitilizer
    {
        public bool Complete { get; protected set; }
        public abstract void InitilizeBegin(IziAppModuled app);
        public abstract void InitilizeEnd();
    }
}
