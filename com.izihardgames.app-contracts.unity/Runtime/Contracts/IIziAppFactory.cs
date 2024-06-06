using System.Threading.Tasks;

namespace IziHardGames.Apps.Abstractions.Lib
{
    public interface IIziAppFactory
    {
        public Task<IIziAppVersion1> CreateAsync(IIziAppBuilder builder);
    }
}
