using System.Threading.Tasks;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Apps.ForUnity;

namespace IziHardGames.Apps.Games.ForUnity
{
    public sealed class MonoAppGameFactory : AbstractAppFactory
    {
        public override Task<IIziApp> CreateAsync(IIziAppBuilder builder)
        {
            IziAppGameMono iziAppGameMono = new IziAppGameMono(builder);
            return Task.FromResult<IIziApp>(iziAppGameMono);
        }
    }
}