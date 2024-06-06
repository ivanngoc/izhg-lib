using System.Threading.Tasks;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Apps.ForUnity;

namespace IziHardGames.Apps.Games.ForUnity
{
    public sealed class MonoAppGameFactory : AbstractAppFactory
    {
        public override Task<IIziAppVersion1> CreateAsync(IIziAppBuilder builder)
        {
            IziAppGameMono iziAppGameMono = new IziAppGameMono(builder);
            return Task.FromResult<IIziAppVersion1>(iziAppGameMono);
        }
    }
}