using System;
using System.Linq;
using System.Threading.Tasks;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Apps.NetStd21;
using IziHardGames.Game.Abstractions.Lib;
using IziHardGames.Game.Abstractions.Logics;
using IziHardGames.Libs.NonEngine.Game.Abstractions;
using IziHardGames.UserControl.Abstractions.NetStd21;

namespace IziHardGames.Apps.Games.Lib
{
    public class IziAppGame : IziApp
    {
        private readonly GameArchytype gameArchytype = new GameArchytype();
        private IGameConrtoller gameConrtoller;
        public IGameConrtoller Conrtoller => gameConrtoller ?? throw new NullReferenceException();
        public IziAppGame(IIziAppBuilder builder) : base(builder)
        {
            foreach (var pair in builder.Singletons)
            {
                var singleton = pair.Value;
                if (singleton is IInputSystem inputSystem)
                {

                }
                else if (singleton is IGameElement gameElement)
                {

                }
                else if (singleton is ITickSystem tickSystem)
                {

                }
                else if (singleton is IGameConrtoller gc)
                {
                    gameConrtoller = gc;
                }
            }
            if (gameConrtoller is null) throw new NullReferenceException($"{typeof(IGameConrtoller).Name} must be added to singletons");
        }
        public override async Task StartAsync()
        {
            await base.StartAsync();
            var world = await gameConrtoller.StartGameAsync();
            AddSingletonTemp(world);
        }

        private void Validate()
        {
            if (!services.Any(x => x is IInputSystem))
            {
                throw new MissingMemberException("You must specify Input System!");
            }
        }
    }
}
