using System.Threading.Tasks;

namespace IziHardGames.Apps.Abstractions.Lib
{
    public interface IIziAppVersion1: IIziApp
    {
        public static bool IsStartupFinished { get; internal set; }
        public static IIziAppVersion1? Singleton { get; set; }
        public Task StartAsync();
        public T GetSingleton<T>() where T : class;
    }
}
