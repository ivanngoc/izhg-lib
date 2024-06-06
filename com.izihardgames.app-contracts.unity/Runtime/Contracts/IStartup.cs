using System.Threading.Tasks;

namespace IziHardGames.Apps.Abstractions.Lib
{

    public interface IStartup
    {
        public static IStartup? Startup { get; set; }
        /// <summary>
        /// Do not use that method. invoke <see cref="FinishStartupGlobal"/> Uppon startup is finished
        /// </summary>
        public void FinishStartup(IIziAppVersion1 app);
        public Task LateStartupAsync();

        public static void BeginStartupGlobal(IStartup startup)
        {
            Startup = startup;
        }
        public static void FinishStartupGlobal()
        {
            IIziAppVersion1.IsStartupFinished = true;
        }
    }
}
